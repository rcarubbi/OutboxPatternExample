namespace DomainLayer.Events;

public class ProductPriceChanged : IDomainEvent
{
    public ProductPriceChanged(Guid productId, decimal oldValue, decimal newValue)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        ProductId = productId;
        OldValue = oldValue;
        NewValue = newValue;
    }

    public Guid Id { get; }

    public Guid ProductId { get; }

    public DateTime CreatedAt { get; }

    public decimal OldValue { get; }

    public decimal NewValue { get; }
}
