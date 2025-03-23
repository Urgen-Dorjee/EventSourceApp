using MediatR;

namespace Domain;
public abstract class AggregateRoot
{
    public Guid Id { get; protected set; }
    private readonly List<INotification> _domainEvent = new();
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvent.AsReadOnly();

    protected void ApplyChange(INotification @event)
    {
        _domainEvent.Add(@event);
        When(@event);
    }
    protected abstract void When(INotification @event);
    public void ClearDomainEvents() => _domainEvent.Clear();
}
