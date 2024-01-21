using FluentValidation.Results;

namespace ParsaLibraryManagement.Application.Utilities;

/// <summary>
///     Static class providing helper methods for validation purposes.
/// </summary>
/// <remarks>
///     This class contains static methods that can be used to assist with various validation tasks.
/// </remarks>
public static class ValidationHelper
{
    #region Methods

    /// <summary>
    ///     Gets a concatenated string of error messages from a ValidationResult object.
    /// </summary>
    /// <param name="validationResult">
    ///     The ValidationResult object containing the collection of validation errors.
    /// </param>
    /// <returns>
    ///     A string representing the concatenated error messages separated by commas.
    /// </returns>
    /// <remarks>
    ///     This method is useful for consolidating error messages from validation results into a single string.
    /// </remarks>
    public static string GetErrorMessages(ValidationResult validationResult)
    {
        return string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
    }

    #endregion
}