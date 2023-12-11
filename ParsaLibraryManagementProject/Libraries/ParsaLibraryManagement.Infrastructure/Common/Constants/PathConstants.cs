namespace ParsaLibraryManagement.Infrastructure.Common.Constants
{
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

        public static string BookCategoriesFolderName = "BookCategories";
        public static string BookCategoriesImgPathPhysicalAddress = "wwwroot\\images\\BookCategories";
        public static string BookCategoriesImgPath = "wwwroot/images/BookCategories";
        public static string BookCategoriesImgPathWithoutWwwRoot = "images/BookCategories";

        public static string BookCategoriesThumbnailImgPathPhysicalAddress = "wwwroot\\images\\BookCategories\\thumbnail";
        public static string BookCategoriesThumbnailImgPath = "wwwroot/images/BookCategories/thumbnail";
        public static string BookCategoriesThumbnailImgPathWithoutWwwRoot = "images/BookCategories/thumbnail";

        #endregion
    }
}
