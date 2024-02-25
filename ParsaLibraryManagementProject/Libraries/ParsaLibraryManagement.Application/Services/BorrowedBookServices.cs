using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Interfaces;
using ParsaLibraryManagement.Application.Interfaces.ImageServices;
using ParsaLibraryManagement.Application.Utilities;
using ParsaLibraryManagement.Domain.Common;
using ParsaLibraryManagement.Domain.Common.Extensions;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Domain.Interfaces.ImageServices;
using ParsaLibraryManagement.Domain.Interfaces.Repository;
using ParsaLibraryManagement.Domain.Models;
using System.Linq.Expressions;

namespace ParsaLibraryManagement.Application.Services
{
    ///<inheritdoc cref="IBorrowedBookServices"/>
    public class BorrowedBookServices : IBorrowedBookServices
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IValidator<BorrowedBookEditDto> _validatoredit;
        private readonly IValidator<BorrowedBookDto> _validator;

        private readonly IBaseRepository<BorrowedBook> _baseRepository;
        private readonly IBorrowedBookRepository _borrowedbookRepository;


        #endregion

        #region Ctor

        public BorrowedBookServices(
                                          IMapper mapper
                                        , IValidator<BorrowedBookDto> validator
                                        , IValidator<BorrowedBookEditDto> validatoredit

                                        , IRepositoryFactory repositoryFactory,
                                        IBorrowedBookRepository borrowedbookRepository
                                     )
        {
            _mapper = mapper;
            _validator = validator;
            _validatoredit = validatoredit;

            _baseRepository = repositoryFactory.GetRepository<BorrowedBook>();
            _borrowedbookRepository = borrowedbookRepository;
           
        }

        #endregion

        #region Methods

        #region Private

              #endregion

        #region Retrieval

        /// <inheritdoc />
        public async Task<BorrowedBookDto?> GetBorrowedByIdAsync(int BorrowedBookId)
        {
            try
            {
                // Get Borrowed
                var booksBorrowed = await _baseRepository.GetByIdAsync(BorrowedBookId);

                // Map entity to DTO and return
                return booksBorrowed == null ? null : _mapper.Map<BorrowedBookDto>(booksBorrowed);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets all book Borrowed asynchronously.
        /// </summary>
        /// <param name="BookId">Book Id</param>
        /// <param name="UserId">User Id</param>
        /// <returns>A task representing the asynchronous operation, yielding a list of <see cref="BorrowedBookDto"/>.</returns>
        public async Task<List<BorrowedBookDto>> GetAllBorrowedByBookandUserAsync(int BookId, int UserId)
        {
            try
            {
                // Retrieve all Borrowed
                var booksBorrowed = await _baseRepository.GetAllAsync(d=> (UserId==0 || d.UserId== UserId) &&
                (BookId == 0 || d.BookId == BookId)
                );

                // Map Borrowed to DTOs and return the list
                return booksBorrowed.Select(BorrowedBook => _mapper.Map<BorrowedBookDto>(BorrowedBook)).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<List<BorrowedBookDto>> GetAllBorrowedAsync()
        {
            try
            {
                // Retrieve all Borrowed
                var booksBorrowed = await _baseRepository.GetAllAsync();

                // Map Borrowed to DTOs and return the list
                return booksBorrowed.Select(BorrowedBook => _mapper.Map<BorrowedBookDto>(BorrowedBook)).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }

       
        /// <inheritdoc />
        public async Task<BorrowedBookDto> GetBorrowedForEditAsync(int BorrowedId)
        {
            try
            {
                
                var _list =await _baseRepository.GetByIdAsync(BorrowedId);

                // Return mapped Borrowed
                return _mapper.Map<BorrowedBookDto>(_list);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region Modification

        /// <inheritdoc />
        public async Task<string?> CreateBorrowedAsync(BorrowedBookDto BorrowedBookDto)
        {
          
            try
            {

                // Check existence of title
                var Exists = 
                    await _baseRepository.AnyAsync(p => 
                                                       p.BookId == BorrowedBookDto.BookId  
                                                    && p.UserId== BorrowedBookDto.UserId
                                                    && !p.BackEndDate.HasValue

                                                   );
                if (Exists)
                    return string.Format(ErrorMessages.Exist, nameof(BorrowedBookDto.BookId));

                BorrowedBookDto.StartDateBorrowed = DateTime.Now;
                var validationResult = await _validator.ValidateAsync(BorrowedBookDto);
                if (!validationResult.IsValid)
                    return string.Format(ValidationHelper.GetErrorMessages(validationResult));

                              // Map DTO to entity and save
                var BorrowedBook = _mapper.Map<BorrowedBook>(BorrowedBookDto);

            
                await _baseRepository.AddAsync(BorrowedBook);
                await _baseRepository.SaveChangesAsync();

                // Done
                return null;
            }
            catch (Exception e)
            {
               
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<OperationResultModel> UpdateBorrowedAsync(BorrowedBookEditDto BorrowedBookDto)
        {
        
            try
            {
                // Get existing Borrowed
                var existingBorrowed = await _baseRepository.GetByIdAsync(BorrowedBookDto.Bid);
                if (existingBorrowed == null)
                    return new OperationResultModel { WasSuccess = false, Message = ErrorMessages.ItemNotFoundMsg };


                var validationResult = await _validatoredit.ValidateAsync(BorrowedBookDto);
                if (!validationResult.IsValid)
                    return new OperationResultModel { WasSuccess = false, Message = ValidationHelper.GetErrorMessages(validationResult) }; //TODO image is uploaded and should be handled

                //// Check existence of title
                //var Exists = await _baseRepository.AnyAsync(p => 
                //       p.UserId== BorrowedBookDto.UserId
                //   &&  p.BookId == BorrowedBookDto.BookId
                //   && !p.BackEndDate.HasValue
                //&& p.Bid != BorrowedBookDto.BorrowedId);
                //if (Exists)
                //    return new OperationResultModel { WasSuccess = false, Message = string.Format(ErrorMessages.Exist, nameof(BorrowedBookDto.BookId)) };

                // Map DTO to existing entity and save
                _mapper.Map(BorrowedBookDto, existingBorrowed);

                existingBorrowed.BackEndDate = DateTime.Now;
                await _baseRepository.UpdateAsync(existingBorrowed);
                await _baseRepository.SaveChangesAsync();

                // Done
                return new OperationResultModel { WasSuccess = true };
            }
            catch (Exception e)
            {
               
                throw;
            }
        }

        #endregion

        #region Deletion

        /// <inheritdoc />
        public async Task<string?> DeleteBorrowedAsync(short BorrowedId, string folderName)
        {
            try
            {
                              // Get existing Borrowed
                var existingBorrowed = await _baseRepository.GetByIdAsync(BorrowedId);
                if (existingBorrowed == null)
                    return ErrorMessages.ItemNotFoundMsg;

                // Delete the Borrowed
                await _baseRepository.RemoveAsync(existingBorrowed);
                await _baseRepository.SaveChangesAsync();

                // Done
                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }


        #endregion

        #endregion
    }
}
