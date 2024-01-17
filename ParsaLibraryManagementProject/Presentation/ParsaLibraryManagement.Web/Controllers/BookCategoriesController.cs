using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Infrastructure.Common.Constants;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.ViewModels.BookCategories;

namespace ParsaLibraryManagement.Web.Controllers;

/// <summary>
///     Controller responsible for handling actions related to book categories.
/// </summary>
/// <remarks>
///     This controller inherits from the <see cref="BaseController"/> and provides actions for managing book categories.
/// </remarks>
public class BookCategoriesController : BaseController
{
    #region Fields

    private readonly IBookCategoryServices _bookCategoryServices;
    private readonly ILogger<BookCategoriesController> _logger;

    #endregion

    #region Ctor

    public BookCategoriesController(IBookCategoryServices bookCategoryServices, ILogger<BookCategoriesController> logger)
    {
        _bookCategoryServices = bookCategoryServices;
        _logger = logger;
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Prepares a view model for the BookCategory view (Create and Edit methods).
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding a <see cref="BookCategoryViewModel"/>.</returns>
    private async Task<BookCategoryViewModel> PrepareBookCategoryViewModel()
    {
        try
        {
            // Get all categories
            var categories = await _bookCategoryServices.GetAllCategoriesAsync();

            // Assign categories to view model
            return new BookCategoryViewModel
                { RefGroups = categories.Select(bookCategoryDto => new SelectListItem(bookCategoryDto.Title, bookCategoryDto.CategoryId.ToString())).ToList() };
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <summary>
    ///     Re-populates the view model and returns the associated view.
    /// </summary>
    /// <param name="model">The view model to be re-populated.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    private async Task<IActionResult> RePopulateViewModelAndReturnView(BookCategoryViewModel model)
    {
        try
        {
            // Get view model data
            var viewModel = await PrepareBookCategoryViewModel();

            // Preserve user inputs
            viewModel.Category = model.Category;

            // Return view with populated model
            return View(viewModel);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the index view of book categories.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            // Get all categories
            var categoriesDto = await _bookCategoryServices.GetAllCategoriesAsync();

            return View(categoriesDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading book categories.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemsErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the view of filtered book categories.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet("[controller]/[action]/{prefix}")]
    public async Task<IActionResult> Filter(string prefix)
    {
        try
        {
            // Get all categories
            var categoriesDto = await _bookCategoryServices.GetCategoriesAsync(prefix);

            return View("Index", categoriesDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading book categories.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemsErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the create view of book categories.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            // Prepare view model
            return await RePopulateViewModelAndReturnView(new BookCategoryViewModel());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for book category create.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP POST request for creating a book category.
    /// </summary>
    /// <param name="model">The view model containing data for the new book category.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(BookCategoryViewModel model)
    {
        try
        {
            // Check validation
            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            // Create book category
            var result = await _bookCategoryServices.CreateCategoryAsync(model.Category, model.ImageFile, PathConstants.BookCategoriesFolderName);

            // Check error
            if (string.IsNullOrWhiteSpace(result))
                return RedirectToAction("Index"); // Done

            // Handle errors
            ViewBag.errorMessage = result;
            return await RePopulateViewModelAndReturnView(model);
        }
        catch (Exception ex)
        {
            //TODO: Add appropriate error handling logic
            _logger.LogError(ex, "Error creating book category.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulCreateItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the edit view of a specific book category.
    /// </summary>
    /// <param name="bookCategoryId">The ID of the book category to be edited.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(short bookCategoryId)
    {
        try
        {
            // Get category
            var categoryDto = await _bookCategoryServices.GetCategoryByIdAsync(bookCategoryId);
            if (categoryDto == null)
                return NotFound();

            // Prepare view model with the existing category data
            var viewModel = new BookCategoryViewModel { Category = categoryDto };
            return await RePopulateViewModelAndReturnView(viewModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for book category edit.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP POST request for editing a specific book category.
    /// </summary>
    /// <param name="model">The view model containing updated data for the book category.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpPost]
    public async Task<IActionResult> Edit(BookCategoryViewModel model)
    {
        try
        {
            // Validate model
            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            // Edit the book category
            var result = await _bookCategoryServices.UpdateCategoryAsync(model.Category, model.ImageFile, PathConstants.BookCategoriesFolderName);
            if (string.IsNullOrWhiteSpace(result))
                return RedirectToAction("Index"); // Done

            // Error
            ViewBag.errorMessage = result;
            return await RePopulateViewModelAndReturnView(model);
        }
        catch (Exception ex)
        {
            //TODO: Add appropriate error handling logic
            _logger.LogError(ex, "Error editing book category.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulEditItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the delete confirmation view of a specific book category.
    /// </summary>
    /// <param name="bookCategoryId">The ID of the book category to be deleted.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(short bookCategoryId)
    {
        try
        {
            // Get category
            var categoryDto = await _bookCategoryServices.GetCategoryByIdAsync(bookCategoryId);
            if (categoryDto == null)
                return NotFound();

            // Prepare view model
            return View(new BookCategoryDeleteViewModel { Category = categoryDto });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for book category delete.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP POST request for deleting a specific book category.
    /// </summary>
    /// <param name="model">The delete view model containing confirmation data.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpPost]
    public async Task<IActionResult> Delete(BookCategoryDeleteViewModel model)
    {
        try
        {
            // Validate model
            if (!ModelState.IsValid)
                return View(model);

            // Delete the book category
            var result = await _bookCategoryServices.DeleteCategoryAsync(model.Category.CategoryId, PathConstants.BookCategoriesFolderName);
            if (string.IsNullOrWhiteSpace(result))
                return RedirectToAction("Index"); // Done

            // Error
            ViewBag.errorMessage = result;
            return View(model);
        }
        catch (Exception ex)
        {
            //TODO: Add appropriate error handling logic
            _logger.LogError(ex, "Error deleting book category.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulDeleteItemErrMsg)!;
        }
    }

    #endregion
}