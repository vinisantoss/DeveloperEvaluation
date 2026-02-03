using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Common;

/// <summary>
/// Base class for domain entities, providing identity, validation
/// and comparison behavior.
/// </summary>
public class BaseEntity : IComparable<BaseEntity>
{
    /// <summary>
    /// Unique identifier of the entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Executes domain validation rules for the current entity instance.
    /// </summary>
    /// <returns>
    /// A collection of validation error details, if any rule is violated.
    /// </returns>
    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }

    /// <summary>
    /// Compares the current entity with another entity based on its identifier.
    /// </summary>
    /// <param name="other">
    /// The entity to compare against.
    /// </param>
    /// <returns>
    /// A value indicating the relative order of the entities.
    /// </returns>
    public int CompareTo(BaseEntity? other)
    {
        if (other is null)
        {
            return 1;
        }

        return other.Id.CompareTo(Id);
    }
}
