using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Web.ViewModels.Publisher;

public class PublisherViewModel
{
    public PublisherDto Publisher { get; set; }

    public List<SelectListItem>? Genders { get; set; }
}