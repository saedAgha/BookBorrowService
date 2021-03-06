using System.Threading.Tasks;
using BookBorrowService.Models;
using BookBorrowService.Repositiories;

namespace BookBorrowService.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRequestRepository _bookRequestRepository;
        public BookService(IBookRequestRepository bookRequestRepository)
        {
            _bookRequestRepository = bookRequestRepository;
        }

        public async  Task<Book> GetById(int id)
        {
            return await _bookRequestRepository.GetBookById(id);
        }
    }
    
}
