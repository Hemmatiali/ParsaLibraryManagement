namespace ParsaLibraryManagement.Domain.Common.Extensions;

/// <summary>
///     Provides extension methods for working with <see cref="Guid"/>.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    ///     Converts the specified string representation of a GUID to its <see cref="Guid"/> equivalent.
    /// </summary>
    /// <param name="value">The string representation of the GUID.</param>
    /// <returns>
    /// A <see cref="Guid"/> instance if the conversion succeeds; otherwise, <see cref="Guid.Empty"/>.
    /// </returns>
    /// <remarks>
    ///     This method attempts to parse the input string and return its corresponding <see cref="Guid"/> representation.
    ///     If the parsing fails, it returns <see cref="Guid.Empty"/>.
    /// </remarks>
    public static Guid ToGuid(this string value)
    {
        return Guid.TryParse(value, out var guid) ? guid : Guid.Empty;
    }
}