using System.Text.Json.Serialization;

namespace SphereBlog.Models
{
    public abstract class BaseResponse
    {
        public string Status { get; set; } = "";
        public int Code { get; set; }
    }

    public class SuccessResponse<T> : BaseResponse
    {
        public T Data { get; set; }

        public SuccessResponse(T data, int code = 200)
        {
            Status = "success";
            Code = code;
            Data = data;
        }
    }

    public class SuccessResponse : BaseResponse
    {
        public SuccessResponse(int code = 200)
        {
            Status = "success";
            Code = code;
        }
    }

    public class ErrorResponse : BaseResponse
    {
        public string Message { get; set; }

        public ErrorResponse(string message, int code = 400)
        {
            Status = "error";
            Code = code;
            Message = message;
        }
    }

    // Keep the original ResponseWrapper for backward compatibility but update implementation
    public class ResponseWrapper<T>
    {
        public string Status { get; set; } = "success";
        public int Code { get; set; } = 200;
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static object Success(T data, int code = 200)
        {
            return new SuccessResponse<T>(data, code);
        }

        public static object Error(string message, int code = 400)
        {
            return new ErrorResponse(message, code);
        }
    }

    // For responses without data
    public class ResponseWrapper : ResponseWrapper<object>
    {
        public static object Success(int code = 200)
        {
            return new SuccessResponse(code);
        }

        public static object Error(string message, int code = 400)
        {
            return new ErrorResponse(message, code);
        }
    }
}