using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Domain.Common;

namespace ParsaLibraryManagement.Application.Validators;

public class GenderValidator:AbstractValidator<GenderDto>
{
    private const int TitleMaximumLength= 10;
    private const int TitleMinimumLength= 1;
    
    private const int CodeMaximumLength= 10;
    private const int CodeMinimumLength= 1;

    public GenderValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(GenderDto.Title)))
            .Length(TitleMinimumLength, TitleMaximumLength).WithMessage(string.Format(ErrorMessages.LengthBetweenMsg,
                nameof(GenderDto.Title), TitleMaximumLength, TitleMinimumLength));
        
        RuleFor(dto => dto.Code)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(GenderDto.Code)))
            .Length(TitleMinimumLength, TitleMaximumLength).WithMessage(string.Format(ErrorMessages.LengthBetweenMsg,
                nameof(GenderDto.Code), CodeMaximumLength, CodeMinimumLength));
    }
}