using System.Threading;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Events;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.ApplicationCore.EventHandlers;

public class OrderPlacedHandler : IEventHandler<OrderPlaced>
{
    private readonly ILogger<OrderPlacedHandler> _logger;

    public OrderPlacedHandler(ILogger<OrderPlacedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderPlaced @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled Order placed event for order with Id: {Id}", @event.Id);
        return Task.CompletedTask;
    }
}
