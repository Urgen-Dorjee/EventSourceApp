// Application/Customers/Queries/GetCustomerByIdQueryHandler.cs
using Application.Customers.Contacts;
using Application.ViewModels;
using AutoMapper;
using MediatR;

namespace Application.Customers.Queries;

/// <summary>
/// Query to get a customer by Id.
/// </summary>
public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerViewModel?>
{
    /// <summary>
    /// Gets the Id of the customer.
    /// </summary>
    public Guid Id { get; init; } = Id;
}
/// <summary>
/// Handler for GetCustomerByIdQuery.
/// </summary>
public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerViewModel?>
{
    private readonly ICustomerReadRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCustomerByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="repository">The customer read repository.</param>
    /// <param name="mapper">The mapper.</param>
    public GetCustomerByIdQueryHandler(ICustomerReadRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetCustomerByIdQuery.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The customer view model.</returns>
    public async Task<CustomerViewModel?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return customer is null ? null : _mapper.Map<CustomerViewModel>(customer);
    }
}
