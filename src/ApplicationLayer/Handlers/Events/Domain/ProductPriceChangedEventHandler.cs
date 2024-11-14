using DomainLayer.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Handlers.Events.Domain;

internal class ProductPriceChangedEventHandler : INotificationHandler<ProductPriceChanged>
{
    private readonly ILogger<ProductPriceChangedEventHandler> logger;

    public ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger)
    {
        this.logger = logger;
    }

    public Task Handle(ProductPriceChanged notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Price changed event received for product {ProductId}", notification.ProductId);
        return Task.CompletedTask;
    }
}
