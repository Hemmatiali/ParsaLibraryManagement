using Xunit;
using ParsaLibraryManagement.Domain.Entities;
using System.Net;
using System.Security.Cryptography;

namespace Test.ParsaLibraryManagement.Domain.Entities.BorrowedBooks;

/// <summary>
///     Unit tests for the <see cref="BorrowedBook"/> class.
/// </summary>
public class BorrowedBookTests
{
    #region Methods

    /// <summary>
    ///     Ensures that the constructor initializes the InverseRef collection correctly.
    /// </summary>
    [Fact]
    public void User_CheckInitializesBooks_ShouldTrue()
    {
        // Arrange & Act
        var BorrowedBook = new BorrowedBook();

        // Assert
        Assert.Null(BorrowedBook.Book);
    }

    /// <summary>
    ///     Ensures that the constructor initializes the Books collection correctly.
    /// </summary>
    [Fact]
    public void User_CheckInitializesUser_ShouldTrue()
    {
        // Arrange & Act
        var BorrowedBook = new BorrowedBook();

        // Assert
        Assert.Null(BorrowedBook.User);
    }
    /// <summary>
    ///     Test data for the <see cref="Properties_ShouldBeSetCorrectly"/> test.
    /// </summary>
    /// <returns>An enumerable collection of test data.</returns>
    public static IEnumerable<object[]> BookBorrowedBookTestData()
    {
        yield return new BorrowedBook[] { new BorrowedBook() {
             Bid=1
            ,BookId=5
           
            ,UserId=1
            ,StartDateBorrowed=DateTime.Now
            ,BackEndDate=DateTime.Now
        } };
        yield return new BorrowedBook[] { new BorrowedBook() {
             Bid=2
            ,BookId=51
            ,UserId=11
            ,StartDateBorrowed=DateTime.Now
            ,BackEndDate=DateTime.Now
        }  };
    }
    /// <summary>
    ///     Ensures that properties are set correctly when using the constructor.
    /// </summary>
    /// <param name="categoryId">The category ID.</param>
    /// <param name="title">The category title.</param>
    /// <param name="imageAddress">The image address.</param>
    /// <param name="refId">The reference ID.</param>
    [Theory]
    [MemberData(nameof(BookBorrowedBookTestData))]
    public void Properties_ShouldBeSetCorrectly(BorrowedBook borrowedBook)
    {
        // Arrange
        var BorrowedBook = new BorrowedBook
        {
            StartDateBorrowed = borrowedBook.StartDateBorrowed,
            Bid = borrowedBook.Bid,
            BookId = borrowedBook.BookId,
            BackEndDate = borrowedBook.BackEndDate,
            UserId = borrowedBook.UserId,

        };

        // Act & Assert
        Assert.Equal(borrowedBook.StartDateBorrowed, BorrowedBook.StartDateBorrowed);
        Assert.Equal(borrowedBook.Bid, BorrowedBook.Bid);
        Assert.Equal(borrowedBook.BookId, BorrowedBook.BookId);
        Assert.Equal(borrowedBook.BackEndDate, BorrowedBook.BackEndDate);
        Assert.Equal(borrowedBook.UserId, BorrowedBook.UserId);

    }

    /// <summary>
    ///     Ensures that adding a book to the Books collection works correctly.
    /// </summary>
    [Fact]
    public void BorrowedBook_AddingBookToBooks_ShouldWorkCorrectly()
    {
        // Arrange
        var BorrowedBook = new BorrowedBook();
        var book = new Book(); // Assuming 'Book' is a valid entity

        // Act
        BorrowedBook.Book = book;

        // Assert
        Assert.Equal(book, BorrowedBook.Book);
    }

    [Fact]
    public void BorrowedBook_AddinguserTousers_ShouldWorkCorrectly()
    {
        // Arrange
        var BorrowedBook = new BorrowedBook();
        var user = new User(); // Assuming 'user' is a valid entity

        // Act
        BorrowedBook.User = user;

        // Assert
        Assert.Equal(user, BorrowedBook.User);
    }
    #endregion
}
