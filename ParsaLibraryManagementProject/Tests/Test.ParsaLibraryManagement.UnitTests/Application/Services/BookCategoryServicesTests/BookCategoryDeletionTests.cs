namespace Test.ParsaLibraryManagement.UnitTests.Application.Services.BookCategoryServicesTests;

/// <summary>
///     Unit tests for the deletion operations of the BookCategoryServices class.
/// </summary>
/// <remarks>
///     Inherits from <see cref="BaseBookCategoryTests"/>.
/// </remarks>
public class BookCategoryDeletionTests : BaseBookCategoryTests
{
    #region Methods

    /// <summary>
    ///     Executes the deletion of a book category asynchronously and retrieves a message indicating the result.
    ///     Setup and call the action
    /// </summary>
    /// <param name="categoryId">The ID of the category to delete.</param>
    /// <param name="folderName">The name of the folder where the image is stored.</param>
    /// <returns>
    ///     A message indicating the result of the deletion operation.
    ///     Returns null if successful, otherwise returns an error message.
    /// </returns>
    private async Task<string?> ExecuteDeleteCategoryAsync(short categoryId, string folderName)
    {
        // Arrange
        var service = CreateService();

        // Act
        return await service.DeleteCategoryAsync(categoryId, folderName);
    }

    /// <summary>
    ///     Sets up the mock to simulate the existence of child relations for a specified category.
    /// </summary>
    /// <param name="categoryId">The ID of the category.</param>
    /// <param name="result">The result of the operation indicating whether child relations exist.</param>
    private void SetupHasChildRelations(short categoryId, OperationResultModel result)
    {
        MockBooksCategoryRepository.Setup(repo => repo.HasChildRelations(categoryId))
            .ReturnsAsync(result);
    }

    /// <summary>
    ///     Sets up the mock to simulate the retrieval of a category by its ID.
    /// </summary>
    /// <param name="categoryId">The ID of the category.</param>
    /// <param name="category">The category object to be returned.</param>
    private void SetupGetCategoryById(short categoryId, BookCategory? category)
    {
        MockBaseRepository.Setup(repo => repo.GetByIdAsync(categoryId))
            .ReturnsAsync(category);
    }

    /// <summary>
    ///     Sets up the mock to simulate the deletion of an image.
    /// </summary>
    /// <param name="imageName">The name of the image to be deleted.</param>
    /// <param name="folderName">The name of the folder where the image is stored.</param>
    /// <param name="expectedResult">The expected result of the image deletion operation. Returns true if successful, otherwise false.</param>
    private void SetupDeleteImage(string imageName, string? folderName, bool expectedResult)
    {
        MockImageServices.Setup(service => service.DeleteImageAsync(imageName, folderName))
            .ReturnsAsync(expectedResult); // Assuming true is returned on successful deletion & false is returned on unsuccessful deletion
    }


    #region DeleteCategoryAsync

    /// <summary>
    ///     Unit test for deleting a category when child relations exist.
    ///     Dependency Exists
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where the deletion of a category fails due to existing child relations.
    /// </remarks>
    [Fact]
    public async Task DeleteCategoryAsync_WithChildRelations_ReturnsFailureMessage()
    {
        // Arrange
        short categoryId = 1;
        string folderName = "exampleFolder";
        var failureResult = new OperationResultModel() { WasSuccess = true, Message = "Category has child relations" };

        SetupHasChildRelations(categoryId, failureResult);

        // Act
        var result = await ExecuteDeleteCategoryAsync(categoryId, folderName);

        // Assert
        Assert.Equal("Category has child relations", result);
    }

    /// <summary>
    ///     Unit test for deleting a category when the category is not found.
    ///     Not Found
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where the deletion of a category fails because the category with the specified ID does not exist.
    /// </remarks>
    [Fact]
    public async Task DeleteCategoryAsync_CategoryNotFound_ReturnsNotFoundMessage()
    {
        // Arrange
        short nonExistentCategoryId = 999;
        var successResult = new OperationResultModel() { WasSuccess = false };

        SetupHasChildRelations(nonExistentCategoryId, successResult);
        SetupGetCategoryById(nonExistentCategoryId, null);

        // Act
        var result = await ExecuteDeleteCategoryAsync(nonExistentCategoryId, FolderName);

        // Assert
        Assert.Equal(ErrorMessages.ItemNotFoundMsg, result);
    }

    /// <summary>
    ///     Unit test for deleting a category with additional functionality, such as image deletion.
    ///     Additional Functionality
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where the deletion of a category involves additional functionality, such as image deletion, and verifies the error message if image deletion fails.
    /// </remarks>
    [Fact]
    public async Task DeleteCategoryAsync_WhenImageServiceFails_ReturnsErrorMessage()
    {
        // Arrange
        short categoryId = 1;
        var successResult = new OperationResultModel() { WasSuccess = false };
        var categoryWithImage = new BookCategory { CategoryId = categoryId, ImageAddress = "image.jpg", RefId = null };

        SetupHasChildRelations(categoryId, successResult);
        SetupGetCategoryById(categoryId, categoryWithImage);
        SetupDeleteImage(categoryWithImage.ImageAddress, FolderName, false); // Simulate unsuccessful image deletion

        // Act
        var result = await ExecuteDeleteCategoryAsync(categoryId, FolderName);

        // Assert
        Assert.Equal(ErrorMessages.ImageDeleteFailedMsg, result);
        MockImageServices.Verify(service => service.DeleteImageAsync(categoryWithImage.ImageAddress, FolderName), Times.Once()); // Verify image deletion is attempted
    }

    /// <summary>
    ///     Unit test for successful deletion of a category.
    ///     Happy Path
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where a category is successfully deleted, including the deletion of associated images.
    /// </remarks>
    [Fact]
    public async Task DeleteCategoryAsync_SuccessfulDeletion_ReturnsNull()
    {
        // Arrange
        short categoryId = 1;
        var noChildResult = new OperationResultModel() { WasSuccess = false };
        var category = new BookCategory { CategoryId = categoryId, ImageAddress = "image.jpg", RefId = null };

        SetupHasChildRelations(categoryId, noChildResult);
        SetupGetCategoryById(categoryId, category);
        SetupDeleteImage(category.ImageAddress, FolderName, true); // Simulate successful image deletion

        // Act
        var result = await ExecuteDeleteCategoryAsync(categoryId, FolderName);

        // Assert
        Assert.Null(result); // Expecting null on successful deletion
        MockImageServices.Verify(service => service.DeleteImageAsync(category.ImageAddress, FolderName), Times.Once()); // Verify image deletion is attempted
        MockBaseRepository.Verify(repo => repo.RemoveAsync(category), Times.Once()); // Verify category deletion is attempted
    }

    /// <summary>
    ///     Unit test for error handling during category deletion.
    ///     Error Handling
    /// </summary>
    /// <remarks>
    ///     Tests the scenario where an exception is thrown by the repository during category deletion and ensures the method properly handles the exception.
    /// </remarks>
    [Fact]
    public async Task DeleteCategoryAsync_WhenRepositoryThrows_ThrowsException()
    {
        // Arrange
        short categoryId = 1;

        MockBooksCategoryRepository.Setup(repo => repo.HasChildRelations(categoryId))
            .ThrowsAsync(new Exception(SimulatedDatabaseFailureError));

        var service = CreateService();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.DeleteCategoryAsync(categoryId, FolderName));
    }

    #endregion

    #endregion
}