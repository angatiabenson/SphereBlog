using Microsoft.AspNetCore.Mvc;
using SphereBlog.Data;
using SphereBlog.Dtos.User;
using SphereBlog.Mapper;

namespace SphereBlog.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly AppDBContext _context;
        public UserController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetUserProfile(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return ApiNotFound($"User with ID {id} not found");
            }
            return ApiSuccess(user.MapToDto());
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserCreateRequest userRequest)
        {
            if (userRequest == null)
            {
                return ApiBadRequest("Invalid user data");
            }
            var user = new Models.User
            {
                Name = userRequest.Name,
                Email = userRequest.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password),
                Role = "user"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return ApiSuccess(user.MapToDto(), statusCode: 201);
        }

        [HttpPut("update")]
        public IActionResult UpdateUser([FromBody] UserUpdateRequest userRequest)
        {
            if (userRequest == null)
            {
                return ApiBadRequest("Invalid user data");
            }
            var user = _context.Users.Find(userRequest.Id);
            if (user == null)
            {
                return ApiNotFound($"User with ID {userRequest.Id} not found");
            }
            user.Name = userRequest.Name;
            user.Email = userRequest.Email;

            // Save changes to the database
            _context.Users.Update(user);
            _context.SaveChanges();
            return ApiSuccess(user.MapToDto());
        }


        [HttpPut("update-password")]
        public IActionResult UpdateUserPassword([FromBody] UserUpdatePasswordRequest passwordRequest)
        {
            if (passwordRequest == null)
            {
                return ApiBadRequest("Invalid password data");
            }
            var user = _context.Users.Find(passwordRequest.Id);
            if (user == null)
            {
                return ApiNotFound($"User with ID {passwordRequest.Id} not found");
            }
            if (!BCrypt.Net.BCrypt.Verify(passwordRequest.OldPassword, user.Password))
            {
                return ApiBadRequest("Old password is incorrect");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(passwordRequest.Password);
            // Save changes to the database
            _context.Users.Update(user);
            _context.SaveChanges();
            return ApiSuccess("Password updated successfully");
        }
    }
}
