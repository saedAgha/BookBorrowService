using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookBorrowService.Models;
using BookBorrowService.Domain.Enums;
using BookBorrowService.Repositiories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;


namespace BooksBorrowService.UnitTests
{
    public class BookRequestRepositoryTests : TestBase
    {
 
        private IBookRequestRepository _bookRequestRepository;
        private Mock<ILogger<BookRequestRepository>> _loggerMock;

        public BookRequestRepositoryTests()
        {
            _loggerMock = new Mock<ILogger<BookRequestRepository>>();
        }

        [Fact]
        public async Task GetBookById_Success()
        {
            using var dbContext = GetDbContext();

            var bookScience = new Book {BookId = 1, NumOFCopies = 1, Title = "Science"};
            var authorJhon = new Author
            {
                AuthorId = 1,
                FirstName = "Jhon",
                LastName = "Zeus",
                Books = new List<Book> { bookScience }
            };
            var authorMark = new Author
            {
                AuthorId = 2,
                FirstName = "Mark",
                LastName = "Zeus",
                Books = new List<Book> { bookScience}
            };
            bookScience.Authors = new List<Author> { authorJhon, authorMark };
            dbContext.Books.Add(bookScience);
            await dbContext.SaveChangesAsync();

            _bookRequestRepository = new BookRequestRepository(dbContext, _loggerMock.Object);

            var book = await _bookRequestRepository.GetBookById(1);
            Assert.NotNull(book);
            Assert.Equal(2, book.Authors.Count);
            Assert.Equal(1, book.NumOFCopies);
            Assert.Equal(1, book.Authors.First().Books.Count);
        }


        [Fact]
        public async Task GetBookById_Fail()
        {
            using var dbContext = GetDbContext();

            var bookScience = new Book { BookId = 1, NumOFCopies = 1, Title = "Science" };
            var authorJhon = new Author
            {
                AuthorId = 1,
                FirstName = "Jhon",
                LastName = "Zeus",
                Books = new List<Book> { bookScience }
            };
            var authorMark = new Author
            {
                AuthorId = 2,
                FirstName = "Mark",
                LastName = "Zeus",
                Books = new List<Book> { bookScience }
            };
            bookScience.Authors = new List<Author> { authorJhon, authorMark };
            dbContext.Books.Add(bookScience);
            await dbContext.SaveChangesAsync();

            _bookRequestRepository = new BookRequestRepository(dbContext, _loggerMock.Object);

            var book = await _bookRequestRepository.GetBookById(4);
            Assert.Null(book);
        }

        [Fact]
        public async Task GetUserById_Customer_Success()
        {
            using var dbContext = GetDbContext();

            dbContext.Users.Add(new User { UserId = 1, Type = UserType.Customer });
            await dbContext.SaveChangesAsync();

            _bookRequestRepository = new BookRequestRepository(dbContext, _loggerMock.Object);

            var user = await _bookRequestRepository.GetUserById(1);
            Assert.NotNull(user);
            Assert.Equal(UserType.Customer, user.Type);
        }


        [Fact]
        public async Task GetUserById_Librarian_Success()
        {
            using var dbContext = GetDbContext();

            dbContext.Users.Add(new User { UserId = 6, Type = UserType.Librarian });

            await dbContext.SaveChangesAsync();

            _bookRequestRepository = new BookRequestRepository(dbContext, _loggerMock.Object);

            var user = await _bookRequestRepository.GetUserById(6);
            Assert.NotNull(user);
            Assert.Equal(UserType.Librarian, user.Type);
        }

        [Fact]
        public async Task GetUserById_NotExist()
        {
            using var dbContext = GetDbContext();

            dbContext.Users.Add(new User { UserId = 6, Type = UserType.Librarian });

            await dbContext.SaveChangesAsync();

            _bookRequestRepository = new BookRequestRepository(dbContext, _loggerMock.Object);

            var user = await _bookRequestRepository.GetUserById(2);
            Assert.Null(user);
        }
        //TODO : continue test for the rest of the functions


        

    }
}
