using Domain.Model;

namespace Domain.Contacts;

public interface ICustomerRepository
{
    Task CreateAsync(Customer customer, CancellationToken cancellationToken);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
