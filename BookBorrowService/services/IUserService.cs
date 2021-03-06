using System.Threading.Tasks;
using BookBorrowService.Models;


namespace BookBorrowService.Services
{
    public interface IUserService
    {
        Task<User> GetById(int id);
    }
}
