using Domain.Contacts;
using Domain.Model;
using MediatR;

namespace Application.Customers.Commands;

/// <summary>
/// Command to create a new customer.
/// </summary>
public record CreateCustomerCommand(string Name) : IRequest<Unit>;

/// <summary>
/// Handles the creation of a new customer.
/// </summary>
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Unit>
{
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IEventStore _eventStore;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCustomerCommandHandler"/> class.
    /// </summary>
    /// <param name="repository">The customer repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dispatcher">The domain event dispatcher.</param>
    /// <param name="eventStore">The event store.</param>
    public CreateCustomerCommandHandler(
        ICustomerRepository repository,
        IUnitOfWork unitOfWork,
        IDomainEventDispatcher dispatcher,
        IEventStore eventStore)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _dispatcher = dispatcher;
        _eventStore = eventStore;
    }

    /// <summary>
    /// Handles the creation of a new customer.
    /// </summary>
    /// <param name="request">The create customer command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A unit value.</returns>
    public async Task<Unit> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(request.Name);
        await _repository.CreateAsync(customer, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        await _dispatcher.RaiseEventsAsync(customer, cancellationToken);
        await _eventStore.SaveAsync(customer.Id, customer.DomainEvents, cancellationToken);
        return Unit.Value;
    }
}
