using Microsoft.AspNetCore.Mvc;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.ViewModels.Gender;

namespace ParsaLibraryManagement.Web.Controllers;

public class GenderController:BaseController
{
    private readonly IGenderService _genderService;
    private readonly ILogger<GenderController> _logger;

    public GenderController(IGenderService genderService, ILogger<GenderController> logger)
    {
        _genderService = genderService;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        try
        {
            var genders = await _genderService.GetGendersAsync();
            return View(genders);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading genders.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemsErrMsg)!;
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            // Prepare view model
            return await RePopulateViewModelAndReturnView(new GenderViewModel());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for gender create.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(GenderViewModel model)
    {
        try
        {
            // Check validation
            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            // Create publisher
            var result = await _genderService.CreateGenderAsync(model.GenderDto);

            // Check error
            if (string.IsNullOrWhiteSpace(result))
                return RedirectToAction("Index"); // Done

            // Handle errors
            ViewBag.errorMessage = result;
            return await RePopulateViewModelAndReturnView(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating gender.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulCreateItemErrMsg)!;
        }
    }
    
    [HttpGet("Gender/Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            // Get gender
            var gender = await _genderService.GetGenderByAsync(Convert.ToInt16(id));
            if (gender == null)
                return NotFound();

            // Prepare view model with the existing gender data
            var viewModel = new GenderViewModel { GenderDto = gender };
            return await RePopulateViewModelAndReturnView(viewModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for gender edit.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(GenderViewModel model)
    {
        try
        {
            // Validate model
            if (!ModelState.IsValid)
                return await RePopulateViewModelAndReturnView(model);

            // Edit the book category
            var result = await _genderService.UpdateGenderAsync(model.GenderDto);
            if (string.IsNullOrWhiteSpace(result))
                return RedirectToAction("Index"); // Done

            // Error
            ViewBag.errorMessage = result;
            return await RePopulateViewModelAndReturnView(model);
        }
        catch (Exception ex)
        {
            //TODO: Add appropriate error handling logic
            _logger.LogError(ex, "Error editing gender.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulEditItemErrMsg)!;
        }
    }
    
    //Helper Methods
    private async Task<IActionResult> RePopulateViewModelAndReturnView(GenderViewModel model)
    {
        try
        {
            // Get view model data
            var viewModel = await PrepareGenderViewModel();
            viewModel.GenderDto = model.GenderDto;
            // Return view with populated model
            return View(viewModel);
        }
        catch (Exception e)
        {
            throw;
        }
    }
    
    
    private static Task<GenderViewModel> PrepareGenderViewModel()
    {
        try
        {
            return Task.FromResult(new GenderViewModel());
        }
        catch (Exception e)
        {
            throw;
        }
    }
}