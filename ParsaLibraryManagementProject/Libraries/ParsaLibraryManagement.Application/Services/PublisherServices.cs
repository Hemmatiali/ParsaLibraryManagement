using AutoMapper;
using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Utilities;
using ParsaLibraryManagement.Domain.Common;
using ParsaLibraryManagement.Domain.Common.Extensions;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Domain.Interfaces.Repository;
using System.Linq.Expressions;

namespace ParsaLibraryManagement.Application.Services;

///<inheritdoc cref="IPublisherServices"/>
public class PublisherServices : IPublisherServices
{
    #region Fields

    private readonly IMapper _mapper;
    private readonly IValidator<PublisherDto> _validator;
    private readonly IBaseRepository<Publisher> _basePublisherRepository;
    private readonly IBaseRepository<Gender> _baseGenderRepository;
    private readonly IPublisherRepository _publisherRepository;

    #endregion

    #region Ctor

    public PublisherServices(IMapper mapper, IValidator<PublisherDto> validator, IRepositoryFactory repositoryFactory,
        IPublisherRepository publisherRepository)
    {
        _mapper = mapper;
        _validator = validator;
        _basePublisherRepository = repositoryFactory.GetRepository<Publisher>();
        _baseGenderRepository = repositoryFactory.GetRepository<Gender>();
        _publisherRepository = publisherRepository;
    }

    #endregion

    #region Methods

    #region Private

    /// <summary>
    ///     Normalizes and trims the string properties of the provided <see cref="PublisherDto"/>.
    /// </summary>
    /// <param name="publisherDto">The <see cref="PublisherDto"/> to be normalized.</param>
    private static void NormalizePublisherDto(PublisherDto publisherDto)
    {
        publisherDto.FirstName = publisherDto.FirstName.NormalizeAndTrim();
        publisherDto.LastName = publisherDto.LastName.NormalizeAndTrim();
        publisherDto.Email = publisherDto.Email.NormalizeAndTrim();
    }

    #endregion

    #region Retrieval

    /// <inheritdoc />
    public async Task<PublisherDto?> GetPublisherByIdAsync(Guid publisherId)
    {
        try
        {
            // Get publisher
            var publisher = await _basePublisherRepository.GetByIdAsync(publisherId);

            // Map entity to DTO and return
            return publisher == null ? null : _mapper.Map<PublisherDto>(publisher);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<PublisherDto>> GetAllPublishersAsync()
    {
        try
        {
            // Retrieve all publishers
            var publishers = await _basePublisherRepository.GetAllAsync(new Expression<Func<Publisher, object>>[]
                { p => p.Gender });
            //todo order by insertDate or updateDate

            // Map publishers to DTOs and return the list
            return publishers.Select(publisher => _mapper.Map<PublisherDto>(publisher)).ToList();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    #endregion

    #region Checking

    /// <inheritdoc />
    public async Task<bool> IsEmailUniqueAsync(string emailAddress)
    {
        try
        {
            // Check uniqueness of email address
            return await _publisherRepository.IsEmailUniqueAsync(emailAddress);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> DoesGenderExistAsync(byte genderId)
    {
        try
        {
            // Check existence of gender Id
            return await _baseGenderRepository.AnyAsync(g => g.GenderId == genderId);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    #endregion

    #region Modification

    /// <inheritdoc />
    public async Task<string?> CreatePublisherAsync(PublisherDto publisherDto)
    {
        try
        {
            // Validate DTO
            var validationResult = await _validator.ValidateAsync(publisherDto);
            if (!validationResult.IsValid)
                return ValidationHelper.GetErrorMessages(validationResult);

            // Normalize values
            NormalizePublisherDto(publisherDto);

            // Check existence of email
            var emailExists = await _basePublisherRepository.AnyAsync(p => p.Email.Equals(publisherDto.Email));
            if (emailExists)
                return string.Format(ErrorMessages.Exist, nameof(PublisherDto.Email));

            // Map DTO to existing entity and save
            var publisher = _mapper.Map<Publisher>(publisherDto);
            await _basePublisherRepository.AddAsync(publisher);
            await _basePublisherRepository.SaveChangesAsync();

            // Done
            return null;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<string?> UpdatePublisherAsync(PublisherDto publisherDto)
    {
        try
        {
            // Check if PublisherId has a value
            if (publisherDto.PublisherId == Guid.Empty)
                return string.Format(ErrorMessages.NotValid, nameof(PublisherDto.PublisherId));

            // Get existing publisher
            var existingPublisher = await _basePublisherRepository.GetByIdAsync(publisherDto.PublisherId);
            if (existingPublisher == null)
                return ErrorMessages.ItemNotFoundMsg;

            // Validate DTO
            var validationResult = await _validator.ValidateAsync(publisherDto);
            if (!validationResult.IsValid)
                return ValidationHelper.GetErrorMessages(validationResult);

            // Normalize values
            NormalizePublisherDto(publisherDto);

            // Check existence of email
            var emailExists = await _basePublisherRepository.AnyAsync(p =>
                p.Email.Equals(publisherDto.Email) && p.PublisherId != publisherDto.PublisherId);
            if (emailExists)
                return string.Format(ErrorMessages.Exist, nameof(PublisherDto.Email));

            // Map DTO to existing entity and save
            _mapper.Map(publisherDto, existingPublisher);
            await _basePublisherRepository.UpdateAsync(existingPublisher);
            await _basePublisherRepository.SaveChangesAsync();

            // Done
            return null;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    #endregion

    #region Deletion

    /// <inheritdoc />
    public async Task<string?> DeletePublisherAsync(Guid publisherId)
    {
        try
        {
            // Check for child relations
            var hasChildRelationResult = await _publisherRepository.HasChildRelations(publisherId);
            if (hasChildRelationResult.WasSuccess || !string.IsNullOrWhiteSpace(hasChildRelationResult.Message))
                return hasChildRelationResult.Message;

            // Get existing publisher
            var publisher = await _basePublisherRepository.GetByIdAsync(publisherId);
            if (publisher == null)
                return ErrorMessages.ItemNotFoundMsg;

            // Delete the publisher
            await _basePublisherRepository.RemoveAsync(publisher);
            await _basePublisherRepository.SaveChangesAsync();

            // Done
            return null;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    #endregion

    #endregion
}