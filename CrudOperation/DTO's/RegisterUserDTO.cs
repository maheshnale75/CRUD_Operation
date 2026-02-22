using System.ComponentModel.DataAnnotations;

namespace CrudOperation.DTO_s
{
    public class RegisterUserDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string UserName { get; set; }

        [StringLength(100, MinimumLength = 6,
        ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&]).{6,}$",
        ErrorMessage = "Password must contain uppercase, lowercase, number and special character.")]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100)]
        public string EmailId { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string Country { get; set; }
        [Required]
        public int RoleId { get; set; }
        public IFormFile? ProfileImage { get; set; }

    }
}
