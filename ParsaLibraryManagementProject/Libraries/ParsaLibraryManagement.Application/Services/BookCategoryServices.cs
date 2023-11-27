using AutoMapper;
using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces.Repository;

namespace ParsaLibraryManagement.Application.Services
{
    //todo xml
    public class BookCategoryServices : IBookCategoryServices
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IValidator<BookCategoryDto> _validator;
        private readonly IBaseRepository<BooksCategory> _repository;

        #endregion

        #region Ctor

        public BookCategoryServices(IMapper mapper, IValidator<BookCategoryDto> validator, IRepositoryFactory repositoryFactory)
        {
            _mapper = mapper;
            _validator = validator;
            _repository = repositoryFactory.GetRepository<BooksCategory>();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public async Task<List<BookCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repository.GetAllAsync();
            var categoryDtos = categories.Select(c => _mapper.Map<BookCategoryDto>(c)).ToList();
            return categoryDtos;
        }

        /// <inheritdoc />
        public async Task<string?> CreateCategoryAsync(BookCategoryDto categoryDto)
        {
            // Validate DTO
            var validationResult = await _validator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return errorMessages;
            }

            // Mapper
            var category = _mapper.Map<BooksCategory>(categoryDto);

            // Adds the category to the repository
            await _repository.AddAsync(category);

            // Saves changes to the database
            await _repository.SaveChangesAsync();

            return null;
        }

        #endregion
    }
}
