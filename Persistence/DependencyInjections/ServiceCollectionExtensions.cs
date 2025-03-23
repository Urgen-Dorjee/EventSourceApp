using Application.Customers.Contacts;
using Domain.Contacts;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;
using Persistence.Repositories.Reads;

namespace Persistence.DependencyInjections;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceService(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEventStore, EventStoreDbRepository>();
        services.AddScoped(typeof(RepositoryAsync<>));
        services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
        return services;
    }
}
