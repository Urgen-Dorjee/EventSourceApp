using Domain.Contacts;
using MediatR;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly IEventStore _eventStore;

    public DomainEventDispatcher(IMediator mediator, IEventStore eventStore)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    public async Task RaiseEventsAsync(AggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        if (aggregateRoot == null) throw new ArgumentNullException(nameof(aggregateRoot));

        var domainEvents = aggregateRoot.DomainEvents.ToList();

        aggregateRoot.ClearDomainEvents();

        // Save to EventStore
        await _eventStore.SaveAsync(aggregateRoot.Id, domainEvents, cancellationToken);

        // Publish to MediatR handlers
        var tasks = domainEvents.Select(e => _mediator.Publish(e, cancellationToken));
        await Task.WhenAll(tasks);
    }
}
