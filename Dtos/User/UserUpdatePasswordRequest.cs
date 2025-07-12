using System.ComponentModel.DataAnnotations;

namespace SphereBlog.Dtos.User
{
    public class UserUpdatePasswordRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer.")]
        public int Id { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Confirm Password must be at least 8 characters long.")]
        [Compare("Password", ErrorMessage = "Confirm Password does not match Password.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
