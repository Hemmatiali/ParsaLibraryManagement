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

        //todo xml
        private static void NormalizeBookCategoryDto(BookCategoryDto bookCategoryDto)
        {
            bookCategoryDto.Title = bookCategoryDto.Title.NormalizeAndTrim();
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
                var booksCategories = await _baseRepository.GetAllAsync(new Expression<Func<BookCategory, object>>[] { b => b.Ref! });

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
            // Get categories
            var bookCategories = await _booksCategoryRepository.GetBookCategoriesAsync(prefix);

            // Map the categories to Dto
            return _mapper.Map<List<BookCategoryDto>>(bookCategories);
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
        public async Task<string?> UpdateCategoryAsync(BookCategoryDto bookCategoryDto, IFormFile imageFile, string folderName)
        {
            var imageNameWithExtension = "";
            try
            {
                // Get existing category
                var existingCategory = await _baseRepository.GetByIdAsync(bookCategoryDto.CategoryId);
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
                bookCategoryDto.ImageAddress = imageNameWithExtension;
                var validationResult = await _validator.ValidateAsync(bookCategoryDto);
                if (!validationResult.IsValid)
                    return ValidationHelper.GetErrorMessages(validationResult); //TODO image is uploaded and should be handled

                // Normalize values
                NormalizeBookCategoryDto(bookCategoryDto);

                // Check existence of title
                var titleExists = await _baseRepository.AnyAsync(p => p.Title.Equals(bookCategoryDto.Title) && p.CategoryId != bookCategoryDto.CategoryId);
                if (titleExists)
                    return string.Format(ErrorMessages.Exist, nameof(bookCategoryDto.Title));

                // Map DTO to existing entity and save
                _mapper.Map(bookCategoryDto, existingCategory);
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

        #endregion
    }
}
