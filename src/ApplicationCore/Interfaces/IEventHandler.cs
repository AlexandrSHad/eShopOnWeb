using MediatR;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : class, IEvent
{
}
