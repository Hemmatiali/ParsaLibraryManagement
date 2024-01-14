using AutoMapper;
using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Utilities;
using ParsaLibraryManagement.Domain.Common;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces.Repository;

namespace ParsaLibraryManagement.Application.Services;

public class GenderService : IGenderService
{
    private readonly IMapper _mapper;
    private readonly IValidator<GenderDto> _validator;
    private readonly IBaseRepository<Gender> _baseRepository;

    public GenderService(IValidator<GenderDto> validator, IMapper mapper, IRepositoryFactory repositoryFactory)
    {
        _validator = validator;
        _mapper = mapper;
        _baseRepository = repositoryFactory.GetRepository<Gender>();
    }

    public async Task<string?> CreateGenderAsync(GenderDto command)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return ValidationHelper.GetErrorMessages(validationResult);

            var gender = _mapper.Map<Gender>(command);
            await _baseRepository.AddAsync(gender);
            await _baseRepository.SaveChangesAsync();

            return null;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<GenderDto?> GetGenderByAsync(short id)
    {
        try
        {
            // Retrieve gender by ID
            var gender = await _baseRepository.GetByIdAsync(id);

            // Map entity to DTO and return
            return gender == null ? null : _mapper.Map<GenderDto>(gender);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<string?> UpdateGenderAsync(GenderDto command)
    {
        try
        {
            // Check if genderId has a value
            if (!command.GenderId.HasValue)
                return string.Format(ErrorMessages.NotValid, nameof(GenderDto.GenderId));

            // Get existing gender without awaiting immediately
            var genderTask = _baseRepository.GetByIdAsync(command.GenderId!.Value);

            // Validate the updated gender data without awaiting immediately
            var validationResultTask = _validator.ValidateAsync(command);

            // Concurrently await both tasks
            await Task.WhenAll(genderTask, validationResultTask);

            var gender = genderTask.Result;

            // Check if gender is null
            if (gender == null)
                return ErrorMessages.ItemNotFoundMsg;

            // Check if validation result is valid
            if (!validationResultTask.Result.IsValid)
                return ValidationHelper.GetErrorMessages(validationResultTask.Result);

            // Map DTO to existing entity and save changes
            _mapper.Map(command, gender);
            await _baseRepository.UpdateAsync(gender);
            await _baseRepository.SaveChangesAsync();

            // Successful update
            return null;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<List<GenderDto>> GetGendersAsync()
    {
        // Retrieve all genders
        var genders = await _baseRepository.GetAllAsync();

        // Map entities to DTOs and return the list
        return genders.Select(gender => _mapper.Map<GenderDto>(gender)).ToList();
    }
}