using System.Threading.Channels;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.Infrastructure.Events;

public class MessageChannel : IMessageChannel
{
    private readonly Channel<IEvent> _messages = Channel.CreateUnbounded<IEvent>();

    public ChannelReader<IEvent> Reader => _messages.Reader;
    public ChannelWriter<IEvent> Writer => _messages.Writer;
}
