using ParsaLibraryManagement.Domain.Entities;
namespace Test.ParsaLibraryManagement.UnitTests.Domain.Entities;

/// <summary>
///     Unit tests for the <see cref="BookCategory"/> class.
/// </summary>
public class BookCategoryTests
{
    #region Methods

    /// <summary>
    ///     Test data for the <see cref="Properties_ShouldBeSetCorrectly"/> test.
    /// </summary>
    /// <returns>An enumerable collection of test data.</returns>
    public static IEnumerable<object[]> BookCategoryTestData()
    {
        yield return new object[] { (short)1, "Fiction", "http://example.com/image.jpg", (short?)2 };
        yield return new object[] { (short)2, "Non-Fiction", "http://example.com/image2.jpg", null };
    }

    /// <summary>
    ///     Ensures that the constructor initializes the InverseRef collection correctly.
    /// </summary>
    [Fact]
    public void Constructor_InitializesInverseRefCollection()
    {
        // Arrange & Act
        var bookCategory = new BookCategory();

        // Assert
        Assert.Empty(bookCategory.InverseRef);
    }

    /// <summary>
    ///     Ensures that the constructor initializes the Books collection correctly.
    /// </summary>
    [Fact]
    public void Constructor_InitializesBooksCollection()
    {
        // Arrange & Act
        var bookCategory = new BookCategory();

        // Assert
        Assert.Empty(bookCategory.Books);
    }

    /// <summary>
    ///     Ensures that properties are set correctly when using the constructor.
    /// </summary>
    /// <param name="categoryId">The category ID.</param>
    /// <param name="title">The category title.</param>
    /// <param name="imageAddress">The image address.</param>
    /// <param name="refId">The reference ID.</param>
    [Theory]
    [MemberData(nameof(BookCategoryTestData))]
    public void Properties_ShouldBeSetCorrectly(short categoryId, string title, string imageAddress, short? refId)
    {
        // Arrange
        var bookCategory = new BookCategory
        {
            CategoryId = categoryId,
            Title = title,
            ImageAddress = imageAddress,
            RefId = refId
        };

        // Act & Assert
        Assert.Equal(categoryId, bookCategory.CategoryId);
        Assert.Equal(title, bookCategory.Title);
        Assert.Equal(imageAddress, bookCategory.ImageAddress);
        Assert.Equal(refId, bookCategory.RefId);
    }

    /// <summary>
    ///     Ensures that adding a book to the Books collection works correctly.
    /// </summary>
    [Fact]
    public void AddingBookToBooks_ShouldWorkCorrectly()
    {
        // Arrange
        var bookCategory = new BookCategory();
        var book = new Book(); // Assuming 'Book' is a valid entity

        // Act
        bookCategory.Books.Add(book);

        // Assert
        Assert.Contains(book, bookCategory.Books);
    }

    /// <summary>
    ///     Ensures that adding a category to the InverseRef collection works correctly.
    /// </summary>
    [Fact]
    public void AddingCategoryToInverseRef_ShouldWorkCorrectly()
    {
        // Arrange
        var parentCategory = new BookCategory();
        var childCategory = new BookCategory();

        // Act
        parentCategory.InverseRef.Add(childCategory);

        // Assert
        Assert.Contains(childCategory, parentCategory.InverseRef);
    }

    #endregion
}