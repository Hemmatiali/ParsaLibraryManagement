using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Utilities;
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
    ///     Prepares and returns a view model for the Book Category index page.
    /// </summary>
    /// <param name="selectedFilter">The selected filter for categorization, if any.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
    ///     The task result is a <see cref="BookCategoryIndexViewModel"/> with populated categories, alphabet letters, and selected filter.
    /// </returns>
    /// <remarks>
    ///     This method retrieves all categories, generates alphabet letters, and constructs a view model for the Book Category index page.
    /// </remarks>
    private async Task<BookCategoryIndexViewModel> PrepareBookCategoryIndexViewModel(string selectedFilter = "")
    {
        try
        {
            // Retrieve categories based on the selected filter
            var categoriesDto = string.IsNullOrEmpty(selectedFilter)
                ? await _bookCategoryServices.GetAllCategoriesAsync()
                : await _bookCategoryServices.GetCategoriesAsync(selectedFilter);

            // Generate alphabet letters
            var letters = DataHelper.GenerateAlphabetLetters();

            // Return the view model with populated data
            return new BookCategoryIndexViewModel
            {
                Categories = categoriesDto,
                AlphabetLetters = letters,
                SelectedFilter = selectedFilter
            };
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <summary>
    ///     Repopulates the specified view model and returns the corresponding view.
    /// </summary>
    /// <param name="model">The <see cref="BookCategoryCreateEditViewModel"/> containing user inputs.</param>
    /// <param name="currentCategoryId">The ID of the current book category, if applicable.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
    ///     The task result is an <see cref="IActionResult"/> representing the view with the repopulated model.
    /// </returns>
    /// <remarks>
    ///     This method retrieves view model data using <see cref="PrepareBookCategoryViewModel"/>, preserves user inputs,
    ///     and returns the view with the repopulated model.
    /// </remarks>
    private async Task<IActionResult> RePopulateViewModelAndReturnView(BookCategoryCreateEditViewModel model, short? currentCategoryId = null)
    {
        try
        {
            // Get view model data
            var viewModel = await PrepareBookCategoryViewModel(currentCategoryId);

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
    ///     Prepares and returns a <see cref="BookCategoryCreateEditViewModel"/> based on specified conditions.
    /// </summary>
    /// <param name="currentCategoryId">The ID of the current book category, if applicable.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
    ///     The task result is a <see cref="BookCategoryCreateEditViewModel"/> with assigned categories.
    /// </returns>
    /// <remarks>
    ///     This method retrieves all categories based on conditions for creation or edition and assigns them to the view model.
    /// </remarks>
    private async Task<BookCategoryCreateEditViewModel> PrepareBookCategoryViewModel(short? currentCategoryId = null)
    {
        try
        {
            // Get all categories based on conditions for creation or edition
            var categories = currentCategoryId.HasValue
                ? await _bookCategoryServices.GetCategoriesForEditAsync(currentCategoryId.Value)
                : await _bookCategoryServices.GetAllCategoriesAsync();

            // Assign categories to view model
            return new BookCategoryCreateEditViewModel
            { RefGroups = categories.Select(bookCategoryDto => new SelectListItem(bookCategoryDto.Title, bookCategoryDto.CategoryId.ToString())).ToList() };
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the index view of Book Categories.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
    ///     The task result is an <see cref="IActionResult"/> representing the view with the populated Book Category index view model.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            // Prepare the Book Category index view model
            var viewModel = await PrepareBookCategoryIndexViewModel();

            // Return the view with the populated model
            return View(viewModel);
        }
        catch (Exception e)
        {
            //TODO: Add appropriate error handling logic
            _logger.LogError(e, "Error reading book categories.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemsErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for filtering Book Categories based on a prefix.
    /// </summary>
    /// <param name="prefix">The prefix used for filtering categories.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
    ///     The task result is an <see cref="IActionResult"/> representing the view with the filtered Book Category index view model.
    /// </returns>
    [HttpGet("[controller]/[action]/{prefix}")]
    public async Task<IActionResult> Filter(string prefix)
    {
        try
        {
            // Prepare the Book Category index view model with filtering
            var viewModel = await PrepareBookCategoryIndexViewModel(prefix);

            // Return the view with the filtered model
            return View("Index", viewModel);
        }
        catch (Exception e)
        {
            //TODO: Add appropriate error handling logic
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
            return await RePopulateViewModelAndReturnView(new BookCategoryCreateEditViewModel());
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
    public async Task<IActionResult> Create(BookCategoryCreateEditViewModel model)
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
            var viewModel = new BookCategoryCreateEditViewModel { Category = categoryDto };
            return await RePopulateViewModelAndReturnView(viewModel, bookCategoryId);
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
    public async Task<IActionResult> Edit(BookCategoryCreateEditViewModel model)
    {
        try
        {
            // Validate model
            ModelState.Remove(nameof(model.ImageFile));
            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            // Edit the book category
            var result = await _bookCategoryServices.UpdateCategoryAsync(model.Category, model.ImageFile, PathConstants.BookCategoriesFolderName);
            if (result.WasSuccess && string.IsNullOrWhiteSpace(result.Message))
                return RedirectToAction("Index"); // Done

            // Error
            ViewBag.errorMessage = result.Message;
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