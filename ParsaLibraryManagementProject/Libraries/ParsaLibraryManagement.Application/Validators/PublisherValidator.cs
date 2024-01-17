using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Domain.Common;
using System.Text.RegularExpressions;

namespace ParsaLibraryManagement.Application.Validators;

/// <summary>
///     Validator for the <see cref="PublisherDto"/> class.
/// </summary>
/// <remarks>
///     This class defines validation rules for the properties of the <see cref="PublisherDto"/> class.
/// </remarks>
public class PublisherValidator : AbstractValidator<PublisherDto>
{
    #region Fields

    private const int FirstNameLastNameMinimumLength = 1;
    private const int FirstNameLastNameMaximumLength = 30;

    #endregion

    #region Ctor

    public PublisherValidator()
    {
        RuleFor(dto => dto.FirstName)
            .NotEmpty()
            .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(PublisherDto.FirstName)))
            .Length(FirstNameLastNameMinimumLength, FirstNameLastNameMaximumLength)
            .WithMessage(string.Format(ErrorMessages.LengthBetweenMsg, nameof(PublisherDto.FirstName), FirstNameLastNameMinimumLength, FirstNameLastNameMaximumLength));

        RuleFor(dto => dto.LastName)
            .NotEmpty()
            .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(PublisherDto.LastName)))
            .Length(FirstNameLastNameMinimumLength, FirstNameLastNameMaximumLength)
            .WithMessage(string.Format(ErrorMessages.LengthBetweenMsg, nameof(PublisherDto.LastName), FirstNameLastNameMinimumLength, FirstNameLastNameMaximumLength));


        RuleFor(dto => dto.Email)
            .NotEmpty()
            .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(PublisherDto.Email)))
            .EmailAddress()
            .WithMessage(string.Format(ErrorMessages.NotValid, nameof(PublisherDto.Email)))
            .Must(BeAValidEmail)
            .WithMessage(string.Format(ErrorMessages.NotValid, nameof(PublisherDto.Email)));
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Validates whether the provided string is a valid email address using a stricter regex pattern.
    /// </summary>
    /// <param name="email">The email address to be validated.</param>
    /// <returns>
    /// <c>true</c> if the provided string is a valid email address; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     This method uses a regular expression for stricter email validation, checking for the basic structure of an email address.
    /// </remarks>
    private bool BeAValidEmail(string email)
    {
        // Regex for stricter email validation
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    #endregion
}