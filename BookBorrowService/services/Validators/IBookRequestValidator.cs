using BookBorrowService.Models;

namespace BookBorrowService.Services.Validators
{
    public interface IBookRequestValidator
    {
        public void ValidateBook(Book book);

    }
}
