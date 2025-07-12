using System.ComponentModel.DataAnnotations;

namespace SphereBlog.Dtos.User
{
    public class UserUpdateRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer.")]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Must be a vaid email")]
        public string Email { get; set; } = string.Empty;
        }
}
