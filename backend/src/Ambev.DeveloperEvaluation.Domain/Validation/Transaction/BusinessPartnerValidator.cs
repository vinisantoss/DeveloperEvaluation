using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for BusinessPartner entity
/// </summary>
public class BusinessPartnerValidator : AbstractValidator<BusinessPartner>
{
    /// <summary>
    /// Initializes validation rules for BusinessPartner
    /// </summary>
    public BusinessPartnerValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithMessage("External ID is required")
            .MaximumLength(50)
            .WithMessage("External ID must not exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address")
            .MaximumLength(100)
            .WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.Document)
            .NotEmpty()
            .WithMessage("Document is required")
            .MinimumLength(11)
            .WithMessage("Document must have at least 11 characters")
            .MaximumLength(18)
            .WithMessage("Document must not exceed 18 characters");
    }
}