using Microsoft.AspNetCore.Mvc;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.Generators;
using Serilog;

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
        //[HttpGet("Error/{errorMessage?}")]
        //todo: remove if unNecessary
        public IActionResult HandledError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                ErrorMessageGenerator.ErrorMessageGeneratorMethod(ErrorsMessagesConstants.UnHandledErrMsg, out var errMessage);
            }

            ViewBag.errMessage = errorMessage;//todo check
            Log.Fatal(errorMessage);
            return View();
        }

        /// <summary>
        ///     Handles HTTP errors by displaying a custom error page with appropriate error messages.
        /// </summary>
        /// <param name="statusCode">The HTTP status code of the error.</param>
        /// <returns>A view representing the error page.</returns>
        /// <remarks>
        ///     This method uses a switch expression to select an error message based on the provided status code.
        ///     It covers common HTTP status codes like 400 (Bad Request), 401 (Unauthorized), 403 (Forbidden),
        ///     404 (Not Found), 500 (Internal Server Error), etc. For status codes not explicitly handled,
        ///     a default error message is used.
        /// </remarks>
        public IActionResult Index(int statusCode)
        {
            // Error messages based on status code
            var errorMessage = statusCode switch
            {
                400 => "Bad request.",
                401 => "Unauthorized access.",
                403 => "Forbidden access.",
                404 => "Page not found.",
                405 => "Method not allowed.",
                408 => "Request timeout.",
                500 => "Internal server error.",
                501 => "Not implemented.",
                502 => "Bad gateway.",
                503 => "Service unavailable.",
                504 => "Gateway timeout.",
                _ => "An unexpected error occurred."
            };

            // TODO: log
            // Assign status code and error message
            ViewBag.StatusCode = statusCode;
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }

        #endregion
    }
}
