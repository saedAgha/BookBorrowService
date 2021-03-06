using System;
using System.Threading.Tasks;
using BookBorrowService.Exceptions;
using BookBorrowService.Models;
using BookBorrowService.Domain.Enums;
using BookBorrowService.Repositiories;
using BookBorrowService.Services;
using BookBorrowService.Services.Validators;
using Moq;
using Xunit;


namespace BooksBorrowService.UnitTests
{
    public class ValidatorsTests
    {

        private IBookService _bookService;
        private IUserService _userService;
        private IBookRequestService _bookRequestService;
        private readonly IBookRequestValidator _bookRequestValidator;
        private readonly IUserServiceValidator _userServiceValidator;

        private Mock<IBookRequestRepository> _bookServiceRepositoryWork;

        public ValidatorsTests()
        {
            _bookServiceRepositoryWork = new Mock<IBookRequestRepository>();
            _bookService = new BookBorrowService.Services.BookService(_bookServiceRepositoryWork.Object);
            _userService = new UserService(_bookServiceRepositoryWork.Object);
            _bookRequestValidator = new BookRequestValidator();
            _userServiceValidator = new UserServiceValidator();

            _bookRequestService = new BookRequestService(_userService, _bookService,
                _bookRequestValidator, _userServiceValidator, _bookServiceRepositoryWork.Object);
        }



        [Fact]
        public async Task BookService_ValidateGetById()
        {
            var book = new Book
            {
                BookId = 1
            };
            _bookServiceRepositoryWork.Setup(c => c.GetBookById(It.IsAny<int>()))
                .ReturnsAsync(() => book);

            // Act
            var actual = await _bookService.GetById(1);
            Assert.Equal(book, actual);
        }

        [Fact]
        public async Task UserService_ValidateGetById()
        {
            var user = new User
            {
                UserId = 1
            };
            _bookServiceRepositoryWork.Setup(c => c.GetUserById(It.IsAny<int>()))
                .ReturnsAsync(() => user);

            // Act
            var actual = await _userService.GetById(1);
            Assert.Equal(user, actual);
        }

 

        [Fact]
        public Task BookRequestValidator_BookIsNull()
        {
            var exception = Assert.Throws<NotFoundException>(() => _bookRequestValidator.ValidateBook(null));
            Assert.Equal("Book Doesn't Exist", exception.Message);
            return Task.CompletedTask;
        }

        [Fact]
        public Task UserServiceValidator_UserIsNull()
        {
            var exception =
                Assert.Throws<ArgumentException>(() => _userServiceValidator.ValidateUser(null, UserType.Customer));
            Assert.Equal("Customer Doesn't Exist", exception.Message);
            return Task.CompletedTask;
        }

        [Fact]
        public Task UserServiceValidator_LibrarianIsNull()
        {
            var exception =
                Assert.Throws<ArgumentException>(() => _userServiceValidator.ValidateUser(null, UserType.Librarian));
            Assert.Equal("Librarian Doesn't Exist", exception.Message);
            return Task.CompletedTask;
        }


    }
}
