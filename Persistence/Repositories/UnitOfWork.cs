using Domain.Contacts;
using Microsoft.EntityFrameworkCore;
using Persistence.DataContext;

namespace Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CustomerDbContext _context;

    public UnitOfWork(CustomerDbContext context)
    {
        _context = context;
    }
    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
