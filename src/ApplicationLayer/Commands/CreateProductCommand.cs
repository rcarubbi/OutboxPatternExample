using MediatR;

namespace ApplicationLayer.Commands;

public sealed record CreateProductCommand(string Name, decimal Price) : IRequest<Guid>;
