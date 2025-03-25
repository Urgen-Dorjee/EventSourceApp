using Domain.Contacts;
using EventStore.Client;
using MediatR;
using System.Text;
using System.Text.Json;

public class EventStoreDbRepository : IEventStore
{
    private readonly EventStoreClient _eventStoreClient;

    public EventStoreDbRepository(EventStoreClient eventStoreClient)
    {
        _eventStoreClient = eventStoreClient;
    }

    public async Task<T?> LoadAsync<T>(Guid aggregateId) where T : AggregateRoot, new()
    {
        if (aggregateId == Guid.Empty)
            throw new ArgumentException(nameof(aggregateId));

        var streamName = GetStreamName<T>(aggregateId);
        var aggregate = new T();

        var readStream = _eventStoreClient.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);

        if (await readStream.ReadState == ReadState.StreamNotFound)
            return null;

        var events = await (from @event in readStream
                            let json = Encoding.UTF8.GetString(@event.Event.Data.ToArray())
                            let type = Type.GetType(Encoding.UTF8.GetString(@event.Event.Metadata.ToArray())!)
                            select JsonSerializer.Deserialize(json, type!) as INotification
                            into deserialized
                            where deserialized != null
                            select deserialized).ToListAsync();

        aggregate.LoadFromHistory(events!);
        return aggregate;
    }

    public async Task SaveAsync(Guid aggregateId, IEnumerable<INotification> events, CancellationToken cancellationToken)
    {
        var streamName = $"Customer-{aggregateId}";

        var eventData = events.Select(@event => new EventData(
            Uuid.NewUuid(),
            @event.GetType().Name,
            JsonSerializer.SerializeToUtf8Bytes((object)@event),
            Encoding.UTF8.GetBytes(@event.GetType().AssemblyQualifiedName!)
        ));

        await _eventStoreClient.AppendToStreamAsync(streamName, StreamState.Any, eventData, cancellationToken: cancellationToken);
    }

    private static string GetStreamName<T>(Guid id) => $"{typeof(T).Name}-{id}";
}