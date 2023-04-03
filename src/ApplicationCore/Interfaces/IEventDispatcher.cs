using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IEventDispatcher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
}
