using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Utilities;
using ParsaLibraryManagement.Domain.Common;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Domain.Interfaces.Repository;

namespace ParsaLibraryManagement.Application.Validators;

public class PublisherValidator : AbstractValidator<PublisherDto>
{
    private readonly IBaseRepository<Gender> _baseRepository;
    private readonly IPublisherRepository _publisherRepository;
    public PublisherValidator(IRepositoryFactory repositoryFactory, IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
        _baseRepository = repositoryFactory.GetRepository<Gender>();

        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(PublisherDto.Email)))
            .EmailAddress().WithMessage(string.Format(ErrorMessages.NotValid, nameof(PublisherDto.Email)))
            .MustAsync(IsEmailUniqueInDatabase).WithMessage(string.Format(ErrorMessages.DuplicatedValue,nameof(PublisherDto.Email)));

        RuleFor(dto => dto.FirstName)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(PublisherDto.FirstName)))
            .Length(1, 50)
            .WithMessage(string.Format(ErrorMessages.LengthBetweenMsg, nameof(PublisherDto.FirstName), 1, 50));

        RuleFor(dto => dto.LastName)
            .NotEmpty().WithMessage(string.Format(ErrorMessages.RequiredFieldMsg, nameof(PublisherDto.LastName)))
            .Length(1, 50)
            .WithMessage(string.Format(ErrorMessages.LengthBetweenMsg, nameof(PublisherDto.LastName), 1, 50));

        RuleFor(dto => dto.GenderId)
            .MustAsync(ExistInDatabase)
            .WithMessage(string.Format(ErrorMessages.NotValid, nameof(PublisherDto.GenderId)));
    }

    private async Task<bool> IsEmailUniqueInDatabase(PublisherDto publisherDto, string email, CancellationToken cancellationToken)
    {
        // Use TryGetPublisherByAsync method to reduce unnecessary exceptions
        var (isSuccess,publisher) = await _publisherRepository.TryGetPublisherByAsync(email, cancellationToken);
        if (isSuccess)
        {
            return !(publisherDto.PublisherId == null && publisher != null);
        }

        return true;
    }



    private async Task<bool> ExistInDatabase(short genderId, CancellationToken cancellationToken)
    {
        var gender = await _baseRepository.GetByIdAsync(genderId);
        return gender != null;
    }
}