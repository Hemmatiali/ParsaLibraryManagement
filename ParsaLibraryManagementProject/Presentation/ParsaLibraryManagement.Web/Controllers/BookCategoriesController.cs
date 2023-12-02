using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Domain.Interfaces.ImageServices;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.ValidationServices;
using ParsaLibraryManagement.Web.ViewModels.BookCategories;

namespace ParsaLibraryManagement.Web.Controllers
{
    //todo xml
    public class BookCategoriesController : BaseController
    {
        #region Fields

        private readonly IBookCategoryServices _bookCategoryServices;
        private readonly ImageFileValidationService _imageFileValidationServices;
        private readonly IImageServices _imageService;
        private readonly ILogger<BookCategoriesController> _logger;

        #endregion

        #region Ctor

        public BookCategoriesController(IBookCategoryServices bookCategoryServices, ImageFileValidationService imageFileValidationServices, IImageServices imageService, ILogger<BookCategoriesController> logger)
        {
            _bookCategoryServices = bookCategoryServices;
            _imageFileValidationServices = imageFileValidationServices;
            _imageService = imageService;
            _logger = logger;
        }

        #endregion

        #region Methods

        //todo xml
        private async Task<BookCategoryViewModel> PrepareBookCategoryViewModel()
        {//todo comments & try catch
            var categories = await _bookCategoryServices.GetAllCategoriesAsync();
            return new BookCategoryViewModel
            {
                RefGroups = categories.Select(c => new SelectListItem(c.Title, c.CategoryId.ToString())).ToList()
            };
        }

        //todo xml
        private async Task<IActionResult> RePopulateViewModelAndReturnView(BookCategoryViewModel model)
        {//todo comments & try catch
            var viewModel = await PrepareBookCategoryViewModel();
            viewModel.Category = model.Category; // Preserve user inputs
            return View(viewModel);
        }

        //todo xml
        private async Task<bool> ValidateAndUploadImage(BookCategoryViewModel model, bool isEditMode = false)
        {
            try
            {
                // Check image file
                if (model.ImageFile != null)
                {
                    // Validate image file
                    if (!_imageFileValidationServices.ValidateFile(model.ImageFile, out var errorMessage))
                    {
                        ModelState.AddModelError("ImageFile", errorMessage);
                        return false;
                    }

                    // In edit mode, delete the existing image and thumbnail image if it exists
                    if (isEditMode && !string.IsNullOrWhiteSpace(model.Category.ImageAddress))
                    {
                        await _imageService.DeleteImageAsync(model.Category.ImageAddress, "BookCategories");
                    }

                    // Save the new image
                    var imagePath = await _imageService.SaveImageAsync(model.ImageFile, "BookCategories");
                    if (string.IsNullOrWhiteSpace(imagePath))
                    {
                        ModelState.AddModelError("ImageFile", "Failed to upload image.");
                        return false;
                    }

                    // Update the image path in the model
                    model.Category.ImageAddress = imagePath;
                }
                else if (isEditMode)
                {
                    // Image is expected in edit mode but not provided
                    ModelState.AddModelError("ImageFile", "Image upload is required in edit mode.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // todo:Log and handle the exception
                // Consider using a logging framework or service
                ModelState.AddModelError("ImageFile", "An error occurred while processing the image.");
                _logger.LogError(ex, "Error processing the image.");
                return false;
            }
        }

        //todo xml
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

        //todo xml
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                // Prepare view model
                var viewModel = await PrepareBookCategoryViewModel();
                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error reading items for book category create.");
                return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
            }
        }

        //todo xml
        [HttpPost]
        public async Task<IActionResult> Create(BookCategoryViewModel model)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    return await RePopulateViewModelAndReturnView(model);
                }

                // Validate image
                if (!await ValidateAndUploadImage(model))
                {
                    return await RePopulateViewModelAndReturnView(model);
                }

                // Create the book category
                var result = await _bookCategoryServices.CreateCategoryAsync(model.Category);
                if (string.IsNullOrWhiteSpace(result))
                {
                    // Done
                    return RedirectToAction("Index");
                }

                // Error
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


        //todo xml
        [HttpGet]
        public async Task<IActionResult> Edit(short id)
        {
            try
            {
                // Get category
                var categoryDto = await _bookCategoryServices.GetCategoryByIdAsync(id);
                if (categoryDto == null)
                {
                    return NotFound();
                }

                // Prepare view model
                var viewModel = await PrepareBookCategoryViewModel();
                viewModel.Category = categoryDto;
                return View(viewModel);
            }
            catch (Exception e)
            {
                //TODO: Add appropriate error handling logic
                _logger.LogError(e, "Error reading items for book category edit.");
                return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
            }
        }

        //todo xml
        [HttpPost]
        public async Task<IActionResult> Edit(short id, BookCategoryViewModel model)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    return await RePopulateViewModelAndReturnView(model);
                }

                // Validate image
                if (!await ValidateAndUploadImage(model, true))
                {
                    return await RePopulateViewModelAndReturnView(model);
                }

                // Edit the book category
                var result = await _bookCategoryServices.UpdateCategoryAsync(id, model.Category);
                if (string.IsNullOrWhiteSpace(result))
                    // Done
                    return RedirectToAction("Index");

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

        #endregion
    }
}
