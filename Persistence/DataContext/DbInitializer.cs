using Domain.Model;
using Persistence.DataContext;

public static class DbInitializer
{
    public static async Task SeedAsync(CustomerDbContext context)
    {
        if (!context.Customers.Any())
        {
            context.Customers.AddRange(
                Customer.Create("Alice"),
                Customer.Create("Bob")
            );

            await context.SaveChangesAsync();
        }
    }
}