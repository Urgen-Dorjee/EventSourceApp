using Application.Customers.Contacts;
using Application.ViewModels;
using AutoMapper;
using MediatR;

namespace Application.Customers.Queries;


/// <summary>
/// Query to get all customers.
/// </summary>
public record GetAllCustomersQuery() : IRequest<List<CustomerViewModel>>;
/// <summary>
/// Handler for GetAllCustomersQuery.
/// </summary>
public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerViewModel>>
{
    private readonly ICustomerReadRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllCustomersQueryHandler"/> class.
    /// </summary>
    /// <param name="repository">The customer read repository.</param>
    /// <param name="mapper">The mapper.</param>
    public GetAllCustomersQueryHandler(ICustomerReadRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetAllCustomersQuery.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of customer view models.</returns>
    public async Task<List<CustomerViewModel>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<CustomerViewModel>>(customers);
    }
}
