using Application.Customers.Commands;
using Application.Customers.Queries;
using Application.ViewModels;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class CustomersControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CustomersController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreateCustomer_Should_Send_Command_And_Return_Ok()
    {
        // Arrange
        var viewModel = new CustomerViewModel { Name = "John Doe" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), default))
                     .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.CreateCustomer(viewModel);

        // Assert
        result.Should().BeOfType<OkResult>();
        _mediatorMock.Verify(m => m.Send(It.Is<CreateCustomerCommand>(cmd => cmd.Name == viewModel.Name), default), Times.Once);
    }

    [Fact]
    public async Task UpdateCustomer_Should_Send_Command_And_Return_Ok()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var viewModel = new CustomerViewModel { Name = "Updated Name" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateCustomerCommand>(), default))
                     .ReturnsAsync(Unit.Value);

        // Act
        var result = await _controller.UpdateCustomer(customerId, viewModel);

        // Assert
        result.Should().BeOfType<OkResult>();
        _mediatorMock.Verify(m => m.Send(It.Is<UpdateCustomerCommand>(cmd =>
            cmd.Id == customerId && cmd.Name == viewModel.Name), default), Times.Once);
    }


    [Fact]
    public async Task GetAll_ReturnsOkWithCustomerList()
    {
        // Arrange
        var customers = new List<CustomerViewModel> {
            new CustomerViewModel { Name = "Alice" },
            new CustomerViewModel { Name = "Bob" }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCustomersQuery>(), default))
            .ReturnsAsync(customers);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(customers);
    }

    [Fact]
    public async Task GetById_ReturnsOkIfFound()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new CustomerViewModel { Name = "Charlie" };

        _mediatorMock.Setup(m => m.Send(new GetCustomerByIdQuery(customerId), default))
            .ReturnsAsync(customer);

        // Act
        var result = await _controller.GetById(customerId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(customer);
    }

    [Fact]
    public async Task GetById_ReturnsNotFoundIfNull()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(new GetCustomerByIdQuery(customerId), default))
            .ReturnsAsync((CustomerViewModel?)null);

        // Act
        var result = await _controller.GetById(customerId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}