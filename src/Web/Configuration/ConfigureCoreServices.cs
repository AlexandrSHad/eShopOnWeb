using DotNetCore.CAP.Internal;
using Microsoft.eShopWeb.ApplicationCore.EventHandlers;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Data.Queries;
using Microsoft.eShopWeb.Infrastructure.Events.CAP;
using Microsoft.eShopWeb.Infrastructure.Logging;
using Microsoft.eShopWeb.Infrastructure.Services;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Microsoft.eShopWeb.Web.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IBasketQueryService, BasketQueryService>();

        var catalogSettings = configuration.Get<CatalogSettings>() ?? new CatalogSettings();
        services.AddSingleton<IUriComposer>(new UriComposer(catalogSettings));

        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddTransient<IEmailSender, EmailSender>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(OrderPlacedHandler).Assembly));

        services.AddSingleton<IConsumerServiceSelector, CapConsumerServiceSelector>();
        services.AddCap(options =>
            options
                .UseInMemoryStorage()
                .UseInMemoryMessageQueue()
                .UseDashboard());

        return services;
    }
}
