namespace ParsaLibraryManagement.Infrastructure.Common.Constants;

/// <summary>
///     Contains constant values related to file paths in the application.
/// </summary>
/// <remarks>
///     This static class provides a centralized location for storing file path constants used throughout the application.
/// </remarks>
public static class PathConstants
{
    #region General Images

    public static string GeneralImgPathPhysicalAddress = "wwwroot\\images";
    public static string GeneralImgPath = "wwwroot/images";
    public static string GeneralImgPathWithoutWwwRoot = "images";


    public static string GeneralThumbnailImgPathPhysicalAddress = "wwwroot\\images\\thumbnail";
    public static string GeneralThumbnailImgPath = "wwwroot/images/thumbnail";
    public static string GeneralThumbnailImgPathWithoutWwwRoot = "images/thumbnail";

    #endregion

    #region Book category

    public const string BookCategoriesFolderName = "BookCategories";
    public const string BookCategoriesImgPathPhysicalAddress = "wwwroot\\images\\BookCategories";
    public const string BookCategoriesImgPath = "wwwroot/images/BookCategories";
    public const string BookCategoriesImgPathWithoutWwwRoot = "images/BookCategories";

    public const string BookCategoriesThumbnailImgPathPhysicalAddress = "wwwroot\\images\\BookCategories\\thumbnail";
    public const string BookCategoriesThumbnailImgPath = "wwwroot/images/BookCategories/thumbnail";
    public const string BookCategoriesThumbnailImgPathWithoutWwwRoot = "images/BookCategories/thumbnail";

    #endregion

    #region Books

    public const string BooksFolderName = "Books";
    public const string BooksImgPathWithoutWwwRoot = "images/Books";
    public static string ThumbnailBooksImgPathWithoutWwwRoot = "images/thumbnail/Books";

    #endregion
}