using DomainLayer.Entities;

namespace DomainLayer.Repositories;

public interface IProductRepository
{
    Task Add(Product product, CancellationToken cancellationToken);
    Task<Product?> GetById(Guid productId, CancellationToken cancellationToken);
    Task Update(Product product, CancellationToken cancellationToken);

}