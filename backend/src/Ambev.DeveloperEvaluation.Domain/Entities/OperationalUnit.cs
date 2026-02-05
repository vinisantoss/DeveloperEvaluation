using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an operational unit using External Identity pattern
/// </summary>
public class OperationalUnit : BaseEntity
{
    /// <summary>
    /// External identifier from the OperationalUnit
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// OperationalUnit name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// OperationalUnit location
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Performs validation of the operational unit entity using the OperationalUnitValidator rules.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new OperationalUnitValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}