using FluentValidation.Results;

namespace ParsaLibraryManagement.Application.Utilities
{
    /// <summary>
    ///     Static class providing helper methods for validation purposes.
    /// </summary>
    /// <remarks>
    ///     This class contains static methods that can be used to assist with various validation tasks.
    /// </remarks>
    public static class ValidationHelper
    {
        public static string GetErrorMessages(ValidationResult validationResult)
        {
            return string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
        }
    }

}
