﻿using MediatR;
using  Domain.Events;

namespace Domain.Model;

public partial class Customer : AggregateRoot
{
    public string Name { get; private set; }

    private Customer(){}
    public static Customer Create(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Customer name cannot be empty");
        }
        var customer = new Customer { Id = Guid.NewGuid(), Name = name };
        customer.ApplyChange(new CustomerCreatedEvent(customer.Id, name));
        return customer;

    }
    public void Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Updated name cannot be empty");

        Name = name;
        ApplyChange(new CustomerUpdatedEvent(Id, name)); // raise event
    }

    protected override void When(INotification @event)
    {
        switch (@event)
        {
            case CustomerCreatedEvent e:
                Id = e.Id;
                Name = e.Name;
                break;

            case CustomerUpdatedEvent e:
                Name = e.Name;
                break;
        }
    }

}