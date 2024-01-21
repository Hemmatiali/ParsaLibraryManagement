namespace ParsaLibraryManagement.Application.Utilities;

/// <summary>
///     Helper class for data-related operations.
/// </summary>
/// <remarks>
///     This class provides static methods for generating and manipulating data.
/// </remarks>
public static class DataHelper
{
    #region Methods

    /// <summary>
    ///     Generates and returns a list of alphabet letters from 'A' to 'Z'.
    /// </summary>
    /// <returns>A list of strings representing the alphabet letters.</returns>
    public static List<string> GenerateAlphabetLetters() => Enumerable.Range('A', 'Z' - 'A' + 1).Select(i => ((char)i).ToString()).ToList();

    #endregion
}
