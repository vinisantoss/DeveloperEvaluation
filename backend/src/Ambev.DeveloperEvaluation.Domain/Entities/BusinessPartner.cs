using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a business partner using External Identity pattern.
/// </summary>
public class BusinessPartner : BaseEntity
{
    /// <summary>
    /// External identifier from the BusinessPartner
    /// </summary>
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Business partner name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Business partner email.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Business partner document.
    /// </summary>
    public string Document { get; set; } = string.Empty;

    /// <summary>
    /// Performs validation of the business partner entity using the BusinessPartnerValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new BusinessPartnerValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}