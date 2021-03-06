using System;
using System.Threading.Tasks;
using BookBorrowService.Models;
using BookBorrowService.Domain.Enums;
using BookBorrowService.Repositiories;
using BookBorrowService.Services;
using BookBorrowService.Services.Validators;
using BookService.Models;
using Moq;
using Xunit;


namespace BooksBorrowService.UnitTests
{
    public class BookRequestServiceTests
    {


        private IBookRequestService _bookRequestService;


        private Mock<IBookService> _bookServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IBookRequestValidator> _bookRequestValidatorMock;
        private Mock<IUserServiceValidator> _userServiceValidatorMock;
        private Mock<IBookRequestRepository> _bookServiceRepositoryWork;

        public BookRequestServiceTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _userServiceMock = new Mock<IUserService>();
            _bookRequestValidatorMock = new Mock<IBookRequestValidator>();
            _userServiceValidatorMock = new Mock<IUserServiceValidator>();
            _bookServiceRepositoryWork = new Mock<IBookRequestRepository>();


            _bookRequestService = new BookRequestService(_userServiceMock.Object, _bookServiceMock.Object,
                _bookRequestValidatorMock.Object, _userServiceValidatorMock.Object, _bookServiceRepositoryWork.Object);
        }



        [Fact]
        public async Task BorrowBookRequest_ValidateZeroNumberOfCopies()
        {
            var book = new Book
            {
                BookId = 1,
                NumOFCopies = 0
            };
            var customer = new User
            {
                UserId = 1,
                Type = UserType.Customer
            };
            var librarian = new User
            {
                UserId = 2,
                Type = UserType.Librarian
            };

            _bookServiceMock.Setup(c => c.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => book);
            _userServiceMock.Setup(c => c.GetById(1))
                .ReturnsAsync(() => customer);
            _userServiceMock.Setup(c => c.GetById(2))
                .ReturnsAsync(() => librarian);
            _bookRequestValidatorMock.Setup(c => c.ValidateBook(It.IsAny<Book>())).Verifiable();
            _userServiceValidatorMock.Setup(c => c.ValidateUser(It.IsAny<User>(), It.IsAny<UserType>()));


            _bookRequestService.BorrowBookRequest(1, new RequestBook {LibrarianId = 2, CustomerId = 1});

            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _bookRequestService.BorrowBookRequest(1, new RequestBook {LibrarianId = 2, CustomerId = 1}));
            Assert.Equal("No Books Left , Please Come Back Later", exception.Message);
        }

        [Fact]
        public async Task BookRequestValidateUserCantSameUnReturnedBook()
        {
            var book = new Book
            {
                BookId = 1,
                NumOFCopies = 0
            };
            var customer = new User
            {
                UserId = 1,
                Type = UserType.Customer
            };
            var librarian = new User
            {
                UserId = 2,
                Type = UserType.Librarian
            };

            var dummyBookBorrowRequest = new BookRequest();

            _bookServiceMock.Setup(c => c.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => book);
            _userServiceMock.Setup(c => c.GetById(1))
                .ReturnsAsync(() => customer);
            _userServiceMock.Setup(c => c.GetById(2))
                .ReturnsAsync(() => librarian);
            _bookRequestValidatorMock.Setup(c => c.ValidateBook(It.IsAny<Book>())).Verifiable();
            _userServiceValidatorMock.Setup(c => c.ValidateUser(It.IsAny<User>(), It.IsAny<UserType>()));


            _bookServiceRepositoryWork.Setup(c => c.GetUnReturnedBookRequest(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => dummyBookBorrowRequest);
            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _bookRequestService.BorrowBookRequest(1, new RequestBook {LibrarianId = 2, CustomerId = 1}));
            Assert.Equal("Customer Didn't returned the same book from before ,Can't allow borrow for same book twice",
                exception.Message);
        }

        [Fact]
        public async Task BookRequestSuccess()
        {
            var book = new Book
            {
                BookId = 1,
                NumOFCopies = 0
            };
            var customer = new User
            {
                UserId = 1,
                Type = UserType.Customer
            };
            var librarian = new User
            {
                UserId = 2,
                Type = UserType.Librarian
            };

            var dummyBookBorrowRequest = new BookRequest();

            _bookServiceMock.Setup(c => c.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => book);
            _userServiceMock.Setup(c => c.GetById(1))
                .ReturnsAsync(() => customer);
            _userServiceMock.Setup(c => c.GetById(2))
                .ReturnsAsync(() => librarian);
            _bookRequestValidatorMock.Setup(c => c.ValidateBook(It.IsAny<Book>())).Verifiable();
            _userServiceValidatorMock.Setup(c => c.ValidateUser(It.IsAny<User>(), It.IsAny<UserType>()));


            _bookServiceRepositoryWork.Setup(c => c.GetUnReturnedBookRequest(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

            _bookServiceRepositoryWork.Setup(c => c.AddBorrowRequest(It.IsAny<BookRequest>())).ReturnsAsync(1);

            var id = await _bookRequestService.BorrowBookRequest(1, new RequestBook {LibrarianId = 2, CustomerId = 1});
 
            Assert.Equal(1, id);

        }

        [Fact]
        public async Task ReturnBookRequest_Validate_UserCantBorrowSameUnReturnedBook()
        {
            var book = new Book
            {
                BookId = 1,
                NumOFCopies = 0
            };
            var customer = new User
            {
                UserId = 1,
                Type = UserType.Customer
            };
            var librarian = new User
            {
                UserId = 2,
                Type = UserType.Librarian
            };


            _bookServiceMock.Setup(c => c.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => book);
            _userServiceMock.Setup(c => c.GetById(1))
                .ReturnsAsync(() => customer);
            _userServiceMock.Setup(c => c.GetById(2))
                .ReturnsAsync(() => librarian);
            _bookRequestValidatorMock.Setup(c => c.ValidateBook(It.IsAny<Book>())).Verifiable();
            _userServiceValidatorMock.Setup(c => c.ValidateUser(It.IsAny<User>(), It.IsAny<UserType>()));


            _bookServiceRepositoryWork.Setup(c => c.GetUnReturnedBookRequest(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);
            
            // Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _bookRequestService.ReturnBookRequest(1, new RequestBook {LibrarianId = 2, CustomerId = 1}));
            Assert.Equal("No such borrow recorded , Cannot allow return", exception.Message);
        }

        [Fact]
        public async Task ReturnBookRequest_Success()
        {
            var book = new Book
            {
                BookId = 1,
                NumOFCopies = 0
            };
            var customer = new User
            {
                UserId = 1,
                Type = UserType.Customer
            };
            var librarian = new User
            {
                UserId = 2,
                Type = UserType.Librarian
            };


            _bookServiceMock.Setup(c => c.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => book);
            _userServiceMock.Setup(c => c.GetById(1))
                .ReturnsAsync(() => customer);
            _userServiceMock.Setup(c => c.GetById(2))
                .ReturnsAsync(() => librarian);
            _bookRequestValidatorMock.Setup(c => c.ValidateBook(It.IsAny<Book>())).Verifiable();
            _userServiceValidatorMock.Setup(c => c.ValidateUser(It.IsAny<User>(), It.IsAny<UserType>()));


            _bookServiceRepositoryWork.Setup(c => c.GetUnReturnedBookRequest(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

              await _bookRequestService.ReturnBookRequest(1, new RequestBook {LibrarianId = 2, CustomerId = 1});

            // Act
            /*
             *
             * need to verfiy it ends successfully
             */
        }

    }
}
