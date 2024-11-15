using DomainLayer.Events;

namespace DomainLayer.Entities;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(Guid id)
        : base(id)
    {
    }

    protected AggregateRoot()
    {
    }


    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList().AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}
