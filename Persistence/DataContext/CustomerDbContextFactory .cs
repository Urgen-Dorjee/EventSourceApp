// File: /Persistence/CustomerDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Persistence.DataContext;

namespace Persistence;

public class CustomerDbContextFactory : IDesignTimeDbContextFactory<CustomerDbContext>
{
    public CustomerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDb;Database=SopWebApp;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new CustomerDbContext(optionsBuilder.Options);
    }
}
