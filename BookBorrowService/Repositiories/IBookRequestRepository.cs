using System.Threading.Tasks;
using BookBorrowService.Models;

namespace BookBorrowService.Repositiories
{
    public interface IBookRequestRepository
    {
        Task<User> GetUserById(int id);
        Task<Book> GetBookById(int id);
        Task<BookRequest> GetBorrowRequestById(int id);
        Task<BookRequest> GetUnReturnedBookRequest(int bookId, int customerId);
        Task<int> AddBorrowRequest(BookRequest bookRequest);
        Task UpdateBorrowRequest(BookRequest bookRequest);
    }
}
