using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Domain.Common;

namespace ParsaLibraryManagement.Application.Validators;

/// <summary>
///     Validator for the <see cref="BookDto"/> class.
/// </summary>
/// <remarks>
///     This class defines validation rules for the properties of the <see cref="BookDto"/> class.
/// </remarks>
public class BookValidator : AbstractValidator<BookDto>
{
    // Fields
    private const int NameMaximumLength = 50;
    private const int ImageAddressMaximumLength = 37;

    // Constructor
    public BookValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(BookDto.Name)))
            .MaximumLength(NameMaximumLength)
            .WithMessage(string.Format(ErrorMessages.MaximumLengthMsg, nameof(BookDto.Name), NameMaximumLength));

        RuleFor(dto => dto.ImageAddress)
            .NotEmpty()
            .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(BookCategoryDto.ImageAddress)))
            .MaximumLength(ImageAddressMaximumLength)
            .WithMessage(string.Format(ErrorMessages.MaximumLengthMsg, nameof(BookCategoryDto.ImageAddress), ImageAddressMaximumLength));

        RuleFor(dto => dto.Status).IsInEnum();
    }
}