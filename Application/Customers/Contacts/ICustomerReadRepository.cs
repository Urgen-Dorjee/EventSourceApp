using Domain.Model;

namespace Application.Customers.Contacts;

/// <summary>
/// Interface for reading customer data.
/// </summary>
public interface ICustomerReadRepository
{
    /// <summary>
    /// Gets all customers asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of customer view models.</returns>
    Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a customer by ID asynchronously.
    /// </summary>
    /// <param name="id">Customer ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Customer view model.</returns>
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
