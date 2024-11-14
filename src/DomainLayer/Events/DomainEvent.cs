using MediatR;

namespace DomainLayer.Events;

public interface IDomainEvent : INotification
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
}
