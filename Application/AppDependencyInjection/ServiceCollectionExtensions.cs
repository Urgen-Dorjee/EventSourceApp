// File: Application/AppDependencyInjections/ServiceCollectionExtensions.cs
using Application.Customers.Commands;
using Application.Customers.Queries;
using Application.MapperConfig;
using Microsoft.Extensions.DependencyInjection;

namespace Application.AppDependencyInjection;

/// <summary>
/// Provides extension methods for configuring application services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds application-specific services to the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> for chaining additional calls.</returns>
    /// <remarks>
    /// This method configures the following services:
    /// <list type="bullet">
    ///   <item><description>AutoMapper for object mapping.</description></item>
    ///   <item><description>MediatR for handling commands and queries.</description></item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(CustomerMappingProfile));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommandHandler).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateCustomerCommandHandler).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllCustomersQueryHandler).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCustomerByIdQueryHandler).Assembly));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}