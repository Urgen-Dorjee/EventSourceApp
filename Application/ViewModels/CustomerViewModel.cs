using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels;

/// <summary>
/// Represents a customer view model for API operations
/// </summary>
public class CustomerViewModel
{
    /// <summary>
    /// Unique identifier for the customer
    /// </summary>
    /// <example>550e8400-e29b-41d4-a716-446655440000</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Full name of the customer
    /// </summary>
    /// <example>John Doe</example>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}