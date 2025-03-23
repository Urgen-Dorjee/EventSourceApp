using Domain.Contacts;
using MediatR;

namespace Application.Customers.Commands;

/// <summary>
/// Command to update a customer.
/// </summary>
public record UpdateCustomerCommand(Guid Id, string Name) : IRequest<Unit>
{
    /// <summary>
    /// Gets the customer ID.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// Gets the customer name.
    /// </summary>
    public string Name { get; init; } = Name;
}
/// <summary>
/// Handler for updating a customer.
/// </summary>
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
{
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IEventStore _eventStore;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCustomerCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The customer repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dispatcher">The domain event dispatcher.</param>
    /// <param name="eventStore">The event store.</param>
    public UpdateCustomerCommandHandler(ICustomerRepository repository, IUnitOfWork unitOfWork,
        IDomainEventDispatcher dispatcher, IEventStore eventStore)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _dispatcher = dispatcher;
        _eventStore = eventStore;
    }

    /// <summary>
    /// Handles the update customer command.
    /// </summary>
    /// <param name="request">The update customer command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the customer is not found.</exception>
    public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (customer is null)
            throw new KeyNotFoundException($"Customer with ID {request.Id} not found");

        customer.Update(request.Name);

        await _unitOfWork.CommitAsync(cancellationToken);
        await _dispatcher.RaiseEventsAsync(customer, cancellationToken); // dispatch event
        await _eventStore.SaveAsync(customer.Id, customer.DomainEvents, cancellationToken);
        return Unit.Value;
    }
}
