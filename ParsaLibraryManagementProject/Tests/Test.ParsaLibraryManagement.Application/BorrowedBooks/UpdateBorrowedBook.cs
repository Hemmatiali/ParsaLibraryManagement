using AutoMapper;
using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Services;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces.Repository;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Application.Validators;

using Xunit;
using EmptyFiles;
using Test.ParsaLibraryManagement.Application.Publishers;

namespace Test.ParsaLibraryManagement.Application.booksCategorys;

public class UpdateBorrowedBook
{
    /// <summary>
    /// check Method Update
    /// </summary>
    [Fact]
    public async void booksCategory_Update_ShouldTrue()
    {
        try
        {
            //Arrange
            int BorrowedBookid = 1;
            #region Mock
            Moq.Mock<IMapper> mockIMapper = new Moq.Mock<IMapper>();
            Moq.Mock<IRepositoryFactory> mockrepositoryFactory = new Moq.Mock<IRepositoryFactory>();
            Moq.Mock<IBorrowedBookRepository> BorrowedBookRepository = new Moq.Mock<IBorrowedBookRepository>();

            Moq.Mock<IBaseRepository<BorrowedBook>> _baseBorrowedBookRepository = new Moq.Mock<IBaseRepository<BorrowedBook>>();


            mockrepositoryFactory.Setup(x => x.GetRepository<BorrowedBook>()).Returns(_baseBorrowedBookRepository.Object);

            // mockrepositoryFactory.Expect(p => p.GetRepository<BorrowedBook>());

            mockrepositoryFactory.Setup(x => x.GetRepository<BorrowedBook>()).Returns(_baseBorrowedBookRepository.Object);
            _baseBorrowedBookRepository.Setup(x => x.GetByIdAsync(BorrowedBookid)).Returns(TestDataBorrowedBook.getTestData(BorrowedBookid));



            #endregion

            #region fields


            IValidator<BorrowedBookDto> BorrowedBookDto = new BorrowedBookValidator();
            IValidator<BorrowedBookEditDto> BorrowedBookEditDto = new BorrowedBookEditValidator();

            IBorrowedBookServices BorrowedBookServices = new BorrowedBookServices(
               mockIMapper.Object, BorrowedBookDto, BorrowedBookEditDto, mockrepositoryFactory.Object,
               BorrowedBookRepository.Object);




            var _BookCategoryDto = new BorrowedBookEditDto()
            {
                BookId = 1,
                BorrowedId=BorrowedBookid,
                UserId = 1,
                
                BackEndDate = DateTime.Now
            };


            #endregion
            //act

            var _res = await BorrowedBookServices.UpdateBorrowedAsync(_BookCategoryDto);
            //assert
            Assert.True(_res.WasSuccess);
            Assert.True(true);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }


    /// <summary>
    /// check Method Update
    /// </summary>
    [Fact]
    public async void booksCategory_Update_Shouldflase()
    {
        try
        {
            //Arrange
            int BorrowedBookid = 1;
            #region Mock
            Moq.Mock<IMapper> mockIMapper = new Moq.Mock<IMapper>();
            Moq.Mock<IRepositoryFactory> mockrepositoryFactory = new Moq.Mock<IRepositoryFactory>();
            Moq.Mock<IBorrowedBookRepository> BorrowedBookRepository = new Moq.Mock<IBorrowedBookRepository>();

            Moq.Mock<IBaseRepository<BorrowedBook>> _baseBorrowedBookRepository = new Moq.Mock<IBaseRepository<BorrowedBook>>();


            mockrepositoryFactory.Setup(x => x.GetRepository<BorrowedBook>()).Returns(_baseBorrowedBookRepository.Object);

            // mockrepositoryFactory.Expect(p => p.GetRepository<BorrowedBook>());
            // mockrepositoryFactory.Expect(p => p.GetRepository<BorrowedBook>());

            mockrepositoryFactory.Setup(x => x.GetRepository<BorrowedBook>()).Returns(_baseBorrowedBookRepository.Object);
            _baseBorrowedBookRepository.Setup(x => x.GetByIdAsync(BorrowedBookid)).Returns(TestDataBorrowedBook.getTestData(BorrowedBookid));


            #endregion

            #region fields


            IValidator<BorrowedBookDto> BorrowedBookDto = new BorrowedBookValidator();
            IValidator<BorrowedBookEditDto> BorrowedBookEditDto = new BorrowedBookEditValidator();

            IBorrowedBookServices BorrowedBookServices = new BorrowedBookServices(
               mockIMapper.Object, BorrowedBookDto, BorrowedBookEditDto, mockrepositoryFactory.Object,
               BorrowedBookRepository.Object);





            var _booksCategoryeditDto = new BorrowedBookEditDto()
            {
                BookId = 1,
                UserId = 1,
                BorrowedId=BorrowedBookid,
                StartDateBorrowed = DateTime.Now
            };

            #endregion

            var _res = await BorrowedBookServices.UpdateBorrowedAsync(_booksCategoryeditDto);
            //assert
            
            Assert.True(!_res.WasSuccess);

        }
        catch (Exception ex)
        {
            Assert.True(true);
        }
    }
}
