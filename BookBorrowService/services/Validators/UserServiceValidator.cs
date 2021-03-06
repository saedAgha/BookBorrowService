using System;
using BookBorrowService.Models;
using BookBorrowService.Domain.Enums;

namespace BookBorrowService.Services.Validators
{
    public class UserServiceValidator : IUserServiceValidator
    {
        public void ValidateId(int id)
        {
            if(id<1)
                throw new ArgumentException("Invalid Id value");
        }

        public void ValidateUser(User user, UserType userType)
        {
            if (user != null && user.Type.Equals(userType))
            {
                return;
            }

            else  {
                var userMessage = userType == UserType.Librarian ? "Librarian" : "Customer";
                throw new ArgumentException($"{userMessage} Doesn't Exist");
            }
        }
    }
}
