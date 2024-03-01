using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Infrastructure.Data.Contexts;
using ParsaLibraryManagement.Infrastructure.Data.Repositories;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Common;

namespace Test.ParsaLibraryManagement.IntegrationTests.Infrastructure.Data.Repositories;

/// <summary>
///     Integration tests for <see cref="BooksCategoryRepository"/>.
/// </summary>
public class BooksCategoryRepositoryTests : IDisposable
{
    #region Fields

    private readonly ParsaLibraryManagementDbContext _context;
    private readonly BooksCategoryRepository _repository;

    #endregion

    #region Ctor

    /// <summary>
    ///     Initialize a new instance of <see cref="BooksCategoryRepositoryTests"/>
    /// </summary>
    public BooksCategoryRepositoryTests()
    {
        // Setup in-memory database for each test
        var options = new DbContextOptionsBuilder<ParsaLibraryManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for isolation
            .Options;

        _context = new ParsaLibraryManagementDbContext(options);
        _repository = new BooksCategoryRepository(_context);

        SeedDatabase();
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Seeds the database with test data.
    /// </summary>
    private void SeedDatabase()
    {
        // Seed the database with test data
        var categories = new List<BookCategory>()
        {
            new () { CategoryId = 1, Title = "Test Category 1", RefId = null },
            new () { CategoryId = 2, Title = "Fiction", RefId = null },
            new () { CategoryId = 3, Title = "Non-Fiction", RefId = null },
            new () { CategoryId = 4, Title = "Science Fiction", RefId = 2 },
            new () { CategoryId = 5, Title = "Fantasy", RefId = 2 },
            new () { CategoryId = 6, Title = "Biography", RefId = 3 }
        };

        _context.BooksCategories.AddRange(categories);
        _context.SaveChanges();
    }

    /// <summary>
    ///     Performs cleanup operations after each test.
    /// </summary>
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    #region HasChildRelations Method Tests

    /// <summary>
    ///     Test method for checking if a category has no child relations.
    /// </summary>
    /// <remarks>
    ///     This test verifies that a category without children returns false.
    /// </remarks>
    /// <seealso cref="BooksCategoryRepository.HasChildRelations(short)"/>
    [Fact]
    public async Task HasChildRelations_WithCategoryIdHavingNoChildren_ReturnsFalse()
    {
        // Arrange
        var categoryId = 1; // Assuming this category has no child relations

        // Act
        var result = await _repository.HasChildRelations((short)categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasSuccess);
    }

    /// <summary>
    ///     Test method for checking if a category has children.
    /// </summary>
    /// <remarks>
    ///     This test verifies that a category with children returns true.
    /// </remarks>
    /// <seealso cref="BooksCategoryRepository.HasChildRelations(short)"/>
    [Fact]
    public async Task HasChildRelations_WithCategoryIdHavingChildren_ReturnsTrue()
    {
        // Arrange
        // Assuming category 1 is a parent category to category 2
        var childCategory = new BookCategory { CategoryId = 7, Title = "Urban Fantasy", RefId = 6 };
        _context.BooksCategories.Add(childCategory);
        await _context.SaveChangesAsync();

        short categoryId = 6; // Parent category

        // Act
        var result = await _repository.HasChildRelations(categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.WasSuccess);
        Assert.Equal(ErrorMessages.HasRelationOnSubCategoriesMsg, result.Message);
    }

    /// <summary>
    ///     Test method for checking if a category with a non-existing ID returns false for having child relations.
    /// </summary>
    /// <remarks>
    ///     This test verifies that providing a non-existing category ID returns false for having child relations.
    /// </remarks>
    /// <seealso cref="BooksCategoryRepository.HasChildRelations(short)"/>
    [Fact]
    public async Task HasChildRelations_WithNonExistingCategoryId_ReturnsFalse()
    {
        // Arrange
        var categoryId = 999;  // Assuming Non-existing category ID

        // Act
        var result = await _repository.HasChildRelations((short)categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasSuccess);
    }

    /// <summary>
    ///     Test method for checking if a category with books but no child categories returns true with the correct message.
    /// </summary>
    /// <remarks>
    ///     This test verifies that a category with books but no child categories returns true and provides the correct message.
    /// </remarks>
    /// <seealso cref="BooksCategoryRepository.HasChildRelations(short)"/>
    [Fact]
    public async Task HasChildRelations_WithCategoryIdHavingBooksButNoChildCategories_ReturnsTrueWithCorrectMessage()
    {
        // Arrange
        short categoryIdWithNoChildCategoriesButHasBooks = 4; // Assuming that has no child categories

        // Assuming that there's at least one book associated with this category ID
        var book = new Book { Id = 7, Name = "Unique Test Book", CategoryId = categoryIdWithNoChildCategoriesButHasBooks };
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.HasChildRelations(categoryIdWithNoChildCategoriesButHasBooks);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.WasSuccess);
        Assert.Equal(string.Format(ErrorMessages.HasRelationOnWithPlaceHolderMsg, nameof(Book)), result.Message);
    }

    #endregion

    #region GetBookCategoriesAsync Method Tests

    /// <summary>
    ///     Test method for retrieving book categories with a valid prefix.
    /// </summary>
    /// <remarks>
    ///     This test ensures that the correct categories are returned based on the provided prefix.
    /// </remarks>
    /// <seealso cref="BooksCategoryRepository.GetBookCategoriesAsync(string)"/>
    [Fact]
    public async Task GetBookCategoriesAsync_WithValidPrefix_ReturnsExpectedCategories()
    {
        // Arrange
        var prefix = "Test";

        // Act
        var result = await _repository.GetBookCategoriesAsync(prefix);

        // Assert
        Assert.NotEmpty(result);
        Assert.All(result, category => Assert.StartsWith(prefix, category.Title));
    }

    /// <summary>
    ///     Test method for retrieving book categories with a very long prefix.
    /// </summary>
    /// <remarks>
    ///     This test verifies that providing a very long prefix returns an empty list.
    /// </remarks>
    /// <seealso cref="BooksCategoryRepository.GetBookCategoriesAsync(string)"/>
    [Fact]
    public async Task GetBookCategoriesAsync_WithVeryLongString_ReturnsEmpty()
    {
        // Arrange
        var prefix = new string('a', 1000); // Very long string

        // Act
        var result = await _repository.GetBookCategoriesAsync(prefix);

        // Assert
        Assert.Empty(result);
    }

    /// <summary>
    ///     Test method for retrieving book categories with special characters as a prefix.
    /// </summary>
    /// <remarks>
    ///     This test verifies that providing special characters as a prefix returns an empty list.
    /// </remarks>
    /// <seealso cref="BooksCategoryRepository.GetBookCategoriesAsync(string)"/>
    [Fact]
    public async Task GetBookCategoriesAsync_WithSpecialCharacters_ReturnsEmpty()
    {
        // Arrange
        var prefix = "#$%^&*";

        // Act
        var result = await _repository.GetBookCategoriesAsync(prefix);

        // Assert
        Assert.Empty(result);
    }

    /// <summary>
    ///     Test method for retrieving book categories when the database is empty.
    /// </summary>
    /// <remarks>
    ///     This test verifies that an empty list is returned when the database has no categories.
    /// </remarks>
    /// <seealso cref="BooksCategoryRepository.GetBookCategoriesAsync(string)"/>
    [Fact]
    public async Task GetBookCategoriesAsync_WithEmptyDatabase_ReturnsEmptyList()
    {
        // Arrange
        _context.BooksCategories.RemoveRange(_context.BooksCategories);
        await _context.SaveChangesAsync();
        var prefix = "Any";

        // Act
        var result = await _repository.GetBookCategoriesAsync(prefix);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #endregion
}