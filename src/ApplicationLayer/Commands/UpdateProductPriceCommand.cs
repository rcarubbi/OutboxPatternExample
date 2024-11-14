using MediatR;

namespace ApplicationLayer.Commands;

public sealed record UpdateProductPriceCommand(Guid Id, decimal Price) : IRequest;
