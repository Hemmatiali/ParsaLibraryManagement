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
        #region Fields

        private const int TitleMinimumLength = 1;
        private const int TitleMaximumLength = 50;

        private const int ImageAddressMaximumLength = 37;

        #endregion

        #region Ctor

        public BookCategoryValidator()
        {
            RuleFor(dto => dto.Title)
                .NotEmpty()
                .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(BookCategoryDto.Title)))
                .Length(TitleMinimumLength, TitleMaximumLength)
                .WithMessage(string.Format(ErrorMessages.LengthBetweenMsg, nameof(BookCategoryDto.Title), TitleMinimumLength, TitleMaximumLength));

            RuleFor(dto => dto.ImageAddress)
                .NotEmpty()
                .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(BookCategoryDto.ImageAddress)))
                .MaximumLength(ImageAddressMaximumLength)
                .WithMessage(string.Format(ErrorMessages.MaximumLengthMsg, nameof(BookCategoryDto.ImageAddress), ImageAddressMaximumLength));
        }

        #endregion

    }
}
