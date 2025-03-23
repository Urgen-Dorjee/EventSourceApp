using Domain.Contacts;
using Domain.Model;
using Persistence.DataContext;

namespace Persistence.Repositories;

public class CustomerRepository : RepositoryAsync<Customer>, ICustomerRepository
{
    public CustomerRepository(CustomerDbContext context) : base(context)
    { }
}
