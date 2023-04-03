using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.Infrastructure.Events.CAP;

public class CapEventDispatcher : IEventDispatcher
{
    private readonly ICapPublisher _publisher;

    public CapEventDispatcher(ICapPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
    {
        await _publisher.PublishAsync(@event.GetType().Name, @event);
    }
}
