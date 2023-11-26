using AutoMapper;
using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Infrastructure.Data.Contexts;

namespace ParsaLibraryManagement.Application.Services
{
    //todo xml
    public class BookCategoryServices : IBooksCategoryServices
    {
        #region Fields

        private readonly ParsaLibraryManagementDBContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<BookCategoryDto> _validator;

        #endregion
        #region Ctor

        public BookCategoryServices(ParsaLibraryManagementDBContext context, IMapper mapper, IValidator<BookCategoryDto> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void CreateCategory(BookCategoryDto categoryDto)
        {
            // Validate DTO
            var validationResult = _validator.Validate(categoryDto);
            if (!validationResult.IsValid)
            {
                // Handle validation errors. Throw an exception with errors responses
                throw new ValidationException(validationResult.Errors);
            }

            // Mapper
            var category = _mapper.Map<BooksCategory>(categoryDto);

            // Add in DB
            _context.BooksCategories.Add(category);
            _context.SaveChanges();
        }

        #endregion
    }
}
