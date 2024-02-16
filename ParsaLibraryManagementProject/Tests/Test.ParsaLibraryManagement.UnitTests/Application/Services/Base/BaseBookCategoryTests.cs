using AutoMapper;
using FluentValidation;
using ParsaLibraryManagement.Application.Interfaces.ImageServices;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Domain.Interfaces.ImageServices;

namespace Test.ParsaLibraryManagement.UnitTests.Application.Services.Base;

/// <summary>
///     Base class for unit tests related to book category operations.
/// </summary>
/// <remarks>
///     Implementations of specific test cases should inherit from this class.
/// </remarks>
public abstract class BaseBookCategoryTests
{
    #region Fields

    protected const string SimulatedDatabaseFailureError = "Simulated database failure";

    protected const string BookCategoryTitle = "Fiction";
    protected const string BookCategoryTitle2 = "Non-Fiction";
    protected const string BookCategoryTitle3 = "Finance";
    protected const string BookCategoryTitle4 = "Historical Fiction";


    protected const string FolderName = "TestFolder";

    protected readonly Mock<IMapper> MockMapper = new();
    protected readonly Mock<IValidator<BookCategoryDto>> MockValidator = new();
    protected readonly Mock<IRepositoryFactory> MockRepositoryFactory = new();
    protected readonly Mock<IBaseRepository<BookCategory>> MockBaseRepository = new();
    protected readonly Mock<IBooksCategoryRepository> MockBooksCategoryRepository = new();
    protected readonly Mock<IImageFileValidationService> MockImageFileValidationServices = new();
    protected readonly Mock<IImageServices> MockImageServices = new();

    #endregion

    #region Ctor

    protected BaseBookCategoryTests()
    {
        // Setup the repository factory to return the mocked base repository
        MockRepositoryFactory.Setup(factory => factory.GetRepository<BookCategory>())
            .Returns(MockBaseRepository.Object);
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Creates a <see cref="BookCategoryServices"/> instance for testing purposes.
    /// </summary>
    /// <returns>A new instance of <see cref="BookCategoryServices"/>.</returns>
    protected BookCategoryServices CreateService() =>
        new(MockMapper.Object, MockValidator.Object, MockRepositoryFactory.Object, MockBooksCategoryRepository.Object, MockImageFileValidationServices.Object, MockImageServices.Object);

    #endregion
}