using System;

namespace BookBorrowService.Exceptions
{
    public class NotFoundException:Exception
    {

        public NotFoundException(string message) : base(message)
        {
        }
    }
}
