using System.ComponentModel.DataAnnotations;

namespace BookService.Models
{
    public class RequestBook
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int LibrarianId { get; set; }
    }


}
