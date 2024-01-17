using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Web.ViewModels.Publisher;

/// <summary>
///     View model for creating and editing publishers.
/// </summary>
/// <remarks>
///     This class represents the view model used when creating or editing publishers, including data for the publisher, reference groups, and additional properties.
/// </remarks>
public class PublisherViewModel
{
    public PublisherDto Publisher { get; set; } = new();

    public List<SelectListItem> Genders { get; set; } = new();
}