using System;
using System.Threading.Tasks;
using BookBorrowService.Models;
using BookBorrowService.Domain.Enums;
using BookBorrowService.Repositiories;
using BookBorrowService.Services.Validators;
using BookService.Models;

namespace BookBorrowService.Services
{
    public class BookRequestService: IBookRequestService
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly IBookRequestValidator _bookRequestValidator;
        private readonly IUserServiceValidator _userServiceValidator;
        private readonly IBookRequestRepository _bookRequestRepository;

        public BookRequestService(IUserService userService,
            IBookService bookService, IBookRequestValidator bookRequestValidator,
            IUserServiceValidator userServiceValidator, IBookRequestRepository bookRequestRepository)
        {
            _userService = userService;
            _bookService = bookService;
            _bookRequestValidator = bookRequestValidator;
            _userServiceValidator = userServiceValidator;
            _bookRequestRepository = bookRequestRepository;
        }

        async Task<int> IBookRequestService.BorrowBookRequest(int bookId,RequestBook bookRequest)
        {

            var book = await _bookService.GetById(bookId);
            _bookRequestValidator.ValidateBook(book);

            var customer = await _userService.GetById(bookRequest.CustomerId);
            var employee = await _userService.GetById(bookRequest.LibrarianId);
            _userServiceValidator.ValidateUser(customer, UserType.Customer);
            _userServiceValidator.ValidateUser(employee, UserType.Librarian);

            if (book.NumOFCopies == 0)
            {
                throw new ArgumentException(
                    "No Books Left , Please Come Back Later");
            }

            var existRequest = await _bookRequestRepository.GetUnReturnedBookRequest(bookId, customer.UserId);
            if (existRequest!=null)
            {
                throw new ArgumentException(
                    "Customer Didn't returned the same book from before ,Can't allow borrow for same book twice");
            }


            var newRequest = new BookRequest
            {
                book = book,
                Librarian = employee,
                Customer = customer,
                BorrowDateTime = DateTime.Now
            };
            return await _bookRequestRepository.AddBorrowRequest(newRequest);
        }


        async Task IBookRequestService.ReturnBookRequest(int bookId, RequestBook bookRequest)
        {
            var book = await _bookService.GetById(bookId);
            _bookRequestValidator.ValidateBook(book);

            var customer = await _userService.GetById(bookRequest.CustomerId);
            var employee = await _userService.GetById(bookRequest.LibrarianId);

            _userServiceValidator.ValidateUser(customer ,UserType.Customer);
            _userServiceValidator.ValidateUser(employee, UserType.Librarian);

            var existRequest =  await _bookRequestRepository.GetUnReturnedBookRequest(bookId, customer.UserId);

            if (existRequest == null)
            {
                throw new ArgumentException(
                    "No such borrow recorded , Cannot allow return");
            }

            existRequest.IsReturned = true;
            existRequest.ReturnDateTime = DateTime.Now;

            await _bookRequestRepository.UpdateBorrowRequest(existRequest);
        }

    }
}
