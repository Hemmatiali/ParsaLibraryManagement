using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Application.Validators
{
    //todo xml
    public class BookCategoryValidator : AbstractValidator<BookCategoryDto>
    {
        //todo xml
        public BookCategoryValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 50).WithMessage("Title must be between 1 and 50 characters.");

            RuleFor(x => x.ImageAddress)
                .NotEmpty().WithMessage("Image address is required.")
                .MaximumLength(37).WithMessage("Image address must be maximum 37 characters.");
        }
    }
}
