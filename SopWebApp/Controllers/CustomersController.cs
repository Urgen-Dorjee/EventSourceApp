using Application.Customers.Commands;
using Application.Customers.Queries;
using Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Handles customer-related operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the CustomersController
    /// </summary>
    /// <param name="mediator">Mediator service for handling CQRS operations</param>
    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;

    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="command">Customer creation data</param>
    /// <returns>Confirmation of customer creation</returns>
    /// <response code="200">Customer created successfully</response>
    /// <response code="400">Invalid request payload</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerViewModel command)
    {
        await _mediator.Send(new CreateCustomerCommand(command.Name));
        return Ok();
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <param name="model">Customer update data</param>
    /// <returns>Confirmation of customer update</returns>
    /// <response code="200">Customer updated successfully</response>
    /// <response code="400">Invalid request payload</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerViewModel model)
    {
        await _mediator.Send(new UpdateCustomerCommand(id, model.Name));
        return Ok();
    }

    /// <summary>
    /// Retrieves all customers
    /// </summary>
    /// <returns>List of customers</returns>
    /// <response code="200">Returns the list of customers</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllCustomersQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <returns>Customer details</returns>
    /// <response code="200">Returns the customer details</response>
    /// <response code="404">Customer not found</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetCustomerByIdQuery(id));
        return result is not null ? Ok(result) : NotFound();
    }
}