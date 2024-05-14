namespace Ordering.Domain.Models;

public class Customer : Entity<CustomerId>
{
    public string Name { get; init; } = "";
    public string Email { get; init; } = "";

    public static Customer Create(CustomerId id, string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        var customer = new Customer
        {
            Id = id,
            Name = name,
            Email = email
        };

        return customer;
    }
}
