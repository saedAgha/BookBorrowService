using BookBorrowService.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using BookBorrowService.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookBorrowService.Repositiories
{
    public class BookRequestRepository : IBookRequestRepository
    {
        private readonly BookRequestContext _context;
        private readonly ILogger _logger;
        public BookRequestRepository(BookRequestContext context, ILogger<BookRequestRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> AddBorrowRequest(BookRequest newBookRequest)
        {
            try
            {
                newBookRequest.book.NumOFCopies--;
                 _context.Books.Update(newBookRequest.book);
                await _context.BorrowBookRequests.AddAsync(newBookRequest);
                await _context.SaveChangesAsync();
                return newBookRequest.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed To create new borrow request ", ex);
                throw;
            }
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        /// <summary>
        /// return unreturn book request , it should be only one request that is not returned , because 
        /// we prevent add book borrow if there's an existing one
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<BookRequest> GetUnReturnedBookRequest(int bookId,int customerId)
        {
            return await _context.BorrowBookRequests.Where(b => b.BookId.Equals(bookId) &&
                                                          b.CustomerId.Equals(customerId)&&b.IsReturned.Equals(false)).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<BookRequest> GetBorrowRequestById(int id)
        { 
            return await _context.BorrowBookRequests.FindAsync(id);
        }

        public async Task UpdateBorrowRequest(BookRequest updateBookRequest)
        {
            try
            {
                updateBookRequest.book.NumOFCopies++;
                _context.Books.Update(updateBookRequest.book);
                _context.Entry(updateBookRequest).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException duc)
            {
                var existingEntity =await  GetBorrowRequestById(updateBookRequest.Id);
                if (existingEntity == null)
                {
                    _logger.LogCritical($"Entity wasn't found during update due to {duc}");
                    throw new NotFoundException("Book Borrow Request Wasn't Found");
                }

                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogCritical($"Failed To Update the book request due to {ex}");
                throw;
            }
        }
    }
}
