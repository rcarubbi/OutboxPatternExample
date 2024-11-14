using ApplicationLayer.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Handlers.Events.Application;

public class ProductPriceUpdatedEventHandler : IConsumer<ProductPriceUpdated>
{
    private readonly ILogger<ProductPriceUpdatedEventHandler> logger;

    public ProductPriceUpdatedEventHandler(ILogger<ProductPriceUpdatedEventHandler> logger)
    {
        this.logger = logger;
    }

    public Task Consume(ConsumeContext<ProductPriceUpdated> context)
    {
        logger.LogInformation($"Product price updated: {context.Message.ProductId} - {context.Message.NewPrice:C2}");
        return Task.CompletedTask;
    }
}
