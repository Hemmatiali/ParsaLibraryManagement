using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Domain.Common.Extensions;
using ParsaLibraryManagement.Domain.Models;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.ViewModels.BorrowedBooks;

namespace ParsaLibraryManagement.Web.Controllers;

/// <summary>
///     Controller responsible for handling actions related to BorrowedBooks.
/// </summary>
/// <remarks>
///     This controller inherits from the <see cref="BaseController"/> and provides actions for managing BorrowedBooks.
/// </remarks>
public class BorrowedBookController : BaseController
{
    #region Fields

    private readonly IBorrowedBookServices _BorrowedBookServices;
   
    private readonly ILogger<BorrowedBookController> _logger;

    #endregion

    #region Ctor
  
        public BorrowedBookController(IBorrowedBookServices BorrowedBookServices,  ILogger<BorrowedBookController> logger)
    {
        _BorrowedBookServices = BorrowedBookServices;
      
        _logger = logger;
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Handles the HTTP GET request for the index view of BorrowedBooks.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Index(string UserId, string BookId )
    {
        try
        {

           int.TryParse(BookId, out int _BookId);
            int.TryParse(UserId, out int _UserId);


            // Get all BorrowedBooks
            var BorrowedBooksDto = await _BorrowedBookServices.GetAllBorrowedByBookandUserAsync(_BookId, _UserId);
            BorrowedBookIndexViewModel borrowedBookIndexViewModel = new BorrowedBookIndexViewModel();
            borrowedBookIndexViewModel.BorrowedBooks = BorrowedBooksDto;
            borrowedBookIndexViewModel.UserId = UserId;
            borrowedBookIndexViewModel.BookId = BookId;

            return View(borrowedBookIndexViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading BorrowedBooks.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemsErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the create view of BorrowedBooks.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    /// <summary>
    ///     Handles the HTTP POST request for creating a BorrowedBook.
    /// </summary>
    /// <param name="model">The view model containing data for the new BorrowedBook.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(BorrowedBookCreateEditViewModel model)
    {
        try
        {
            // Check validation
            if (!ModelState.IsValid)
            {
                // Handle errors
             
                return View();
            }
            // Create BorrowedBook
            var result = await _BorrowedBookServices.CreateBorrowedAsync(new BorrowedBookDto() { BookId= model.BookId,UserId=model.UserId });

            // Check error
            if (string.IsNullOrWhiteSpace(result))
                return RedirectToAction("Index"); // Done

            // Handle errors
            ViewBag.errorMessage = result;
            return View();

        }
        catch (Exception ex)
        {
            //TODO: Add appropriate error handling logic
            _logger.LogError(ex, "Error creating BorrowedBook.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulCreateItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP GET request for the edit view of a specific BorrowedBook.
    /// </summary>
    /// <param name="BorrowedBookId">The ID of the BorrowedBook to be edited.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int Bid)
    {
        try
        {
            // Get BorrowedBook
            var BorrowedBookDto = await _BorrowedBookServices.GetBorrowedByIdAsync(Bid);
            if (BorrowedBookDto == null)
                return NotFound();
            return View(BorrowedBookDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reading items for BorrowedBook edit.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulReadItemErrMsg)!;
        }
    }

    /// <summary>
    ///     Handles the HTTP POST request for editing a specific BorrowedBook.
    /// </summary>
    /// <param name="model">The view model containing updated data for the BorrowedBook.</param>
    /// <returns>A task representing the asynchronous operation, yielding an <see cref="IActionResult"/>.</returns>
    [HttpPost]
    public async Task<IActionResult> Edit(BorrowedBookEditDto model)
    {
        try
        {
            // Validate model
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            // Edit the BorrowedBook
            var result = await _BorrowedBookServices.UpdateBorrowedAsync(model);
            if (result.WasSuccess)
                return RedirectToAction("Index"); // Done

            // Error
            ViewBag.errorMessage = result.Message;
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            //TODO: Add appropriate error handling logic
            _logger.LogError(ex, "Error editing BorrowedBook.");
            return GenerateCatchMessage(ErrorsMessagesConstants.UnSuccessfulEditItemErrMsg)!;
        }
    }

  

    #endregion
}