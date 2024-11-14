using DomainLayer.Entities;
using DomainLayer.Repositories;

namespace InfrastructureLayer.Database.Repositories;

internal class ProductRepository(ApplicationDbContext dbContext) : IProductRepository
{
    public async Task Add(Product product, CancellationToken cancellationToken)
    {
        await dbContext.Products.AddAsync(product, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Product?> GetById(Guid productId, CancellationToken cancellationToken)
    {
        return await dbContext.Products.FindAsync(productId, cancellationToken);
    }

    public async Task Update(Product product, CancellationToken cancellationToken)
    {
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
