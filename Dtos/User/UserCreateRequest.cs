using System.ComponentModel.DataAnnotations;

namespace SphereBlog.Dtos.User
{
    public class UserCreateRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Must be a vaid email")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
