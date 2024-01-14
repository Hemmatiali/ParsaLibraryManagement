using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.ViewModels.Publisher;

namespace ParsaLibraryManagement.Web.Controllers;

public class PublisherController : BaseController
{
    private readonly IPublisherServices _publisherServices;
    private readonly ILogger<PublisherController> _logger;
    private readonly IGenderService _genderService;

    public PublisherController(IPublisherServices publisherServices, ILogger<PublisherController> logger,
        IGenderService genderService)
    {
        _publisherServices = publisherServices;
        _logger = logger;
        _genderService = genderService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var publishers = await _publisherServices.GetPublishersAsync();
            return View(publishers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading publishers.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemsErrMsg)!;
        }
    }

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
            _logger.LogError(ex, "Error creating publisher.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulCreateItemErrMsg)!;
        }
    }

    [HttpGet("Publisher/Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            // Get publisher
            var publisher = await _publisherServices.GetPublisherByAsync(Convert.ToInt16(id));
            if (publisher == null)
                return NotFound();

            // Prepare view model with the existing publisher data
            var viewModel = new PublisherViewModel() { Publisher = publisher };
            return await RePopulateViewModelAndReturnView(viewModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for publisher edit.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(PublisherViewModel model)
    {
        try
        {
            // Validate model
            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            // Edit the book category
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

    // Helper Method

    private async Task<PublisherViewModel> PreparePublisherViewModel()
    {
        try
        {
            var genders = await _genderService.GetGendersAsync();


            return new PublisherViewModel
            {
                Genders = genders.Select(dto => new SelectListItem(dto.Title, dto.GenderId.ToString())).ToList()
            };
        }
        catch (Exception e)
        {
            throw;
        }
    }

    private async Task<IActionResult> RePopulateViewModelAndReturnView(PublisherViewModel model)
    {
        try
        {
            // Get view model data
            var viewModel = await PreparePublisherViewModel();

            viewModel.Publisher = model.Publisher;

            // Return view with populated model
            return View(viewModel);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}