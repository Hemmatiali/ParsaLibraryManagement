using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Web.ViewModels.Publisher
{
    /// <summary>
    ///     View model for deleting a publisher, containing data for the publisher to be deleted.
    /// </summary>
    /// <remarks>
    ///     This class represents the view model used when confirming the deletion of a publisher.
    /// </remarks>
    public class PublisherDeleteViewModel
    {
        public PublisherDto Publisher { get; set; } = new();
    }
}
