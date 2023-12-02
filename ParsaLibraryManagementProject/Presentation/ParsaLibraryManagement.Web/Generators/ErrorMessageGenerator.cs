using ParsaLibraryManagement.Web.Constants;

namespace ParsaLibraryManagement.Web.Generators
{
    /// <summary>
    ///    Error message generator.
    /// </summary>
    public static class ErrorMessageGenerator
    {
        #region Fields

        private static string ErrorMessageTemplate { get; set; } = "Error Code: {0}- Error Description: {1}";

        #endregion

        #region Ctor

        #endregion

        #region Methods

        /// <summary>
        ///     This method generates an error message based on the specified <paramref name="errorObject"/> and updates the
        ///     <paramref name="errMessage"/> with the generated message.
        /// </summary>
        /// <param name="errorObject">An object that contains the error code and message.</param>
        /// <param name="errMessage">A output to a string variable that will be updated with the generated error message.</param>
        public static void ErrorMessageGeneratorMethod(ErrorsMessagesConstants errorObject, out string errMessage)
        {
            errMessage = string.Format(ErrorMessageTemplate, errorObject.Code, errorObject.Message);
        }

        #endregion
    }
}
