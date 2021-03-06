using System;
using System.ComponentModel.DataAnnotations;

namespace BookBorrowService.Models
{
    public class BookRequest
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book book { get; set; }

        public int LibrarianId { get; set; }
        public User Librarian { get; set; }

        public int CustomerId { get; set; }
        public User Customer { get; set; }

        public bool IsReturned { get; set; }

        public DateTime BorrowDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
    }
}
