using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Domain.Common;

namespace ParsaLibraryManagement.Application.Validators;

/// <summary>
///     Validator for the <see cref="GenderDto"/> class.
/// </summary>
/// <remarks>
///     This class defines validation rules for the properties of the <see cref="GenderDto"/> class.
/// </remarks>
public class GenderValidator : AbstractValidator<GenderDto>
{
    #region Fields

    private const int TitleMinimumLength = 1;
    private const int TitleMaximumLength = 10;

    private const int CodeMinimumLength = 1;
    private const int CodeMaximumLength = 10;

    #endregion

    #region Ctor

    public GenderValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty()
            .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(GenderDto.Title)))
            .Length(TitleMinimumLength, TitleMaximumLength)
            .WithMessage(string.Format(ErrorMessages.LengthBetweenMsg, nameof(BookCategoryDto.Title), TitleMinimumLength, TitleMaximumLength));

        RuleFor(dto => dto.Code)
            .NotEmpty()
            .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(GenderDto.Code)))
            .Length(CodeMinimumLength, CodeMaximumLength).WithMessage(string.Format(ErrorMessages.LengthBetweenMsg, nameof(GenderDto.Code), CodeMinimumLength, CodeMaximumLength));
    }

    #endregion
}