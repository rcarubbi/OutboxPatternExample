using ApplicationLayer.Queries;
using DomainLayer.Entities;
using DomainLayer.Repositories;
using MediatR;

namespace ApplicationLayer.Handlers.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductRepository productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await productRepository.GetById(request.Id, cancellationToken);
        }
    }
}
