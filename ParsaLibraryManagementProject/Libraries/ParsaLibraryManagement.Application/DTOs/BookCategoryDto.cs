namespace ParsaLibraryManagement.Application.DTOs
{
    //todo xml
    public class BookCategoryDto
    {
        public short CategoryId { get; set; }
        public string Title { get; set; } = "";
        public string ImageAddress { get; set; } = "";
        public short? RefId { get; set; } // Optional, based on domain logic
    }
}
