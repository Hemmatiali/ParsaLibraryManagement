using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Interfaces.ImageServices;
using ParsaLibraryManagement.Application.Utilities;
using ParsaLibraryManagement.Domain.Common;
using ParsaLibraryManagement.Domain.Common.Extensions;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Domain.Interfaces.ImageServices;
using ParsaLibraryManagement.Domain.Interfaces.Repository;
using ParsaLibraryManagement.Domain.Models;
using System.Linq.Expressions;

namespace ParsaLibraryManagement.Application.Services
{
    ///<inheritdoc cref="IBookCategoryServices"/>
    public class BookCategoryServices : IBookCategoryServices
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IValidator<BookCategoryDto> _validator;
        private readonly IBaseRepository<BookCategory> _baseRepository;
        private readonly IBooksCategoryRepository _booksCategoryRepository;
        private readonly IImageFileValidationService _imageFileValidationServices;
        private readonly IImageServices _imageServices;

        #endregion

        #region Ctor

        public BookCategoryServices(IMapper mapper, IValidator<BookCategoryDto> validator, IRepositoryFactory repositoryFactory, IBooksCategoryRepository booksCategoryRepository, IImageFileValidationService imageFileValidationServices, IImageServices imageServices)
        {
            _mapper = mapper;
            _validator = validator;
            _baseRepository = repositoryFactory.GetRepository<BookCategory>();
            _booksCategoryRepository = booksCategoryRepository;
            _imageFileValidationServices = imageFileValidationServices;
            _imageServices = imageServices;
        }

        #endregion

        #region Methods

        #region Private

        /// <summary>
        ///     Normalizes and trims the title of a Book Category DTO.
        /// </summary>
        /// <param name="bookCategoryDto">The Book Category DTO to normalize.</param>
        /// <remarks>This method modifies the Title property of the provided Book Category DTO.</remarks>
        private static void NormalizeBookCategoryDto(BookCategoryDto bookCategoryDto)
        {
            bookCategoryDto.Title = bookCategoryDto.Title.NormalizeAndTrim();
        }

        /// <summary>
        ///     Checks if adding a category would create a circular hierarchy.
        /// </summary>
        /// <param name="categoryId">The ID of the category to be added.</param>
        /// <param name="parentCategoryId">The ID of the parent category.</param>
        /// <returns>True if a circular hierarchy would be created, otherwise false.</returns>
        /// <remarks>
        ///     This method checks if adding a category with the specified parent would create a circular hierarchy.
        /// </remarks>
        private async Task<bool> IsCircularHierarchy(short categoryId, short? parentCategoryId)
        {
            try
            {
                // Check if a parent category ID is provided
                if (!parentCategoryId.HasValue)
                    return false;

                var currentCategoryId = parentCategoryId;

                // Iterate through the hierarchy to check for circular reference
                while (currentCategoryId.HasValue)
                {
                    // Checks if the current category ID matches the target category ID
                    if (currentCategoryId.Value == categoryId)
                        return true;

                    // Retrieves the current category based on the current category ID
                    var currentCategory = await _baseRepository.GetByIdAsync(currentCategoryId.Value);

                    // Updates currentCategoryId to its parent category ID
                    currentCategoryId = currentCategory?.RefId;
                }

                return false;
            }
            catch (Exception a)
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets the IDs of descendant categories for a given category.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve descendants for.</param>
        /// <returns>A list of descendant category IDs.</returns>
        /// <remarks>
        ///     This method asynchronously retrieves the IDs of descendant categories for a given category.
        /// </remarks>
        private async Task<List<short>> GetDescendantCategoryIdsAsync(short categoryId)
        {
            try
            {
                // Initializes an empty list to store descendant category IDs
                var descendants = new List<short>();

                // Retrieves child categories based on the provided category ID
                var childCategories = await _baseRepository.GetAllAsync(cd => cd.RefId == categoryId);

                // Iterates through child categories to collect descendant IDs
                foreach (var child in childCategories)
                {
                    // Adds the ID of the current child category to the descendants list
                    descendants.Add(child.CategoryId);

                    // Recursively calls the method to get descendants of the current child
                    descendants.AddRange(await GetDescendantCategoryIdsAsync(child.CategoryId));
                }

                // Returns the list of descendant category IDs
                return descendants;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region Retrieval

        /// <inheritdoc />
        public async Task<BookCategoryDto?> GetCategoryByIdAsync(short bookCategoryId)
        {
            try
            {
                // Get category
                var booksCategory = await _baseRepository.GetByIdAsync(bookCategoryId);

                // Map entity to DTO and return
                return booksCategory == null ? null : _mapper.Map<BookCategoryDto>(booksCategory);
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
                // Retrieve all categories
                var booksCategories = await _baseRepository.GetAllAsync(null, new Expression<Func<BookCategory, object>>[] { b => b.Ref! });

                // Map categories to DTOs and return the list
                return booksCategories.Select(bookCategory => _mapper.Map<BookCategoryDto>(bookCategory)).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<List<BookCategoryDto>> GetCategoriesAsync(string prefix)
        {
            try
            {
                // Get categories
                var bookCategories = await _booksCategoryRepository.GetBookCategoriesAsync(prefix);

                // Map the categories to Dto
                return _mapper.Map<List<BookCategoryDto>>(bookCategories);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<List<BookCategoryDto>> GetCategoriesForEditAsync(short categoryId)
        {
            try
            {
                // Get all categories
                var allCategories = await _baseRepository.GetAllAsync();

                // Get all descendant categories
                var descendants = await GetDescendantCategoryIdsAsync(categoryId);

                // Exclude the current category and its descendants
                var filteredCategories = allCategories.Where(c => c.CategoryId != categoryId && !descendants.Contains(c.CategoryId));

                // Return mapped categories
                return filteredCategories.Select(c => _mapper.Map<BookCategoryDto>(c)).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region Modification

        /// <inheritdoc />
        public async Task<string?> CreateCategoryAsync(BookCategoryDto bookCategoryDto, IFormFile imageFile, string folderName)
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
                bookCategoryDto.ImageAddress = imageNameWithExtension;
                var validationResult = await _validator.ValidateAsync(bookCategoryDto);
                if (!validationResult.IsValid)
                    return ValidationHelper.GetErrorMessages(validationResult);

                // Normalize values
                NormalizeBookCategoryDto(bookCategoryDto);

                // Check existence of title
                var titleExists = await _baseRepository.AnyAsync(p => p.Title.Equals(bookCategoryDto.Title));
                if (titleExists)
                    return string.Format(ErrorMessages.Exist, nameof(bookCategoryDto.Title));

                // Map DTO to entity and save
                var bookCategory = _mapper.Map<BookCategory>(bookCategoryDto);
                await _baseRepository.AddAsync(bookCategory);
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
        public async Task<OperationResultModel> UpdateCategoryAsync(BookCategoryDto bookCategoryDto, IFormFile imageFile, string folderName)
        {
            var imageNameWithExtension = "";
            try
            {
                // Get existing category
                var existingCategory = await _baseRepository.GetByIdAsync(bookCategoryDto.CategoryId);
                if (existingCategory == null)
                    return new OperationResultModel { WasSuccess = false, Message = ErrorMessages.ItemNotFoundMsg };

                // Circular hierarchy check
                if (await IsCircularHierarchy(bookCategoryDto.CategoryId, bookCategoryDto.RefId))
                    return new OperationResultModel { WasSuccess = false, Message = ErrorMessages.CircularHierarchyMsg };

                // Handle image uploaded if exist
                if (imageFile != null)
                {
                    // Delete old image if it exists
                    if (!string.IsNullOrWhiteSpace(existingCategory.ImageAddress))
                        await _imageServices.DeleteImageAsync(existingCategory.ImageAddress, folderName);

                    // Validate and upload new image
                    var fileValidationResult = await _imageFileValidationServices.ValidateFileAsync(imageFile);
                    if (fileValidationResult.WasSuccess == false ||
                        !string.IsNullOrWhiteSpace(fileValidationResult.Message))
                        return new OperationResultModel { WasSuccess = false, Message = fileValidationResult.Message };

                    // Handle image upload
                    imageNameWithExtension = await _imageServices.SaveImageAsync(imageFile, folderName);
                    if (string.IsNullOrWhiteSpace(imageNameWithExtension))
                        return new OperationResultModel { WasSuccess = false, Message = ErrorMessages.ImageUploadFailedMsg };
                }
                else
                    imageNameWithExtension = existingCategory.ImageAddress;

                // Validate DTO
                bookCategoryDto.ImageAddress = imageNameWithExtension;
                var validationResult = await _validator.ValidateAsync(bookCategoryDto);
                if (!validationResult.IsValid)
                    return new OperationResultModel { WasSuccess = false, Message = ValidationHelper.GetErrorMessages(validationResult) }; //TODO image is uploaded and should be handled

                // Normalize values
                NormalizeBookCategoryDto(bookCategoryDto);

                // Check existence of title
                var titleExists = await _baseRepository.AnyAsync(p => p.Title.Equals(bookCategoryDto.Title) && p.CategoryId != bookCategoryDto.CategoryId);
                if (titleExists)
                    return new OperationResultModel { WasSuccess = false, Message = string.Format(ErrorMessages.Exist, nameof(bookCategoryDto.Title)) };

                // Map DTO to existing entity and save
                _mapper.Map(bookCategoryDto, existingCategory);
                await _baseRepository.UpdateAsync(existingCategory);
                await _baseRepository.SaveChangesAsync();

                // Done
                return new OperationResultModel { WasSuccess = true };
            }
            catch (Exception e)
            {
                await _imageServices.DeleteImageAsync(imageNameWithExtension, folderName);
                throw;
            }
        }

        #endregion

        #region Deletion

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
                {
                    var deletionResult = await _imageServices.DeleteImageAsync(existingCategory.ImageAddress, folderName);
                    if (!deletionResult)
                        return ErrorMessages.ImageDeleteFailedMsg;
                }

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

        #endregion
    }
}
