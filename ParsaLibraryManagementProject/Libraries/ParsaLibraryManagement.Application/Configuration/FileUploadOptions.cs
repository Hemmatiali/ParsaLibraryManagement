namespace ParsaLibraryManagement.Application.Configuration
{
    /// <summary>
    ///     Options for configuring file uploads, including maximum file size and allowed file extensions.
    /// </summary>
    /// <remarks>
    ///     This class defines options for controlling various aspects of file uploads, such as size and allowed file extensions.
    /// </remarks>
    public class FileUploadOptions
    {
        public int MaxFileSize { get; set; }
        public string[] AllowedExtensions { get; set; }
    }
}
