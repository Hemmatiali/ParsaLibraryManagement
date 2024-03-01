using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Infrastructure.Common.Constants;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.ViewModels.Book;

namespace ParsaLibraryManagement.Web.Controllers;

/// <summary>
/// Controller class for managing operations related to books.
/// </summary>
public class BookController : BaseController
{
    private readonly IBooksServices _booksServices;
    private readonly ILogger<BookController> _logger;
    private readonly IPublisherServices _publisherServices;
    private readonly IBookCategoryServices _bookCategoryServices;

    public BookController(IBooksServices booksServices,
        ILogger<BookController> logger,
        IPublisherServices publisherServices,
        IBookCategoryServices bookCategoryServices)
    {
        _booksServices = booksServices;
        _logger = logger;
        _publisherServices = publisherServices;
        _bookCategoryServices = bookCategoryServices;
    }

    /// <summary>
    /// Handles the HTTP GET request for the index view of Book.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result is an <see cref="IActionResult"/> representing the view with the populated Book index view model.</returns>
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
    /// Handles the HTTP GET request for the create view of book.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
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
            _logger.LogError(e, "Error reading items for book category create.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    /// <summary>
    /// Handles the HTTP POST request for creating a book.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(BookCreateEditViewModel model)
    {
        try
        {
            ModelState.Remove(nameof(BookCreateEditViewModel.CategoryIds));
            ModelState.Remove(nameof(BookCreateEditViewModel.PublisherIds));

            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            var result = await _booksServices.AddBookAsync(model.Book, model.ImageFile, PathConstants.BookFolderName);

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
            _logger.LogError(e, "Error reading items for publisher edit.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BookCreateEditViewModel model)
    {
        try
        {
            ModelState.Remove(nameof(BookCreateEditViewModel.CategoryIds));
            ModelState.Remove(nameof(BookCreateEditViewModel.PublisherIds));
            ModelState.Remove(nameof(BookCreateEditViewModel.ImageFile));

            // Validate model
            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            // Edit the book
            var result =
                await _booksServices.UpdateBookAsync(model.Book, model.ImageFile, PathConstants.BookFolderName);

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

    [HttpGet]
    public async Task<IActionResult> Delete(short bookId)
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
            _logger.LogError(e, "Error reading items for book category delete.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(BookDeleteViewModel model)
    {
        try
        {
            // Delete the book
            var result = await _booksServices.DeleteBookAsync(model.Book.Id, PathConstants.BookFolderName);
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

    private async Task<IActionResult> RePopulateViewModelAndReturnView(BookCreateEditViewModel model)
    {
        try
        {
            // Get view model data
            var viewModel = await PrepareBookCategoryViewModel();

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

    private async Task<BookCreateEditViewModel> PrepareBookCategoryViewModel()
    {
        try
        {
            var bookCreateEditViewModel = new BookCreateEditViewModel();

            // Retrieve publisher information asynchronously
            var publisherItems = await _publisherServices.GetAllPublisherIdsAndNamesAsync();

            // Convert publisher data to SelectListItem
            bookCreateEditViewModel.PublisherIds = publisherItems
                .Select(publisher => new SelectListItem(publisher.fullName, publisher.id.ToString())).ToList();

            // Retrieve category information asynchronously
            var categoryItems = await _bookCategoryServices.GetAllCategoriesAsync();

            // Convert category data to SelectListItem
            bookCreateEditViewModel.CategoryIds = categoryItems
                .Select(category => new SelectListItem(category.Title, category.CategoryId.ToString())).ToList();

            return bookCreateEditViewModel;
        }
        catch
        {
            throw; // Re-throwing caught exception as-is
        }
    }

    private async Task<BookViewModel> PrepareBookIndexViewModel()
    {
        try
        {
            // Retrieve Books 
            var bookDtos = await _booksServices.GetBooksAsync();


            // Return the view model with populated data
            return new BookViewModel
            {
                Books = bookDtos
            };
        }
        catch (Exception e)
        {
            throw;
        }
    }
}