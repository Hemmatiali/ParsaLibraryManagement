using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Services;
using ParsaLibraryManagement.Domain.Interfaces.General;
using ParsaLibraryManagement.Web.ValidationServices;
using ParsaLibraryManagement.Web.ViewModels.BookCategories;

namespace ParsaLibraryManagement.Web.Controllers
{
    //todo xml
    public class BookCategoriesController : Controller
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
        {
            var categories = await _bookCategoryServices.GetAllCategoriesAsync();
            return new BookCategoryViewModel
            {
                RefGroups = categories.Select(c => new SelectListItem(c.Title, c.CategoryId.ToString())).ToList()
            };
        }

        //todo xml
        private async Task<IActionResult> RePopulateViewModelAndReturnView(BookCategoryViewModel model)
        {
            var viewModel = await PrepareBookCategoryViewModel();
            viewModel.Category = model.Category; // Preserve user inputs
            return View(viewModel);
        }

        //todo xml
        private async Task<bool> ValidateAndUploadImage(BookCategoryViewModel model)
        {
            if (model.ImageFile != null && !_imageFileValidationServices.ValidateFile(model.ImageFile, out var errorMessage))
            {
                ModelState.AddModelError("ImageFile", errorMessage);
                return false;
            }
            if (!string.IsNullOrWhiteSpace(model.ImageFile?.FileName))
            {
                var imagePath = await _imageService.SaveImageAsync(model.ImageFile, "BookCategories");
                if (string.IsNullOrWhiteSpace(imagePath))
                {
                    ModelState.AddModelError("ImageFile", "Failed to upload image.");
                    return false;
                }
                model.Category.ImageAddress = imagePath;
            }
            return true;
        }

        //todo xml
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //todo xml
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = PrepareBookCategoryViewModel().Result;
            return View(viewModel);
        }


        //todo xml
        [HttpPost]
        public async Task<IActionResult> Create(BookCategoryViewModel model)
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

            try
            {
                // Create the book category
                var result = await _bookCategoryServices.CreateCategoryAsync(model.Category);
                if (string.IsNullOrWhiteSpace(result))
                {
                    // Done
                    return RedirectToAction("Index");
                }

                ViewBag.errorMessage = result;
                return await RePopulateViewModelAndReturnView(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book category");
                // Handle the exception
                //TODO: Add appropriate error handling logic
                return await RePopulateViewModelAndReturnView(model);
            }
        }

        #endregion



    }
}
