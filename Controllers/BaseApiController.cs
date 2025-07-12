using Microsoft.AspNetCore.Mvc;
using SphereBlog.Models;

namespace SphereBlog.Controllers
{ 
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult ApiSuccess<T>(T data, int statusCode = 200)
        {
            var response = new SuccessResponse<T>(data, statusCode);
            return StatusCode(statusCode, response);
        }

        protected IActionResult ApiSuccess(int statusCode = 200)
        {
            var response = new SuccessResponse(statusCode);
            return StatusCode(statusCode, response);
        }

        protected IActionResult ApiError(string message, int statusCode = 400)
        {
            var response = new ErrorResponse(message, statusCode);
            return StatusCode(statusCode, response);
        }

        protected IActionResult ApiNotFound(string message = "Resource not found")
        {
            return ApiError(message, 404);
        }

        protected IActionResult ApiUnauthorized(string message = "Authentication failed")
        {
            return ApiError(message, 401);
        }

        protected IActionResult ApiBadRequest(string message = "Bad request")
        {
            return ApiError(message, 400);
        }
    }
}