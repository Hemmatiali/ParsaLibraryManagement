using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Interfaces.ImageServices;
using ParsaLibraryManagement.Application.Utilities;
using ParsaLibraryManagement.Domain.Common;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces.ImageServices;
using ParsaLibraryManagement.Domain.Interfaces.Repository;

namespace ParsaLibraryManagement.Application.Services;

public class BooksServices : IBooksServices
{
    private readonly IMapper _mapper;
    private readonly IImageServices _imageServices;
    private readonly IValidator<BookDto> _validator;
    private readonly IBaseRepository<Book> _bookBaseRepository;
    private readonly IBaseRepository<Publisher> _publisherBaseRepository;
    private readonly IImageFileValidationService _imageFileValidationServices;
    private readonly IBaseRepository<BookCategory> _bookCategoryBaseRepository;

    public BooksServices(IMapper mapper,
        IImageServices imageServices,
        IValidator<BookDto> validator,
        IImageFileValidationService imageFileValidationService,
        IRepositoryFactory repositoryFactory)
    {
        this._mapper = mapper;
        this._imageServices = imageServices;
        this._validator = validator;
        this._imageFileValidationServices = imageFileValidationService;
        this._bookBaseRepository = repositoryFactory.GetRepository<Book>();
        this._bookCategoryBaseRepository = repositoryFactory.GetRepository<BookCategory>();
        this._publisherBaseRepository = repositoryFactory.GetRepository<Publisher>();
    }

    /// <inheritdoc/>
    public async Task<string?> AddBookAsync(BookDto command, IFormFile imageFile, string folderName)
    {
        if (!(await CheckPublisherExistence(command)))
            return string.Format(ErrorMessages.NotValid, nameof(BookDto.Name));

        if (!(await CheckBookCategoryExistence(command)))
            return string.Format(ErrorMessages.NotValid, nameof(BookDto.CategoryId));

        var imageNameWithExtension = "";
        try
        {
            var fileValidationResult = await _imageFileValidationServices.ValidateFileAsync(imageFile);
            if (fileValidationResult.WasSuccess == false || !string.IsNullOrWhiteSpace(fileValidationResult.Message))
                return fileValidationResult.Message;

            imageNameWithExtension = await _imageServices.SaveImageAsync(imageFile, folderName);
            if (string.IsNullOrWhiteSpace(imageNameWithExtension))
                return ErrorMessages.ImageUploadFailedMsg;

            command.ImageAddress = imageNameWithExtension;
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return ValidationHelper.GetErrorMessages(validationResult);

            // Map DTO to entity and save
            var book = _mapper.Map<Book>(command);
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
    public async Task<string?> UpdateBookAsync(BookDto command, IFormFile? imageFile, string? folderName)
    {
        if (!(await CheckPublisherExistence(command)))
            return string.Format(ErrorMessages.NotValid, nameof(BookDto.Name));

        if (!(await CheckBookCategoryExistence(command)))
            return string.Format(ErrorMessages.NotValid, nameof(BookDto.CategoryId));

        var imageNameWithExtension = "";
        try
        {
            var existingBook = await _bookBaseRepository.GetByIdAsync(command.Id);
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
            command.ImageAddress = imageNameWithExtension;
            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                return ValidationHelper.GetErrorMessages(validationResult);

            // Map DTO to entity and save
            var book = _mapper.Map(command, existingBook);
            await _bookBaseRepository.UpdateAsync(book);
            await _bookBaseRepository.SaveChangesAsync();

            return null;
        }
        catch (Exception e)
        {
            await _imageServices.DeleteImageAsync(imageNameWithExtension, folderName);
            throw;
        }
    }

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

    /// <inheritdoc/>
    public async Task<BookDto?> GetBookByIdAsync(int bookId)
    {
        try
        {
            var book = await _bookBaseRepository.GetByIdAsync(bookId);

            return book == null ? null : _mapper.Map<BookDto>(book);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<BookDto>> GetBooksAsync()
    {
        try
        {
            var books =
                await _bookBaseRepository.GetAllAsync(null, new Expression<Func<Book, object>>[] { b => b.Category });

            return books.Select(bookCategory => _mapper.Map<BookDto>(bookCategory)).ToList();
        }
        catch (Exception e)
        {
            throw;
        }
    }


    //Helper methods
    private async Task<bool> CheckBookCategoryExistence(BookDto command)
        => await _bookCategoryBaseRepository.AnyAsync(category => category.CategoryId.Equals(command.CategoryId));


    private async Task<bool> CheckPublisherExistence(BookDto command)
        => await _publisherBaseRepository.AnyAsync(publisher => publisher.PublisherId.Equals(command.PublisherId));
}