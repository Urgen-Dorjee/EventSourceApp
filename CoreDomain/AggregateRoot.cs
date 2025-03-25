using MediatR;

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

    // 🔥 This is the missing method
    public void LoadFromHistory(IEnumerable<INotification> historyEvents)
    {
        foreach (var @event in historyEvents)
        {
            When(@event); // apply historical event without tracking it
        }
    }
}
