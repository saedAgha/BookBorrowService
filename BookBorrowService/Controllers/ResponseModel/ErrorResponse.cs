using System.Collections.Generic;

namespace BookBorrowService.Controllers.ResponseModel
{
    public class ErrorResponse
    {
        public List<string> Errors { get; set; } = new List<string>();

        public ErrorResponse(string error)
        {
            Errors.Add(error);
        }

        public ErrorResponse(List<string> errors)
        {
            Errors.AddRange(errors);
        }
    }
}
