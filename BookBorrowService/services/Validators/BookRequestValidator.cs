using BookBorrowService.Models;
using BookBorrowService.Exceptions;

namespace BookBorrowService.Services.Validators
{
    public class BookRequestValidator : IBookRequestValidator
    {
        public void ValidateBook(Book book)
        {
            if (book == null)
            {
                throw new NotFoundException("Book Doesn't Exist");
            }
        }


    }
}
