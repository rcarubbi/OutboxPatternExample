using DomainLayer.Entities;
using MediatR;

namespace ApplicationLayer.Queries;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<Product?>;