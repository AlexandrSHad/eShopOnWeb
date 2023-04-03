using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.eShopWeb.Infrastructure.Events.CAP;

public class CapConsumerServiceSelector : ConsumerServiceSelector
{
    private readonly CapOptions _capOptions;

    public CapConsumerServiceSelector(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _capOptions = serviceProvider.GetRequiredService<IOptions<CapOptions>>().Value;
    }

    protected override IEnumerable<ConsumerExecutorDescriptor> FindConsumersFromInterfaceTypes(
        IServiceProvider provider)
    {
        var executorDescriptorList = new List<ConsumerExecutorDescriptor>();

        var types = typeof(IEventHandler<>).Assembly.GetTypes();
        var eventTypes = types
            .Where(x => x.IsClass && x.IsAssignableTo(typeof(IEvent)))
            .ToArray();

        var handlerBaseTypes = eventTypes
            .Select(x => typeof(IEventHandler<>).MakeGenericType(x))
            .ToArray();

        // for better performance register handlers in the DI ServiceProvider
        // and look for handlers by each event 
        var handlerTypes = types
            .Where(c => c.IsClass && handlerBaseTypes.Any(b => b.IsAssignableFrom(c)))
            .ToArray();

        foreach (var handlerType in handlerTypes)
        {
            executorDescriptorList.Add(GetDescriptor(handlerType.GetTypeInfo()));
        }

        return executorDescriptorList;
    }

    private ConsumerExecutorDescriptor GetDescriptor(TypeInfo typeInfo)
    {
        var name = typeInfo.ImplementedInterfaces.FirstOrDefault()?.GetGenericArguments().FirstOrDefault()?.Name!;
        var method = typeInfo.GetMethod("Handle")!;
        var parameters = method.GetParameters()
            .Select(parameter => new ParameterDescriptor
            {
                Name = parameter.Name!,
                ParameterType = parameter.ParameterType,
                // CancellationToken is provided by CAP itself
                IsFromCap = typeof(CancellationToken).IsAssignableFrom(parameter.ParameterType)
            }).ToList();

        return new ConsumerExecutorDescriptor
        {
            Attribute = new CapSubscribeAttribute(name)
            {
                Group = _capOptions.DefaultGroupName + "." + _capOptions.Version
            },
            Parameters = parameters,
            MethodInfo = method,
            ImplTypeInfo = typeInfo,
            TopicNamePrefix = _capOptions.TopicNamePrefix
        };
    }
}
