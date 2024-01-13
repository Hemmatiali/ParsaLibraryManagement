using AutoMapper;
using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Utilities;
using ParsaLibraryManagement.Domain.Common;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces.Repository;

namespace ParsaLibraryManagement.Application.Services;

///<inheritdoc cref="IPublisherServices"/>
public class PublisherService : IPublisherServices
{
    private readonly IBaseRepository<Publisher> _baseRepository;
    private readonly IValidator<PublisherDto> _validator;
    private readonly IMapper _mapper;

    public PublisherService(IValidator<PublisherDto> validator,
        IMapper mapper,
        IRepositoryFactory repositoryFactory)
    {
        _baseRepository = repositoryFactory.GetRepository<Publisher>();
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<string?> CreatePublisherAsync(PublisherDto command)
    {
        try
        {
            // Validate the publisher data
            command.Email = command.Email.CleanEmail();
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return ValidationHelper.GetErrorMessages(validationResult);

            // Map DTO to entity and save
            var publisher = _mapper.Map<Publisher>(command);
            await _baseRepository.AddAsync(publisher);
            await _baseRepository.SaveChangesAsync();

            // Successful creation
            return null;
        }
        catch (Exception e)
        {
            // Handle exceptions appropriately in the calling code
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PublisherDto?> GetPublisherByAsync(short id)
    {
        try
        {
            // Retrieve publisher by ID
            var publisher = await _baseRepository.GetByIdAsync(id);

            // Map entity to DTO and return
            return publisher == null ? null : _mapper.Map<PublisherDto>(publisher);
        }
        catch (Exception e)
        {
            // Handle exceptions appropriately in the calling code
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<string?> UpdatePublisherAsync(PublisherDto command)
    {
        try
        {
            // Check if PublisherId has a value
            if (!command.PublisherId.HasValue)
                return string.Format(ErrorMessages.NotValid, nameof(PublisherDto.PublisherId));

            // Get existing publisher without awaiting immediately
            var publisherTask = _baseRepository.GetByIdAsync(command.PublisherId!.Value);

            // Validate the updated publisher data without awaiting immediately
            command.Email = command.Email.CleanEmail();
            var validationResultTask = _validator.ValidateAsync(command);

            // Concurrently await both tasks
            await Task.WhenAll(publisherTask, validationResultTask);

            var publisher = publisherTask.Result;

            // Check if publisher is null
            if (publisher == null)
                return ErrorMessages.ItemNotFoundMsg;

            // Check if validation result is valid
            if (!validationResultTask.Result.IsValid)
                return ValidationHelper.GetErrorMessages(validationResultTask.Result);

            // Map DTO to existing entity and save changes
            var tempMail = publisher.Email;
            _mapper.Map(command, publisher);
            publisher.Email = tempMail;
            await _baseRepository.UpdateAsync(publisher);
            await _baseRepository.SaveChangesAsync();

            // Successful update
            return null;
        }
        catch (Exception e)
        {
            // Handle exceptions appropriately in the calling code
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<PublisherDto>> GetPublishersAsync()
    {
        // Retrieve all publishers
        var publishers = await _baseRepository.GetAllAsync();

        // Map entities to DTOs and return the list
        return publishers.Select(publisher => _mapper.Map<PublisherDto>(publisher)).ToList();
    }
}