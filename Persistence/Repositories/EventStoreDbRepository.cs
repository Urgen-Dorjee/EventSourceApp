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

        var eventData = events.Select(@event =>
        {
            var json = JsonSerializer.SerializeToUtf8Bytes((object)@event);
            return new EventData(
                Uuid.NewUuid(),
                @event.GetType().Name,
                json,
                Encoding.UTF8.GetBytes(@event.GetType().AssemblyQualifiedName!)
            );
        });

        try
        {
            await _client.AppendToStreamAsync(eventStream, StreamState.Any, eventData, cancellationToken: cancellationToken);
        }
        catch (Grpc.Core.RpcException ex)
        {
            throw new InvalidOperationException("Failed to connect to EventStoreDB. Is it running on localhost:2113?", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error while writing to EventStore stream '{eventStream}'.", ex);
        }
    }
}
