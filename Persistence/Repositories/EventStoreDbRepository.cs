using Domain.Contacts;
using EventStore.Client;
using MediatR;
using System.Text.Json;
using System.Text;

public class EventStoreDbRepository : IEventStore
{
    private readonly EventStoreClient _client;

    public EventStoreDbRepository(EventStoreClient client)
    {
        _client = client;
    }

    public async Task SaveAsync(Guid aggregateId, IEnumerable<INotification> events, CancellationToken cancellationToken)
    {
        var eventStream = $"customer-{aggregateId}";
        var eventData = events.Select(@event => new EventData(
            Uuid.NewUuid(),
            @event.GetType().Name,
            JsonSerializer.SerializeToUtf8Bytes(@event),
            Encoding.UTF8.GetBytes(@event.GetType().AssemblyQualifiedName!)
        ));

        await _client.AppendToStreamAsync(eventStream, StreamState.Any, eventData, cancellationToken: cancellationToken);
    }
}
