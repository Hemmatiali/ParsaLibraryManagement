namespace ParsaLibraryManagement.Domain.Common.Extensions;

/// <summary>
///     Provides extension methods for working with <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Normalizes and trims the input string by converting it to lowercase and removing leading and trailing white spaces.
    /// </summary>
    /// <param name="input">The string to normalize and trim.</param>
    /// <returns>A normalized and trimmed string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input string is null or white space.</exception>
    public static string NormalizeAndTrim(this string input)
    {
        // Check input
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentNullException(nameof(input), ErrorMessages.InputCannotBeNullWhiteSpaceMsg);

        // Return result
        return input.ToLower().Trim();
    }
}