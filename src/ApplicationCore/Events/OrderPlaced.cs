using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Events;

public record OrderPlaced(int Id) : IEvent;
