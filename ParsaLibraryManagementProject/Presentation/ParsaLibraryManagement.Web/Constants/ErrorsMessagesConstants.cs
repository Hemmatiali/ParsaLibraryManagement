using ParsaLibraryManagement.Web.Enums;

namespace ParsaLibraryManagement.Web.Constants
{
    /// <summary>
    ///     Represents a list of messages for handled errors constants.
    ///     Type of messages: Error, Warning.
    /// </summary>
    public class ErrorsMessagesConstants
    {
        #region Fields

        /// <summary>
        ///     Gets the error type.
        /// </summary>
        public ErrorCodeTypesEnum ErrorType { get; }

        /// <summary>
        ///     Gets the error code.
        /// </summary>
        public int Code { get; }

        /// <summary>
        ///     Gets the error message.
        /// </summary>
        public string Message { get; }

        #endregion

        #region Ctor

        /// <summary>
        ///     Initializes a new instance of the <see cref="ErrorsMessagesConsts"/> class with the specified code and message.
        /// </summary>
        /// <param name="errorType">The error type code.</param>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        private ErrorsMessagesConstants(ErrorCodeTypesEnum errorType, int code, string message)
        {
            ErrorType = errorType;
            Code = code;
            Message = message;
        }

        #endregion

        #region Methods

        #region Un handled error messages

        /// <summary>
        ///		Un handled error messages
        ///     Error Code: 500
        ///     Type: Error
        /// </summary>
        public static ErrorsMessagesConstants UnHandledErrMsg => new(ErrorCodeTypesEnum.Error, 500, "Unhandled error.");

        #endregion

        #region CRUD error messages

        #region Create error messages
        //3001

        /// <summary>
        ///		Unsuccessful create item error messages
        ///     Error Code: 3001
        ///     Type: Error
        /// </summary>
        public static ErrorsMessagesConstants UnSuccessfulCreateItemErrMsg => new(ErrorCodeTypesEnum.Error, 3001, "Unsuccessful create item.");

        #endregion

        #region Read error messages
        //4001

        /// <summary>
        ///		Unsuccessful read item error messages
        ///     Error Code: 4001
        ///     Type: Error
        /// </summary>
        public static ErrorsMessagesConstants UnSuccessfulReadItemErrMsg => new(ErrorCodeTypesEnum.Error, 4001, "Unsuccessful read item.");

        /// <summary>
        ///		Unsuccessful read items error messages
        ///     Error Code: 4002
        ///     Type: Error
        /// </summary>
        public static ErrorsMessagesConstants UnSuccessfulReadItemsErrMsg => new(ErrorCodeTypesEnum.Error, 4002, "Unsuccessful read items.");

        #endregion

        #region Update error messages
        //5001

        /// <summary>
        ///		Unsuccessful edit item error messages
        ///     Error Code: 5001
        ///     Type: Error
        /// </summary>
        public static ErrorsMessagesConstants UnSuccessfulEditItemErrMsg => new(ErrorCodeTypesEnum.Error, 5001, "Unsuccessful edit item.");

        #endregion

        #region Delete error messages
        //6001

        /// <summary>
        ///		Unsuccessful delete item error messages
        ///     Error Code: 6001
        ///     Type: Error
        /// </summary>
        public static ErrorsMessagesConstants UnSuccessfulDeleteItemErrMsg => new(ErrorCodeTypesEnum.Error, 6001, "Unsuccessful delete item.");

        #endregion

        #endregion

        #endregion

    }
}
