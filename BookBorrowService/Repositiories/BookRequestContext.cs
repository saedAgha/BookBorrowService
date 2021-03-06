using BookBorrowService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookBorrowService.Repositiories
{
    public class BookRequestContext : DbContext
    {

        public BookRequestContext(DbContextOptions<BookRequestContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
 
        //for now we support only 1 library
        public DbSet<Library> Libraries { get; set; }

        public DbSet<BookRequest> BorrowBookRequests { get; set; }

    }

  


}
