using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Domain.Common;

namespace ParsaLibraryManagement.Application.Validators
{
    /// <summary>
    ///     Validator for the <see cref="BookCategoryDto"/> class.
    /// </summary>
    /// <remarks>
    ///     This class defines validation rules for the properties of the <see cref="BookCategoryDto"/> class.
    /// </remarks>
    public class BookCategoryValidator : AbstractValidator<BookCategoryDto>
    {
        public BookCategoryValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, "Title"))
                .Length(1, 50).WithMessage(string.Format(ErrorMessages.LengthBetweenMsg, "Title", 1, 50));

            RuleFor(x => x.ImageAddress)
                .NotEmpty().WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, "Image address"))
                .MaximumLength(37).WithMessage(string.Format(ErrorMessages.MaximumLengthMsg, "Image address", 37));
        }
    }
}
