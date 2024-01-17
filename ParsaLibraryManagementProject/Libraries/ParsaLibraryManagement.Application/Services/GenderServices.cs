using AutoMapper;
using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces.Repository;

namespace ParsaLibraryManagement.Application.Services;

///<inheritdoc cref="IGenderService"/>
public class GenderServices : IGenderService
{
    #region Fields

    private readonly IMapper _mapper;
    private readonly IValidator<GenderDto> _validator;
    private readonly IBaseRepository<Gender> _baseRepository;

    #endregion

    #region Ctor

    public GenderServices(IMapper mapper, IValidator<GenderDto> validator, IRepositoryFactory repositoryFactory)
    {
        _mapper = mapper;
        _validator = validator;
        _baseRepository = repositoryFactory.GetRepository<Gender>();
    }

    #endregion

    #region Methods

    #region Retrieval

    /// <inheritdoc />
    public async Task<List<GenderDto>> GetAllGendersAsync()
    {
        try
        {
            // Retrieve all genders
            var genders = await _baseRepository.GetAllAsync();

            // Map entities to DTOs and return the list
            return genders.Select(gender => _mapper.Map<GenderDto>(gender)).ToList();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    #endregion

    #endregion
}