

using System.ComponentModel.DataAnnotations;
using BookBorrowService.Domain.Enums;

namespace BookBorrowService.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType Type { get; set; }
    }
}