using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Domain.Common;

namespace ParsaLibraryManagement.Application.Validators
{
    /// <summary>
    ///     Validator for the <see cref="BorrowedBookDto"/> class.
    /// </summary>
    /// <remarks>
    ///     This class defines validation rules for the properties of the <see cref="BorrowedBookDto"/> class.
    /// </remarks>
    public class BorrowedBookValidator : AbstractValidator<BorrowedBookDto>
    {
        #region Fields

    
        #endregion

        #region Ctor

        public BorrowedBookValidator()
        {
            RuleFor(dto => dto.UserId)
                .NotEmpty()
                .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(BorrowedBookDto.UserId)));

            RuleFor(dto => dto.BookId)
                .NotEmpty()
                .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(BorrowedBookDto.BookId)));


            RuleFor(dto => dto.StartDateBorrowed)
             .NotEmpty()
             .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(BorrowedBookDto.StartDateBorrowed)));

         

        }

        #endregion

    }


    /// <summary>
    ///     Validator for the <see cref="BorrowedBookDto"/> class.
    /// </summary>
    /// <remarks>
    ///     This class defines validation rules for the properties of the <see cref="BorrowedBookDto"/> class.
    /// </remarks>
    public class BorrowedBookEditValidator : AbstractValidator<BorrowedBookEditDto>
    {
        #region Fields


        #endregion

        #region Ctor

        public BorrowedBookEditValidator()
        {

            RuleFor(dto => dto.UserId)
              .NotEmpty()
              .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(BorrowedBookDto.UserId)));

            RuleFor(dto => dto.BookId)
                .NotEmpty()
                .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(BorrowedBookDto.BookId)));




            RuleFor(dto => dto.BackEndDate)
                .NotEmpty()
                .WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(BorrowedBookDto.BackEndDate)));

           


        }

        #endregion

    }
}
