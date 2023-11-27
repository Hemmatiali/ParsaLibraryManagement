namespace ParsaLibraryManagement.Web.ValidationServices
{
    //todo xml
    public class ImageFileValidationService
    {
        #region Fields

        private readonly int _maxFileSize;
        private readonly string[]? _allowedExtensions;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public ImageFileValidationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _maxFileSize = _configuration.GetValue<int>("FileUploadOptions:MaxFileSize");
            _allowedExtensions = _configuration.GetSection("FileUploadOptions:AllowedExtensions").Get<string[]>();

        }

        #endregion

        #region Methods

        //todo xml
        public bool ValidateFile(IFormFile file, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (file.Length > _maxFileSize)
            {
                errorMessage = $"File size exceeds {_maxFileSize} bytes.";
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedExtensions.Contains(extension))
            {
                errorMessage = "Invalid file extension.";
                return false;
            }

            return true;
        }

        #endregion

    }
}
