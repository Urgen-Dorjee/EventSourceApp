namespace Domain.Contacts;

public interface IDomainEventDispatcher
{
    Task RaiseEventsAsync(AggregateRoot aggregateRoot, CancellationToken cancellationToken);
}
