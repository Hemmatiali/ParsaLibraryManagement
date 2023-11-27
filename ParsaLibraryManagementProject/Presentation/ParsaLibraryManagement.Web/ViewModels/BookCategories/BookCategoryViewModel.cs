using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Web.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ParsaLibraryManagement.Web.ViewModels.BookCategories
{
    //todo xml
    public class BookCategoryViewModel
    {
        public BookCategoryDto Category { get; set; } = new();

        public List<SelectListItem> RefGroups { get; set; }=new();

        [Required(ErrorMessage = "Please upload an image file.")]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }
    }
}
