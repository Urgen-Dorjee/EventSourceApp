using Domain.Events;
using MediatR;

namespace Application.Customers.Events;

/// <summary>
/// Handles the event when a customer is updated.
/// </summary>
public class CustomerUpdatedEventHandler : INotificationHandler<CustomerUpdatedEvent>
{
    /// <summary>
    /// Handles the CustomerUpdatedEvent.
    /// </summary>
    /// <param name="notification">The event notification.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task Handle(CustomerUpdatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"✅ Customer Updated: {notification.Name}");
        return Task.CompletedTask;
    }
}
