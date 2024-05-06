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
using ParsaLibraryManagement.Domain.Interfaces.ImageServices;
using ParsaLibraryManagement.Domain.Interfaces.Repository;

namespace ParsaLibraryManagement.Application.Services;

public class BooksServices : IBooksServices
{
    #region Fields

    private readonly IMapper _mapper;
    private readonly IImageServices _imageServices;
    private readonly IValidator<BookDto> _validator;
    private readonly IBaseRepository<Book> _bookBaseRepository;
    private readonly IBaseRepository<Publisher> _publisherBaseRepository;
    private readonly IImageFileValidationService _imageFileValidationServices;
    private readonly IBaseRepository<BookCategory> _bookCategoryBaseRepository;

    #endregion

    #region Ctor

    public BooksServices(IMapper mapper, IImageServices imageServices, IValidator<BookDto> validator, IImageFileValidationService imageFileValidationService, IRepositoryFactory repositoryFactory)
    {
        _mapper = mapper;
        _imageServices = imageServices;
        _validator = validator;
        _imageFileValidationServices = imageFileValidationService;
        _bookBaseRepository = repositoryFactory.GetRepository<Book>();
        _bookCategoryBaseRepository = repositoryFactory.GetRepository<BookCategory>();
        _publisherBaseRepository = repositoryFactory.GetRepository<Publisher>();
    }

    #endregion

    #region Methods

    #region Private

    private async Task<bool> CheckBookCategoryExistence(BookDto command)
        => await _bookCategoryBaseRepository.AnyAsync(category => category.CategoryId.Equals(command.CategoryId));

    private async Task<bool> CheckPublisherExistence(BookDto command)
        => await _publisherBaseRepository.AnyAsync(publisher => publisher.PublisherId.Equals(command.PublisherId));

    private static void NormalizeBookDto(BookDto bookDto)
        => bookDto.Name = bookDto.Name.NormalizeAndTrim();

    #endregion

    #region Retrieval

    /// <inheritdoc/>
    public async Task<BookDto?> GetBookByIdAsync(int bookId)
    {
        try
        {
            // Get book
            var book = await _bookBaseRepository.GetByIdAsync(bookId);

            // Map entity to DTO and return
            return book == null ? null : _mapper.Map<BookDto>(book);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<BookDto>> GetAllBooksAsync()
    {
        try
        {
            // Retrieve all books
            var books = await _bookBaseRepository.GetAllAsync();

            // Map books to DTOs and return the list
            return books.Select(_mapper.Map<BookDto>).ToList();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    #endregion

    #region Modification

    /// <inheritdoc/>
    public async Task<string?> CreateBookAsync(BookDto bookDto, IFormFile imageFile, string folderName)
    {
        // Validate publisherId
        if (!await CheckPublisherExistence(bookDto))
            return string.Format(ErrorMessages.NotValidMsg, nameof(Publisher));

        // Validate categoryId
        if (!await CheckBookCategoryExistence(bookDto))
            return string.Format(ErrorMessages.NotValidMsg, nameof(BookCategory));

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
            bookDto.ImageAddress = imageNameWithExtension;
            var validationResult = await _validator.ValidateAsync(bookDto);
            if (!validationResult.IsValid)
                return ValidationHelper.GetErrorMessages(validationResult);

            // Normalize values
            NormalizeBookDto(bookDto);

            // Map DTO to entity and save
            var book = _mapper.Map<Book>(bookDto);
            await _bookBaseRepository.AddAsync(book);
            await _bookBaseRepository.SaveChangesAsync();

            // Done
            return null;
        }
        catch (Exception e)
        {
            await _imageServices.DeleteImageAsync(imageNameWithExtension, folderName);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<string?> UpdateBookAsync(BookDto bookDto, IFormFile? imageFile, string? folderName)
    {
        // Validate publisherId
        if (!await CheckPublisherExistence(bookDto))
            return string.Format(ErrorMessages.NotValidMsg, nameof(Publisher));

        // Validate categoryId
        if (!await CheckBookCategoryExistence(bookDto))
            return string.Format(ErrorMessages.NotValidMsg, nameof(BookCategory));

        var imageNameWithExtension = "";
        try
        {
            // Get existing book
            var existingBook = await _bookBaseRepository.GetByIdAsync(bookDto.Id);
            if (existingBook == null)
                return ErrorMessages.ItemNotFoundMsg;

            // Handle image uploaded if exist
            if (imageFile != null)
            {
                // Delete old image if it exists
                if (!string.IsNullOrWhiteSpace(existingBook.ImageAddress))
                    await _imageServices.DeleteImageAsync(existingBook.ImageAddress, folderName);

                // Validate and upload new image
                var fileValidationResult = await _imageFileValidationServices.ValidateFileAsync(imageFile);
                if (fileValidationResult.WasSuccess == false ||
                    !string.IsNullOrWhiteSpace(fileValidationResult.Message))
                    return fileValidationResult.Message;

                // Handle image upload
                imageNameWithExtension = await _imageServices.SaveImageAsync(imageFile, folderName);
                if (string.IsNullOrWhiteSpace(imageNameWithExtension))
                    return ErrorMessages.ImageUploadFailedMsg;
            }
            else
                imageNameWithExtension = existingBook.ImageAddress;

            // Validate DTO
            bookDto.ImageAddress = imageNameWithExtension;
            var validationResult = await _validator.ValidateAsync(bookDto);
            if (!validationResult.IsValid)
                return ValidationHelper.GetErrorMessages(validationResult);

            // Map DTO to entity and save
            var book = _mapper.Map(bookDto, existingBook);
            await _bookBaseRepository.UpdateAsync(book);
            await _bookBaseRepository.SaveChangesAsync();

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

    /// <inheritdoc/>
    public async Task<string?> DeleteBookAsync(int bookId, string? folderName)
    {
        try
        {
            // Get existing book
            var existingBook = await _bookBaseRepository.GetByIdAsync(bookId);
            if (existingBook == null)
                return ErrorMessages.ItemNotFoundMsg;

            // Delete old image if it exists
            if (!string.IsNullOrWhiteSpace(existingBook.ImageAddress))
                await _imageServices.DeleteImageAsync(existingBook.ImageAddress, folderName);

            // Delete the book
            await _bookBaseRepository.RemoveAsync(existingBook);
            await _bookBaseRepository.SaveChangesAsync();

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