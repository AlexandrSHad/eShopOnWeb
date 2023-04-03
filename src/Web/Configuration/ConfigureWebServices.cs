using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Events;
using Microsoft.eShopWeb.Infrastructure.Events.CAP;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.Services;

namespace Microsoft.eShopWeb.Web.Configuration;

public static class ConfigureWebServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(BasketViewModelService).Assembly));
        services.AddScoped<IBasketViewModelService, BasketViewModelService>();
        services.AddScoped<CatalogViewModelService>();
        services.AddScoped<ICatalogItemViewModelService, CatalogItemViewModelService>();
        services.Configure<CatalogSettings>(configuration);
        services.AddScoped<ICatalogViewModelService, CachedCatalogViewModelService>();

        services.AddSingleton<IMessageChannel, MessageChannel>();
        // Manually implemented in memory event dispatcher
        //services.AddSingleton<IEventDispatcher, InMemoryEventDispatcher>();
        //services.AddHostedService<InMemoryEventDispatcherJob>();
        // Event dispatcher based on CAP libriary. Supports InMemory, RabbitMQ and many more
        services.AddSingleton<IEventDispatcher, CapEventDispatcher>();

        return services;
    }
}
