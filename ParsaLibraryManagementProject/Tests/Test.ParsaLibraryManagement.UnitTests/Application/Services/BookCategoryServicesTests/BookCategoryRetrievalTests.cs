using System.Linq.Expressions;

namespace Test.ParsaLibraryManagement.UnitTests.Application.Services.BookCategoryServicesTests;

/// <summary>
///     Unit tests for the retrieval of book categories.
/// </summary>
/// <remarks>
///     Inherits from <see cref="BaseBookCategoryTests"/>.
/// </remarks>
public class BookCategoryRetrievalTests : BaseBookCategoryTests
{
    #region Methods

    /// <summary>
    ///     Generates a list of book categories for testing purposes.
    /// </summary>
    /// <returns>A list of <see cref="BookCategory"/> objects.</returns>
    private List<BookCategory> GetBookCategories() => new()
    {
        new BookCategory { CategoryId = 1, Title = BookCategoryTitle , RefId = null},
        new BookCategory { CategoryId = 2, Title = BookCategoryTitle2, RefId = 1},
        new BookCategory { CategoryId = 3, Title = BookCategoryTitle3, RefId = 2},
        new BookCategory { CategoryId = 4, Title = BookCategoryTitle4, RefId = null},
    };

    #region GetCategoryByIdAsync

    /// <summary>
    ///     Unit test for retrieving a book category by valid ID.
    ///     Happy path
    /// </summary>
    /// <remarks>
    ///     Tests the happy path scenario where a valid category ID is provided.
    /// </remarks>
    [Fact]
    public async Task GetCategoryById_WithValidId_ReturnsCategory()
    {
        // Arrange
        short validCategoryId = 1;
        var bookCategory = new BookCategory { CategoryId = validCategoryId, Title = BookCategoryTitle }; // Mocked data
        MockBaseRepository.Setup(repo => repo.GetByIdAsync(validCategoryId))
            .ReturnsAsync(bookCategory);
        MockMapper.Setup(mapper => mapper.Map<BookCategoryDto>(It.IsAny<BookCategory>()))
            .Returns(new BookCategoryDto() { CategoryId = validCategoryId, Title = BookCategoryTitle });

        var service = CreateService();

        // Act
        var result = await service.GetCategoryByIdAsync(validCategoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(validCategoryId, result.CategoryId);
        Assert.Equal(BookCategoryTitle, result.Title);
    }

    /// <summary>
    ///     Unit test for retrieving a book category by invalid ID.
    ///     Not found
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where an invalid category ID is provided, resulting in a null return.
    /// </remarks>
    [Fact]
    public async Task GetCategoryById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        short invalidCategoryId = 199;
        MockBaseRepository.Setup(repo => repo.GetByIdAsync(invalidCategoryId))
            .ReturnsAsync((BookCategory?)null);

        var service = CreateService();

        // Act
        var result = await service.GetCategoryByIdAsync(invalidCategoryId);

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    ///     Unit test for handling exceptions thrown by the repository during category retrieval.
    ///     Throws exception
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where the repository throws an exception.
    /// </remarks>
    [Fact]
    public async Task GetCategoryById_RepositoryThrows_ThrowsException()
    {
        // Arrange
        short errorId = 500;
        MockBaseRepository.Setup(repo => repo.GetByIdAsync(errorId))
            .ThrowsAsync(new Exception(SimulatedDatabaseFailureError));

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.GetCategoryByIdAsync(errorId));
    }

    #endregion

    #region GetAllCategoriesAsync

    /// <summary>
    ///     Unit test for retrieving all book categories.
    ///     Happy path
    /// </summary>
    /// <remarks>
    ///     Tests the happy path scenario where all categories are successfully retrieved.
    /// </remarks>
    [Fact]
    public async Task GetAllCategoriesAsync_ReturnsAllCategories()
    {
        // Arrange
        var categories = GetBookCategories();
        MockBaseRepository.Setup(repo => repo.GetAllAsync(null, It.IsAny<Expression<Func<BookCategory, object>>[]>()))
            .ReturnsAsync(categories);
        MockMapper.Setup(mapper => mapper.Map<BookCategoryDto>(It.IsAny<BookCategory>()))
            .Returns<BookCategory>(bc => new BookCategoryDto { CategoryId = bc.CategoryId, Title = bc.Title });

        var service = CreateService();

        // Act
        var result = await service.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categories.Count, result.Count);
        Assert.Contains(result, dto => dto.Title == BookCategoryTitle);
        Assert.Contains(result, dto => dto.Title == BookCategoryTitle2);
    }

    /// <summary>
    ///     Unit test for retrieving all book categories when no categories are found.
    ///     Not found
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where no categories are found in the repository, resulting in an empty list.
    /// </remarks>
    [Fact]
    public async Task GetAllCategoriesAsync_WhenNoCategoriesFound_ReturnsEmptyList()
    {
        // Arrange
        MockBaseRepository.Setup(repo => repo.GetAllAsync(null, It.IsAny<Expression<Func<BookCategory, object>>[]>()))
            .ReturnsAsync(new List<BookCategory>());

        var service = CreateService();

        // Act
        var result = await service.GetAllCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    /// <summary>
    ///     Unit test for handling exceptions thrown by the repository during category retrieval.
    ///     Throws exception
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where the repository throws an exception.
    /// </remarks>
    [Fact]
    public async Task GetAllCategoriesAsync_WhenRepositoryThrows_ThrowsException()
    {
        // Arrange
        MockBaseRepository.Setup(repo => repo.GetAllAsync(null, It.IsAny<Expression<Func<BookCategory, object>>[]>()))
            .ThrowsAsync(new Exception(SimulatedDatabaseFailureError));

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.GetAllCategoriesAsync());
    }

    #endregion

    #region GetCategoriesAsync

    /// <summary>
    ///     Unit test for retrieving book categories with a valid prefix.
    ///     Happy path
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where book categories are filtered based on a valid prefix.
    /// </remarks>
    [Fact]
    public async Task GetCategoriesAsync_WithValidPrefix_ReturnsFilteredCategories()
    {
        // Arrange
        var prefix = "Fi";
        var categories = GetBookCategories().Where(c => c.Title.StartsWith(prefix)).ToList();
        MockBooksCategoryRepository.Setup(repo => repo.GetBookCategoriesAsync(prefix))
            .ReturnsAsync(categories);
        MockMapper.Setup(mapper => mapper.Map<List<BookCategoryDto>>(It.IsAny<List<BookCategory>>()))
            .Returns(categories.Select(c => new BookCategoryDto { CategoryId = c.CategoryId, Title = c.Title }).ToList());

        var service = CreateService();

        // Act
        var result = await service.GetCategoriesAsync(prefix);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categories.Count, result.Count); // Ensure only categories with the correct prefix are returned
        Assert.True(result.All(dto => dto.Title.StartsWith(prefix)));
    }

    /// <summary>
    ///     Unit test for retrieving book categories with no matching prefix.
    ///     Not found
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where no book categories match the provided prefix, resulting in an empty list.
    /// </remarks>
    [Fact]
    public async Task GetCategoriesAsync_WithNoMatchingPrefix_ReturnsEmptyList()
    {
        // Arrange
        var prefix = "NonExisting";
        MockBooksCategoryRepository.Setup(repo => repo.GetBookCategoriesAsync(prefix))
            .ReturnsAsync(new List<BookCategory>());

        var service = CreateService();

        // Act
        var result = await service.GetCategoriesAsync(prefix);

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    ///     Unit test for handling exceptions thrown by the repository during category retrieval.
    ///     Throws exception
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where the repository throws an exception.
    /// </remarks>
    [Fact]
    public async Task GetCategoriesAsync_WhenRepositoryThrows_ThrowsException()
    {
        // Arrange
        var prefix = "Error";
        MockBooksCategoryRepository.Setup(repo => repo.GetBookCategoriesAsync(prefix))
            .ThrowsAsync(new Exception(SimulatedDatabaseFailureError));

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.GetCategoriesAsync(prefix));
    }

    #endregion

    #region GetCategoriesForEditAsync

    /// <summary>
    ///     Unit test for retrieving categories for editing, excluding self and descendants.
    ///     Happy path
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where categories are filtered to exclude the specified category and its descendants.
    /// </remarks>
    [Fact]
    public async Task GetCategoriesForEditAsync_ReturnsFilteredCategories_ExcludingSelfAndDescendants()
    {
        // Arrange
        short categoryId = 1; // Parent category
        var allCategories = GetBookCategories();

        // Mock GetAllAsync without predicate
        MockBaseRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(allCategories);

        // Mock GetAllAsync with predicate
        MockBaseRepository.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<BookCategory, bool>>>(), It.IsAny<Expression<Func<BookCategory, object>>[]>()))
            .ReturnsAsync((Expression<Func<BookCategory, bool>> predicate, Expression<Func<BookCategory, object>>[] includeProperties) =>
            {
                var query = allCategories.AsQueryable();
                if (predicate != null)
                {
                    query = query.Where(predicate);
                }
                // Assume includeProperties are handled if necessary for your test
                return query.ToList();
            });

        // Setup AutoMapper mock
        MockMapper.Setup(mapper => mapper.Map<BookCategoryDto>(It.IsAny<BookCategory>()))
            .Returns((BookCategory source) => new BookCategoryDto
            {
                CategoryId = source.CategoryId,
                Title = source.Title
            });

        var service = CreateService();

        // Act
        var result = await service.GetCategoriesForEditAsync(categoryId);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, dto => dto.CategoryId == 4);
    }

    /// <summary>
    ///     Unit test for retrieving categories for editing when no categories exist.
    ///     Not found
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where no categories exist in the repository, resulting in an empty list.
    /// </remarks>
    [Fact]
    public async Task GetCategoriesForEditAsync_WhenNoCategoriesExist_ReturnsEmptyList()
    {
        // Arrange
        short categoryId = 1;
        var emptyCategories = new List<BookCategory>();

        MockBaseRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(emptyCategories);

        var service = CreateService();

        // Act
        var result = await service.GetCategoriesForEditAsync(categoryId);

        // Assert
        Assert.Empty(result);
    }

    /// <summary>
    ///     Unit test for handling exceptions thrown by the repository during category retrieval for editing.
    ///     Throws exception
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where the repository throws an exception.
    /// </remarks>
    [Fact]
    public async Task GetCategoriesForEditAsync_WhenRepositoryThrows_ThrowsException()
    {
        // Arrange
        short categoryId = 1;

        MockBaseRepository.Setup(repo => repo.GetAllAsync())
            .ThrowsAsync(new Exception(SimulatedDatabaseFailureError));

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.GetCategoriesForEditAsync(categoryId));
    }

    #endregion

    #endregion
}