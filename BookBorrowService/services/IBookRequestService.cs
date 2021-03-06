using System.Threading.Tasks;
using BookService.Models;

namespace BookBorrowService.Services
{
    public interface IBookRequestService
    { 
        Task<int> BorrowBookRequest(int bookId, RequestBook bookRequest);
        Task ReturnBookRequest(int bookId,RequestBook bookRequest);
    }
}
