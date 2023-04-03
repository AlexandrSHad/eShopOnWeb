using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace Microsoft.eShopWeb.Infrastructure.Events;

public class InMemoryEventDispatcherJob : BackgroundService
{
    private readonly IMessageChannel _messageChannel;
    private readonly IMediator _mediator;

    public InMemoryEventDispatcherJob(IMessageChannel messageChannel, IMediator mediator)
    {
        _messageChannel = messageChannel;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach(var @event in _messageChannel.Reader.ReadAllAsync(stoppingToken))
        {
            await _mediator.Publish(@event, stoppingToken);
        }
    }
}
