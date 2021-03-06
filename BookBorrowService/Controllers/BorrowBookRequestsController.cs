using System.Linq;
using System.Threading.Tasks;
using BookBorrowService.Controllers.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using BookBorrowService.Services;
using BookService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BookBorrowService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowBookRequestsController : ControllerBase
    {
        private readonly IBookRequestService _bookRequestService;
        private readonly ILogger _logger;

        public BorrowBookRequestsController(IBookRequestService bookRequestService, ILogger<BorrowBookRequestsController> logger)
        {
            _bookRequestService = bookRequestService;
            _logger = logger;
        }


        /// <summary>
        /// given book id , request Book borrower and liberrian id , 
        // validate request and create new borrow request 
        // Validation: validate cutomer ,employee,book info , check if customer already borrowed same book and didn't return yet
        /// </summary>
        /// <param name="id">book id</param>
        /// <param name="requestBook">json contain customer id , librarian id</param>
        /// <returns>status 201 created with the new borrow request id</returns>
        [HttpPost("book/{id}/borrow")]
        public async Task<IActionResult> CreateBorrowBookRequest(int id, [FromBody] RequestBook requestBook)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)).ToList();
                _logger.LogDebug("Invalid model state, returning 400 response");
                return BadRequest(new ErrorResponse(modelErrors));
            }
            var newEntityId = await _bookRequestService.BorrowBookRequest(id, requestBook);
            return StatusCode(StatusCodes.Status201Created, newEntityId);
        }

        [HttpPatch("book/{id}/return")]
        public async Task<ActionResult<RequestBook>> ReturnBorrowedBook(int id, [FromBody] RequestBook requestBook)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)).ToList();
                _logger.LogDebug("Invalid model state, returning 400 response");
                return BadRequest(new ErrorResponse(modelErrors));
            }
            await _bookRequestService.ReturnBookRequest(id, requestBook);
            return NoContent();
        }
    }
}
