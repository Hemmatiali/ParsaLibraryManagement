using Microsoft.AspNetCore.Mvc;
using ParsaLibraryManagement.Web.Constants;
using ParsaLibraryManagement.Web.Generators;

namespace ParsaLibraryManagement.Web.Controllers
{
    //todo xml
    [AutoValidateAntiforgeryToken]
    public abstract class BaseController : Controller
    {
        #region Fields
        #endregion
        #region Ctor
        #endregion
        #region Methods

        #region Generators

//todo xml
        protected IActionResult? GenerateCatchMessage(ErrorsMessagesConstants message)
        {
            ErrorMessageGenerator.ErrorMessageGeneratorMethod(message, out var errMessage);
            return HandledErrorViewNew(errMessage);
        }


        #endregion

        #region Return views

//todo xml
        protected IActionResult HandledErrorViewNew(string errorMessage)
        {
            return RedirectToAction("HandledError", "Error", new { errorMessage });
        }

        #endregion

        #endregion
    }
}
