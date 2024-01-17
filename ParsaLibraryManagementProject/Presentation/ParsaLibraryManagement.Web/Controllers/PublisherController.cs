using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Domain.Common.Extensions;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.ViewModels.Publisher;

namespace ParsaLibraryManagement.Web.Controllers;

/// <summary>
///     Controller responsible for handling actions related to publishers.
/// </summary>
/// <remarks>
///     This controller inherits from the <see cref="BaseController"/> and provides actions for managing publishers.
/// </remarks>
public class PublisherController : BaseController
{
    #region Fields

    private readonly IPublisherServices _publisherServices;
    private readonly IGenderService _genderService;
    private readonly ILogger<PublisherController> _logger;

    #endregion

    #region Ctor

    public PublisherController(IPublisherServices publisherServices, IGenderService genderService, ILogger<PublisherController> logger)
    {
        _publisherServices = publisherServices;
        _genderService = genderService;
        _logger = logger;
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Prepares a view model for the Publisher view (Create and Edit methods).
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding a <see cref="PublisherViewModel"/>.</returns>
    private async Task<PublisherViewModel> PreparePublisherViewModel()
    {
        try
        {
            // Get all genders
            var genders = await _genderService.GetAllGendersAsync();

            // Assign genders to view model
            return new PublisherViewModel
            { Genders = genders.Select(dto => new SelectListItem(dto.Title, dto.GenderId.ToString())).ToList() };
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
    private async Task<IActionResult> RePopulateViewModelAndReturnView(PublisherViewModel model)
    {
        try
        {
            // Get view model data
            var viewModel = await PreparePublisherViewModel();

            // Preserve user inputs
            viewModel.Publisher = model.Publisher;

            // Return view with populated model
            return View(viewModel);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the index view of publishers.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            // Get all publishers
            var publishersDto = await _publisherServices.GetAllPublishersAsync();

            return View(publishersDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading publishers.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemsErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the create view of publishers.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            // Prepare view model
            return await RePopulateViewModelAndReturnView(new PublisherViewModel());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for publisher create.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP POST request for creating a publisher.
    /// </summary>
    /// <param name="model">The view model containing data for the new publisher.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(PublisherViewModel model)
    {
        try
        {
            // Check validation
            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            // Create publisher
            var result = await _publisherServices.CreatePublisherAsync(model.Publisher);

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
            _logger.LogError(ex, "Error creating publisher.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulCreateItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the edit view of a specific publisher.
    /// </summary>
    /// <param name="publisherId">The ID of the publisher to be edited.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(string publisherId)
    {
        try
        {
            // Get publisher
            var publisherDto = await _publisherServices.GetPublisherByIdAsync(publisherId.ToGuid());
            if (publisherDto == null)
                return NotFound();

            // Prepare view model with the existing publisher data
            var viewModel = new PublisherViewModel() { Publisher = publisherDto };
            return await RePopulateViewModelAndReturnView(viewModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for publisher edit.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP POST request for editing a specific publisher.
    /// </summary>
    /// <param name="model">The view model containing updated data for the publisher.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpPost]
    public async Task<IActionResult> Edit(PublisherViewModel model)
    {
        try
        {
            // Validate model
            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            // Edit the publisher
            var result = await _publisherServices.UpdatePublisherAsync(model.Publisher);
            if (string.IsNullOrWhiteSpace(result))
                return RedirectToAction("Index"); // Done

            // Error
            ViewBag.errorMessage = result;
            return await RePopulateViewModelAndReturnView(model);
        }
        catch (Exception ex)
        {
            //TODO: Add appropriate error handling logic
            _logger.LogError(ex, "Error editing publisher.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulEditItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the delete confirmation view of a specific publisher.
    /// </summary>
    /// <param name="publisherId">The ID of the publisher to be deleted.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(string publisherId)
    {
        try
        {
            // Get publisher
            var publisherDto = await _publisherServices.GetPublisherByIdAsync(publisherId.ToGuid());
            if (publisherDto == null)
                return NotFound();

            // Prepare view model
            return View(new PublisherDeleteViewModel() { Publisher = publisherDto });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for publisher delete.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP POST request for deleting a specific publisher.
    /// </summary>
    /// <param name="model">The delete view model containing confirmation data.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpPost]
    public async Task<IActionResult> Delete(PublisherDeleteViewModel model)
    {
        try
        {
            // Validate model
            if (!ModelState.IsValid)
                return View(model);

            // Delete the publisher
            var result = await _publisherServices.DeletePublisherAsync(model.Publisher.PublisherId);
            if (string.IsNullOrWhiteSpace(result))
                return RedirectToAction("Index"); // Done

            // Error
            ViewBag.errorMessage = result;
            return View(model);
        }
        catch (Exception ex)
        {
            //TODO: Add appropriate error handling logic
            _logger.LogError(ex, "Error deleting publisher.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulDeleteItemErrMsg)!;
        }
    }

    #endregion
}