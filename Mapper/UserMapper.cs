using SphereBlog.Dtos.User;
using SphereBlog.Models;

namespace SphereBlog.Mapper
{
    public static class UserMapper
    {
        public static UserDto? MapToDto(this User user)
        {
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
