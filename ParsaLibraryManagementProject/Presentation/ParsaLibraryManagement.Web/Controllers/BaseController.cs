using Microsoft.AspNetCore.Mvc;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.Generators;

namespace ParsaLibraryManagement.Web.Controllers
{
    /// <summary>
    ///     Base controller for handling common functionalities and behaviors across controllers.
    /// </summary>
    /// <remarks>
    ///     This abstract class provides a foundation for other controllers in the application.
    /// </remarks>
    [AutoValidateAntiforgeryToken]
    public abstract class BaseController : Controller
    {
        #region Fields
        #endregion
        #region Ctor
        #endregion
        #region Methods

        #region Generators

        /// <summary>
        ///     Generates an action result for handling catch messages based on the specified error message constant.
        /// </summary>
        /// <param name="message">The error message constant.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the action.</returns>
        protected IActionResult? GenerateCatchMessage(ErrorsMessagesConstants message)
        {
            ErrorMessageGenerator.ErrorMessageGeneratorMethod(message, out var errMessage);
            return HandledErrorViewNew(errMessage);
        }

        #endregion

        #region Return views

        /// <summary>
        ///     Redirects to the "HandledError" action of the "Error" controller with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message to be displayed.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the action.</returns>
        protected IActionResult HandledErrorViewNew(string errorMessage)
        {
            return RedirectToAction("HandledError", "Error", new { errorMessage });
        }

        #endregion

        #endregion
    }
}
