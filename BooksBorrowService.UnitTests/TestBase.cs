using System;
using BookBorrowService.Repositiories;
using Microsoft.EntityFrameworkCore;

namespace BooksBorrowService.UnitTests
{
    public abstract class TestBase
    {
        protected BookRequestContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<BookRequestContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new BookRequestContext(options);
        }
    }
}
