using System.Threading.Channels;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.Infrastructure.Events;

public interface IMessageChannel
{
    public ChannelReader<IEvent> Reader { get; }
    public ChannelWriter<IEvent> Writer { get; }
}
