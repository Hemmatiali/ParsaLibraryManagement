namespace ParsaLibraryManagement.Domain.Common
{
    /// <summary>
    ///     Static class containing error messages used in the application.
    /// </summary>
    /// <remarks>
    ///     This class provides a centralized location for storing error messages used throughout the application.
    /// </remarks>
    public static class ErrorMessages
    {
        // General messages
        public const string ItemNotFoundMsg = "Item not found.";
        public const string HasRelationOnSubCategoriesMsg = "Has relation on subcategories.";
        public const string InputCannotBeNullWhiteSpaceMsg = "Input cannot be null or whitespace.";

        // Category messages
        public const string CircularHierarchyMsg = "Circular hierarchy detected. A category cannot be its own parent or ancestor.";

        // Image messages
        public const string ImageUploadFailedMsg = "Failed to upload image.";
        public const string UploadImageMsg = "Please upload an image file.";

        // Validator messages
        public const string RequiredFieldMsg = "{0} is required.";
        public const string LengthBetweenMsg = "{0} must be between {1} and {2} characters.";
        public const string MaximumLengthMsg = "{0} must be a maximum of {1} characters.";
        public const string HasRelationOnWithPlaceHolderMsg = "Has relation on {0}.";
        public const string NotValid = "The {0} is not valid.";
        public const string Exist = "The {0} exists in the database.";
        public const string DoesNotExist = "The {0} does not exist in the database.";
        public const string DuplicatedValue = "The {0} cannot be duplicated.";

    }

}