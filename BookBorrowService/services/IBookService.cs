using System.Threading.Tasks;
using BookBorrowService.Models;


namespace BookBorrowService.Services
{
    public interface IBookService
    {
        Task<Book> GetById(int id);
    }
}
