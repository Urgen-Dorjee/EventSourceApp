using Domain.Model;
using MediatR;

namespace Application.Customers.Events;

/// <summary>
/// Handles the event when a customer is created.
/// </summary>
public class CustomerCreatedEventHandler : INotificationHandler<Customer.CustomerCreatedEvent>
{
    /// <summary>
    /// Handles the customer created event.
    /// </summary>
    /// <param name="notification">The event notification containing customer details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task Handle(Customer.CustomerCreatedEvent notification, CancellationToken cancellationToken)
    {
        // You can log, send email, push to message bus, etc.
        Console.WriteLine($"Customer Created: {notification.Name}");
        return Task.CompletedTask;
    }
}
