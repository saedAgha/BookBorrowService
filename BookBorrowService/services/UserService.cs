using System.Threading.Tasks;
using BookBorrowService.Models;
using BookBorrowService.Repositiories;

namespace BookBorrowService.Services
{
    public class UserService : IUserService
    {
        private readonly IBookRequestRepository _bookRequestRepository;
        public UserService(IBookRequestRepository bookRequestRepository)
        {
            _bookRequestRepository = bookRequestRepository;
        }

        public  async Task<User> GetById(int id)
        {
            var user = await _bookRequestRepository.GetUserById(id);
            return user;
        }
    }
}
