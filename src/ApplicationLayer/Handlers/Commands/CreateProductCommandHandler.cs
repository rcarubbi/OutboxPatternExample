using ApplicationLayer.Commands;
using DomainLayer.Entities;
using DomainLayer.Repositories;
using MediatR;

namespace ApplicationLayer.Handlers.Commands;

internal sealed class CreateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product(command.Name, command.Price);

        await productRepository.Add(product, cancellationToken);

        return product.Id;
    }
}
