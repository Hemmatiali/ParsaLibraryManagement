using ParsaLibraryManagement.Domain.Entities;

namespace Test.ParsaLibraryManagement.Application.Publishers;

public class TestDataBorrowedBook
{
    
    public static Task<BorrowedBook> getTestData(int BorrowedBookid)
    {
        return Task.FromResult(
         new BorrowedBook()
         {
             Bid = BorrowedBookid,
             BookId =10,
             UserId = 10,
             StartDateBorrowed = DateTime.Now,

         });
    }


}
