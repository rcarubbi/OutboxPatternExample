using DomainLayer.Events;

namespace DomainLayer.Entities;

public class Product(string name, decimal price)
    : AggregateRoot(Guid.NewGuid())
{
    public string Name { get; private set; } = name;
    public decimal Price { get; private set; } = price;

    public void ChangePrice(decimal newPrice)
    {
        var oldPrice = Price;
        Price = newPrice;
        RaiseEvent(new ProductPriceChanged(Id, oldPrice, newPrice));
    }
}
