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
        public const string HasRelationOnSubCategoriesMsg = "Has relation on sub categories.";

        // Image messages
        public const string ImageUploadFailedMsg = "Failed to upload image.";
        public const string UploadImageMsg = "Please upload an image file.";

        // Validator messages
        public const string RequiredFieldMsg = "{0} is required.";
        public const string LengthBetweenMsg = "{0} must be between {1} and {2} characters.";
        public const string MaximumLengthMsg = "{0} must be maximum {1} characters.";
        public const string HasRelationOnWithPlaceHolderMsg = "Has relation on {0}.";
        public const string NotValid = "{0} not valid";
        public const string DoesNotExist = "The {0} does not exist in the database.";
        public const string DuplicatedValue = "The {0} Can Not Be Duplicated.";
        
    }
}