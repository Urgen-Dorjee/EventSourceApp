using MediatR;

namespace Domain.Contacts;

public interface IEventStore
{
    Task SaveAsync(Guid aggregateId, IEnumerable<INotification> events, CancellationToken cancellationToken);
}
