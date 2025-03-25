using MediatR;

namespace Domain.Contacts;

public interface IEventStore
{
    Task SaveAsync(Guid aggregateId, IEnumerable<INotification> events, CancellationToken cancellationToken);
    Task<T?> LoadAsync<T>(Guid aggregateId) where T : AggregateRoot, new();
}
