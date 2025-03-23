using Application.Customers.Contacts;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistence.DataContext;

namespace Persistence.Repositories.Reads;

public class CustomerReadRepository : ICustomerReadRepository
{
    private readonly CustomerDbContext _context;

    public CustomerReadRepository(CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Customers.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers.FindAsync([id], cancellationToken);
    }
}
