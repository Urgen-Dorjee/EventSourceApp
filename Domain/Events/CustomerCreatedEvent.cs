using MediatR;

namespace Domain.Events;


 public record CustomerCreatedEvent(Guid Id, string Name) : INotification;

