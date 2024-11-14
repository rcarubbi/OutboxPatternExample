namespace ApplicationLayer.Events
{
    public sealed record ProductPriceUpdated : IApplicationEvent
    {
        public Guid ProductId { get; init; }
        public decimal NewPrice { get; init; }

        public ProductPriceUpdated(Guid productId, decimal newPrice)
        {
            ProductId = productId;
            NewPrice = newPrice;
        }
    }
}
