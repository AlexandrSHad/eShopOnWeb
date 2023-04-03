using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.Infrastructure.Events;

public class InMemoryEventDispatcher : IEventDispatcher
{
    private readonly IMessageChannel _messageChannel;

    public InMemoryEventDispatcher(IMessageChannel messageChannel)
    {
        _messageChannel = messageChannel;
    }

    public async Task PublishAsync<TMessage>(TMessage message) where TMessage : class, IEvent
    {
        await _messageChannel.Writer.WriteAsync(message);
    }
}
