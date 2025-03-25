using Domain.Events;
using Domain.Model;
using FluentAssertions;
using static Domain.Model.Customer;


namespace DomainTest.DomainTest;

public class CustomerTests
{
    [Fact]
    public void Create_Should_Set_Name_And_Raise_Event()
    {
        // Arrange
        var name = "John Doe";

        // Act
        var customer = Create(name);

        // Assert
        customer.Name.Should().Be(name);
        customer.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<CustomerCreatedEvent>()
            .Which.Name.Should().Be(name);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrow()
    {
        Action act = () => Create(string.Empty);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*cannot be empty*");
    }

    [Fact]
    public void When_CustomerCreatedEvent_ShouldSetState()
    {
        var customer = (Customer)Activator.CreateInstance(typeof(Customer), nonPublic: true)!;
        var @event = new CustomerCreatedEvent(Guid.NewGuid(), "Jane");

        var method = typeof(Customer).GetMethod("When", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(customer, [@event]);

        customer.Name.Should().Be("Jane");
    }

    [Fact]
    public void Update_WithEmptyName_ShouldThrowException()
    {
        var customer = Create("Valid Name");
        Action act = () => customer.Update("");

        act.Should().Throw<ArgumentException>().WithMessage("*cannot be empty*");
    }
    [Fact]
    public void Update_Should_Change_Customer_Name_And_Raise_Event()
    {
        var customer = Create("Initial Name");
        customer.ClearDomainEvents();

        customer.Update("Updated Name");

        customer.Name.Should().Be("Updated Name");
        customer.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<CustomerUpdatedEvent>()
            .Which.Name.Should().Be("Updated Name");
    }

}
