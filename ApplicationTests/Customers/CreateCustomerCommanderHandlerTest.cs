using Application.Customers.Commands;
using Domain.Contacts;
using Domain.Model;
using FluentAssertions;
using MediatR;
using Moq;

namespace ApplicationTests.Customers;

public class CreateCustomerCommandHandlerTests
{
    private readonly Mock<ICustomerRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IDomainEventDispatcher> _dispatcherMock = new();
    private readonly Mock<IEventStore> _eventStoreMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateCustomer_AndDispatchEvents()
    {
        // Arrange
        var command = new CreateCustomerCommand("John Doe");
        var handler = new CreateCustomerCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _dispatcherMock.Object,
            _eventStoreMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.CreateAsync(It.Is<Customer>(c => c.Name == "John Doe"), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _dispatcherMock.Verify(x => x.RaiseEventsAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        _eventStoreMock.Verify(x => x.SaveAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<INotification>>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Should().Be(Unit.Value);
    }
}
