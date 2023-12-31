using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Interfaces.ImageServices;
using ParsaLibraryManagement.Application.Utilities;
using ParsaLibraryManagement.Domain.Common;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Domain.Interfaces.ImageServices;
using ParsaLibraryManagement.Domain.Interfaces.Repository;

namespace ParsaLibraryManagement.Application.Services
{
    ///<inheritdoc cref="IBookCategoryServices"/>
    public class BookCategoryServices : IBookCategoryServices
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IValidator<BookCategoryDto> _validator;
        private readonly IBaseRepository<BooksCategory> _baseRepository;
        private readonly IBooksCategoryRepository _booksCategoryRepository;
        private readonly IImageFileValidationService _imageFileValidationServices;
        private readonly IImageServices _imageServices;

        #endregion

        #region Ctor

        public BookCategoryServices(IMapper mapper, IValidator<BookCategoryDto> validator, IRepositoryFactory repositoryFactory, IBooksCategoryRepository booksCategoryRepository, IImageFileValidationService imageFileValidationServices, IImageServices imageServices)
        {
            _mapper = mapper;
            _validator = validator;
            _baseRepository = repositoryFactory.GetRepository<BooksCategory>();
            _booksCategoryRepository = booksCategoryRepository;
            _imageFileValidationServices = imageFileValidationServices;
            _imageServices = imageServices;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public async Task<BookCategoryDto?> GetCategoryByIdAsync(short categoryId)
        {
            try
            {
                // Get category
                var category = await _baseRepository.GetByIdAsync(categoryId);
                return category == null ? null :
                    // Map the category to Dto
                    _mapper.Map<BookCategoryDto>(category);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<List<BookCategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                // Get categories
                var categories = await _baseRepository.GetAllAsync();

                // Map the categories to Dto
                var categoryDtos = categories.Select(c => _mapper.Map<BookCategoryDto>(c)).ToList();
                return categoryDtos;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<List<BookCategoryDto>> GetCategoriesAsync(string prefix)
        {
            // Get categories
            var categories = await _booksCategoryRepository.GetBookCategoriesAsync(prefix);

            // Map the categories to Dto
            return _mapper.Map<List<BookCategoryDto>>(categories);
        }

        /// <inheritdoc />
        public async Task<string?> CreateCategoryAsync(BookCategoryDto categoryDto, IFormFile imageFile, string folderName)
        {
            var imageNameWithExtension = "";
            try
            {
                // Validate image file
                var fileValidationResult = await _imageFileValidationServices.ValidateFileAsync(imageFile);
                if (fileValidationResult.WasSuccess == false || !string.IsNullOrWhiteSpace(fileValidationResult.Message))
                    return fileValidationResult.Message;

                // Handle image upload
                imageNameWithExtension = await _imageServices.SaveImageAsync(imageFile, folderName);
                if (string.IsNullOrWhiteSpace(imageNameWithExtension))
                    return ErrorMessages.ImageUploadFailedMsg;

                // Validate DTO
                categoryDto.ImageAddress = imageNameWithExtension;
                var validationResult = await _validator.ValidateAsync(categoryDto);
                if (!validationResult.IsValid)
                    return ValidationHelper.GetErrorMessages(validationResult);

                // Map DTO to entity and save
                var category = _mapper.Map<BooksCategory>(categoryDto);
                category.ImageAddress = imageNameWithExtension; // Set the image path

                await _baseRepository.AddAsync(category);
                await _baseRepository.SaveChangesAsync();

                // Done
                return null;
            }
            catch (Exception e)
            {
                await _imageServices.DeleteImageAsync(imageNameWithExtension, folderName);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<string?> UpdateCategoryAsync(short id, BookCategoryDto categoryDto, IFormFile imageFile, string folderName)
        {
            var imageNameWithExtension = "";
            try
            {
                // Get existing category
                var existingCategory = await _baseRepository.GetByIdAsync(id);
                if (existingCategory == null)
                    return ErrorMessages.ItemNotFoundMsg;

                // Delete old image if it exists
                if (!string.IsNullOrWhiteSpace(existingCategory.ImageAddress))
                    await _imageServices.DeleteImageAsync(existingCategory.ImageAddress, folderName);

                // Validate and upload new image
                var fileValidationResult = await _imageFileValidationServices.ValidateFileAsync(imageFile);
                if (fileValidationResult.WasSuccess == false || !string.IsNullOrWhiteSpace(fileValidationResult.Message))
                    return fileValidationResult.Message;

                // Handle image upload
                imageNameWithExtension = await _imageServices.SaveImageAsync(imageFile, folderName);
                if (string.IsNullOrWhiteSpace(imageNameWithExtension))
                    return ErrorMessages.ImageUploadFailedMsg;

                // Validate DTO
                categoryDto.ImageAddress = imageNameWithExtension;
                var validationResult = await _validator.ValidateAsync(categoryDto);
                if (!validationResult.IsValid)
                    return ValidationHelper.GetErrorMessages(validationResult);

                // Map DTO to existing entity and save
                _mapper.Map(categoryDto, existingCategory);
                await _baseRepository.UpdateAsync(existingCategory);
                await _baseRepository.SaveChangesAsync();

                // Done
                return null;
            }
            catch (Exception e)
            {
                await _imageServices.DeleteImageAsync(imageNameWithExtension, folderName);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<string?> DeleteCategoryAsync(short categoryId, string folderName)
        {
            try
            {
                // Check for child relations
                var hasChildRelationResult = await _booksCategoryRepository.HasChildRelations(categoryId);
                if (hasChildRelationResult.WasSuccess || !string.IsNullOrWhiteSpace(hasChildRelationResult.Message))
                    return hasChildRelationResult.Message;

                // Get existing category
                var existingCategory = await _baseRepository.GetByIdAsync(categoryId);
                if (existingCategory == null)
                    return ErrorMessages.ItemNotFoundMsg;

                // Delete old image if it exists
                if (!string.IsNullOrWhiteSpace(existingCategory.ImageAddress))
                    await _imageServices.DeleteImageAsync(existingCategory.ImageAddress, folderName);

                // Delete the category
                await _baseRepository.RemoveAsync(existingCategory);
                await _baseRepository.SaveChangesAsync();

                // Done
                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        #endregion
    }
}
