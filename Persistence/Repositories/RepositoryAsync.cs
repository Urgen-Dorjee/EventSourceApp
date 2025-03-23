using Microsoft.EntityFrameworkCore;
using Persistence.DataContext;

namespace Persistence.Repositories;

public class RepositoryAsync<T> where T : class
{
    protected readonly CustomerDbContext _context;
    protected readonly DbSet<T> _dbSet;
    public RepositoryAsync(CustomerDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    public virtual async Task CreateAsync(T entity, CancellationToken token)
    {
        await _dbSet.AddAsync(entity, token);
    }
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _dbSet.FindAsync(id, token);
    }

}
