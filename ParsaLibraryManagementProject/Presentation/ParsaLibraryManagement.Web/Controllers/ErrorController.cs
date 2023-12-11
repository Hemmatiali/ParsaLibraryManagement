using Microsoft.AspNetCore.Mvc;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.Generators;

namespace ParsaLibraryManagement.Web.Controllers
{
    /// <summary>
    ///     Controller class for handling and displaying error information to the user.
    /// </summary>
    public class ErrorController : BaseController
    {
        #region Fields

        #endregion

        #region Ctor

        #endregion

        #region Methods

        /// <summary>
        ///     Action method to handle and display error information to the user.
        /// </summary>
        /// <param name="errorMessage">The error message object containing the error information to be displayed.</param>
        /// <returns>Returns a view that displays the error information to the user.</returns>
        [HttpGet("Error/{errorMessage?}")]
        public IActionResult HandledError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                ErrorMessageGenerator.ErrorMessageGeneratorMethod(ErrorsMessagesConstants.UnHandledErrMsg, out var errMessage);
            }

            ViewBag.errMessage = errorMessage;//todo check
            return View();
        }

        #endregion
    }
}
