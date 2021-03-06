using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookBorrowService.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        /* EF Relations */
        public ICollection<Book> Books { get; set; }
    }
}