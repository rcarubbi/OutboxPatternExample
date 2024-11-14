using ApplicationLayer.Commands;
using ApplicationLayer.Events;
using ApplicationLayer.Exceptions;
using ApplicationLayer.Services;
using DomainLayer.Repositories;
using MediatR;

namespace ApplicationLayer.Handlers.Commands;

internal sealed class UpdateProductPriceCommandHandler(IProductRepository productRepository, IEventDispatcher eventDispatcher) : IRequestHandler<UpdateProductPriceCommand>
{
    public async Task Handle(UpdateProductPriceCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetById(request.Id, cancellationToken) ?? throw new ProductNotFoundException(request.Id);
        product.ChangePrice(request.Price);
        var applicationEvent = new ProductPriceUpdated(product.Id, product.Price);
        await eventDispatcher.Dispatch(applicationEvent, cancellationToken);
        await productRepository.Update(product, cancellationToken);
    }
}
