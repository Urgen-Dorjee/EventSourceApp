using Application.Customers.Commands;
using Application.ViewModels;
using AutoMapper;
using Domain.Model;

namespace Application.MapperConfig;

/// <summary>
/// Profile for mapping Customer related objects.
/// </summary>
public class CustomerMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerMappingProfile"/> class.
    /// </summary>
    public CustomerMappingProfile()
    {
        CreateMap<CustomerViewModel, CreateCustomerCommand>();
        CreateMap<Customer, CustomerViewModel>();
    }
}
