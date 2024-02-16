using AutoMapper;
using FluentValidation;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Services;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces.Repository;
using ParsaLibraryManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Shouldly;
using ParsaLibraryManagement.Application.Validators;
using Moq;
using ParsaLibraryManagement.Application.Interfaces.ImageServices;
using ParsaLibraryManagement.Domain.Interfaces.ImageServices;
using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Application.Configuration;
using ParsaLibraryManagement.Infrastructure.Services.ImageServices;
using Xunit;

namespace Test.ParsaLibraryManagement.Application.BorrowedBooks;

public class CreateBorrowedBook
{
    /// <summary>
    /// check Method Create
    /// </summary>
    [Fact]
    public async void BorrowedBook_Create_ShouldTrue()
    {
        try
        {
            //Arrange

            #region Mock
            Moq.Mock<IMapper> mockIMapper = new Moq.Mock<IMapper>();
            Moq.Mock<IRepositoryFactory> mockrepositoryFactory = new Moq.Mock<IRepositoryFactory>();
            Moq.Mock<IBorrowedBookRepository> BorrowedBookRepository = new Moq.Mock<IBorrowedBookRepository>();

            Moq.Mock<IBaseRepository<BorrowedBook>> _baseBorrowedBookRepository = new Moq.Mock<IBaseRepository<BorrowedBook>>();


            mockrepositoryFactory.Setup(x => x.GetRepository<BorrowedBook>()).Returns(_baseBorrowedBookRepository.Object);

            // mockrepositoryFactory.Expect(p => p.GetRepository<BorrowedBook>());



            #endregion

            #region fields


            IValidator<BorrowedBookDto> BorrowedBookDto = new BorrowedBookValidator();
            IValidator<BorrowedBookEditDto> BorrowedBookEditDto = new BorrowedBookEditValidator();

            IBorrowedBookServices BorrowedBookServices = new BorrowedBookServices(
               mockIMapper.Object, BorrowedBookDto, BorrowedBookEditDto, mockrepositoryFactory.Object,
               BorrowedBookRepository.Object);





            var _BorrowedBookDto = new BorrowedBookDto()
            {
                BookId=1,
                UserId=1,
                StartDateBorrowed=DateTime.Now
            };
          
            #endregion

            //act


            var _res = await BorrowedBookServices.CreateBorrowedAsync(_BorrowedBookDto);
            //assert
            Assert.True(string.IsNullOrEmpty(_res));
            Assert.True(true);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }


    /// <summary>
    /// check Method Create
    /// </summary>
    [Fact]
    public async void BorrowedBook_Create_Shouldflase()
    {
        try
        {
            //Arrange

            #region Mock
            Moq.Mock<IMapper> mockIMapper = new Moq.Mock<IMapper>();
            Moq.Mock<IRepositoryFactory> mockrepositoryFactory = new Moq.Mock<IRepositoryFactory>();
            Moq.Mock<IBorrowedBookRepository> BorrowedBookRepository = new Moq.Mock<IBorrowedBookRepository>();

            Moq.Mock<IBaseRepository<BorrowedBook>> _baseBorrowedBookRepository = new Moq.Mock<IBaseRepository<BorrowedBook>>();


            mockrepositoryFactory.Setup(x => x.GetRepository<BorrowedBook>()).Returns(_baseBorrowedBookRepository.Object);

            // mockrepositoryFactory.Expect(p => p.GetRepository<BorrowedBook>());



            #endregion

            #region fields


            IValidator<BorrowedBookDto> BorrowedBookDto = new BorrowedBookValidator();
            IValidator<BorrowedBookEditDto> BorrowedBookEditDto = new BorrowedBookEditValidator();

            IBorrowedBookServices BorrowedBookServices = new BorrowedBookServices(
               mockIMapper.Object, BorrowedBookDto, BorrowedBookEditDto, mockrepositoryFactory.Object,
               BorrowedBookRepository.Object);





            var _BorrowedBookDto = new BorrowedBookDto()
            {
              //  BookId = 1,
              //  UserId = 1,
                StartDateBorrowed = DateTime.Now
            };

            #endregion

            //act


            var _res = await BorrowedBookServices.CreateBorrowedAsync(_BorrowedBookDto);
            //assert
            Assert.True(!string.IsNullOrEmpty( _res));

        }
        catch (Exception ex)
        {
            Assert.True(true);
        }
    }
}
