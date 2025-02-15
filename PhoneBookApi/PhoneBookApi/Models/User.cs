using System.ComponentModel.DataAnnotations;

namespace PhoneBookApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, 
            ErrorMessage = "Name cannot be longer than 100 characters.")]
        [RegularExpression(@"^\S.*$", 
            ErrorMessage = "Name cannot be empty or just whitespace.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(100, 
            ErrorMessage = "Surname cannot be longer than 100 characters.")]
        [RegularExpression(@"^\S.*$", 
            ErrorMessage = "Surname cannot be empty or just whitespace.")]
        public required string Surname { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(16, MinimumLength = 16, 
            ErrorMessage = "Phone number must be exactly 16 characters long.")]
        [RegularExpression(@"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$", 
            ErrorMessage = "Phone number must match the pattern +7(XXX)XXX-XX-XX.")]
        public required string PhoneNumber { get; set; }
    }
}