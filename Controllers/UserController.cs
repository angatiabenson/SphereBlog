using Microsoft.AspNetCore.Mvc;
using SphereBlog.Data;

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
            return ApiSuccess(user);
        }
    }
}
