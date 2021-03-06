using System.ComponentModel.DataAnnotations;
using BookBorrowService.Domain.Enums;

namespace BookBorrowService.Models
{
    public class Library
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}