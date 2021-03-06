using BookBorrowService.Models;
using BookBorrowService.Domain.Enums;

namespace BookBorrowService.Services.Validators
{
    public interface IUserServiceValidator
    {
        public void ValidateUser(User user, UserType userType);
    }
}
