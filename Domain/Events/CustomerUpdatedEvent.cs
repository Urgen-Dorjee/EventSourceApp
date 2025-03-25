using MediatR;

namespace Domain.Events;


public record CustomerUpdatedEvent(Guid Id, string Name) : INotification;

