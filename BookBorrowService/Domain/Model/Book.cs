using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookBorrowService.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }
        public int NumOFCopies { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        /* EF Relations */
        public ICollection<Author> Authors { get; set; }

    }
}