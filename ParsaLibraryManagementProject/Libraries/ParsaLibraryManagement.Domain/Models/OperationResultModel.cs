namespace ParsaLibraryManagement.Domain.Models
{
    /// <summary>
    ///     Represents the result of an operation, indicating whether it was successful and an associated message.
    /// </summary>
    /// <remarks>
    ///     This class is used to convey the result of an operation, including a success indicator and an optional message.
    /// </remarks>
    public class OperationResultModel
    {
        public bool WasSuccess { get; set; }
        public string Message { get; set; } = "";
    }
}
