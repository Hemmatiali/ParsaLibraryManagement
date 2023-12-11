using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.DTOs;
using System.ComponentModel.DataAnnotations;
using ParsaLibraryManagement.Domain.Common;

namespace ParsaLibraryManagement.Web.ViewModels.BookCategories
{
    /// <summary>
    ///     View model for creating and editing book categories.
    /// </summary>
    /// <remarks>
    ///     This class represents the view model used when creating or editing book categories, including data for the category, reference groups, and an image file.
    /// </remarks>
    public class BookCategoryViewModel
    {
        public BookCategoryDto Category { get; set; } = new();

        public List<SelectListItem> RefGroups { get; set; } = new();

        [Required(ErrorMessage = ErrorMessages.UploadImageMsg)]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }
    }
}
