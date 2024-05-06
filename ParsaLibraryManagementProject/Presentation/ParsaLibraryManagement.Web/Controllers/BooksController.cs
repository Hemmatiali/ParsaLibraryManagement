using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Infrastructure.Common.Constants;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.ViewModels.Book;

namespace ParsaLibraryManagement.Web.Controllers;

/// <summary>
///     Controller responsible for handling actions related to books.
/// </summary>
/// <remarks>
///     This controller inherits from the <see cref="BaseController"/> and provides actions for managing books.
/// </remarks>
public class BooksController : BaseController
{
    #region Fields

    private readonly IBooksServices _booksServices;
    private readonly ILogger<BooksController> _logger;
    private readonly IPublisherServices _publisherServices;
    private readonly IBookCategoryServices _bookCategoryServices;

    #endregion

    #region Ctor

    public BooksController(IBooksServices booksServices, ILogger<BooksController> logger, IPublisherServices publisherServices, IBookCategoryServices bookCategoryServices)
    {
        _booksServices = booksServices;
        _logger = logger;
        _publisherServices = publisherServices;
        _bookCategoryServices = bookCategoryServices;
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Prepares and returns a view model for the Books index page.
    /// </summary>
    private async Task<BookViewModel> PrepareBookIndexViewModel()
    {
        try
        {
            // Retrieve Books 
            var bookDtos = await _booksServices.GetAllBooksAsync();

            // Return the view model with populated data
            return new BookViewModel
            { Books = bookDtos };
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <summary>
    ///     Repopulates the specified view model and returns the corresponding view.
    /// </summary>
    /// <param name="model">The <see cref="BookCreateEditViewModel"/> containing user inputs.</param>
    private async Task<IActionResult> RePopulateViewModelAndReturnView(BookCreateEditViewModel model)
    {
        try
        {
            // Get view model data
            var viewModel = await PrepareBookViewModel();

            // Preserve user inputs
            viewModel.Book = model.Book;

            // Return view with populated model
            return View(viewModel);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <summary>
    ///     Prepares and returns a <see cref="BookCreateEditViewModel"/> based on specified conditions.
    /// </summary>
    private async Task<BookCreateEditViewModel> PrepareBookViewModel()
    {
        try
        {
            var bookCreateEditViewModel = new BookCreateEditViewModel();

            // Retrieve publishers information asynchronously
            var publisherItems = await _publisherServices.GetAllPublisherIdsAndNamesAsync();

            // Convert publishers data to SelectListItem
            bookCreateEditViewModel.PublisherIds = publisherItems
                .Select(publisher => new SelectListItem(publisher.fullName, publisher.id.ToString())).ToList();

            // Retrieve categories information asynchronously
            var categoryItems = await _bookCategoryServices.GetAllCategoriesAsync();

            // Convert categories data to SelectListItem
            bookCreateEditViewModel.CategoryIds = categoryItems
                .Select(category => new SelectListItem(category.Title, category.CategoryId.ToString())).ToList();

            return bookCreateEditViewModel;
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the index view of Books.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            // Prepare the Book index view model
            var viewModel = await PrepareBookIndexViewModel();

            // Return the view with the populated model
            return View(viewModel);
        }
        catch (Exception e)
        {
            //TODO: Add appropriate error handling logic
            _logger.LogError(e, "Error reading books.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemsErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the create view of book.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            // Prepare view model
            return await RePopulateViewModelAndReturnView(new BookCreateEditViewModel());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for book create.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP POST request for creating a book.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(BookCreateEditViewModel model)
    {
        try
        {
            // Check validation
            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            // Create book
            var result = await _booksServices.CreateBookAsync(model.Book, model.ImageFile, PathConstants.BooksFolderName);

            // Check error
            if (string.IsNullOrWhiteSpace(result))
                return RedirectToAction("Index"); // Done

            // Handle errors
            ViewBag.errorMessage = result;
            return await RePopulateViewModelAndReturnView(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Book.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulCreateItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the edit view of a specific book.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(int bookId)
    {
        try
        {
            // Get book
            var bookDto = await _booksServices.GetBookByIdAsync(bookId);
            if (bookDto == null)
                return NotFound();

            // Prepare view model with the existing book data
            var viewModel = new BookCreateEditViewModel() { Book = bookDto };
            return await RePopulateViewModelAndReturnView(viewModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for book edit.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP POST request for editing a specific book.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Edit(BookCreateEditViewModel model)
    {
        try
        {
            // Validate model
            ModelState.Remove(nameof(BookCreateEditViewModel.ImageFile));
            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            // Edit the book
            var result = await _booksServices.UpdateBookAsync(model.Book, model.ImageFile, PathConstants.BooksFolderName);

            if (string.IsNullOrWhiteSpace(result))
                return RedirectToAction("Index"); // Done

            // Error
            ViewBag.errorMessage = result;
            return await RePopulateViewModelAndReturnView(model);
        }
        catch (Exception ex)
        {
            //TODO: Add appropriate error handling logic
            _logger.LogError(ex, "Error editing book.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulEditItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the delete confirmation view of a specific book.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Delete(int bookId)
    {
        try
        {
            // Get book
            var bookDto = await _booksServices.GetBookByIdAsync(bookId);
            if (bookDto == null)
                return NotFound();

            // Prepare view model
            return View(new BookDeleteViewModel() { Book = bookDto });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for book delete.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP POST request for deleting a specific book.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Delete(BookDeleteViewModel model)
    {
        try
        {
            // Validate model
            if (!ModelState.IsValid)
                return View(model);

            // Delete the book
            var result = await _booksServices.DeleteBookAsync(model.Book.Id, PathConstants.BooksFolderName);
            if (string.IsNullOrWhiteSpace(result))
                return RedirectToAction("Index"); // Done

            // Error
            ViewBag.errorMessage = result;
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting book.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulDeleteItemErrMsg)!;
        }
    }



    #endregion
}