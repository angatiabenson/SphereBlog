using System.Text.Json;
using SphereBlog.Models;

namespace SphereBlog.Middleware
{
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerOptions _jsonOptions;

        public ResponseWrapperMiddleware(RequestDelegate next)
        {
            _next = next;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body = originalBodyStream;
            responseBody.Seek(0, SeekOrigin.Begin);

            var responseText = await new StreamReader(responseBody).ReadToEndAsync();

            // Only wrap JSON responses from API controllers
            if (context.Request.Path.StartsWithSegments("/api") &&
                context.Response.ContentType?.Contains("application/json") == true)
            {
                var wrappedResponse = WrapResponse(responseText, context.Response.StatusCode);
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(wrappedResponse);
            }
            else
            {
                await context.Response.WriteAsync(responseText);
            }
        }

        private string WrapResponse(string responseText, int statusCode)
        {
            try
            {
                // Check if response is already wrapped by checking for our wrapper properties
                if (!string.IsNullOrEmpty(responseText))
                {
                    try
                    {
                        var existingResponse = JsonSerializer.Deserialize<JsonElement>(responseText);
                        if (existingResponse.TryGetProperty("status", out var statusProp) &&
                            existingResponse.TryGetProperty("code", out var codeProp))
                        {
                            // Response is already wrapped, return as-is
                            return responseText;
                        }
                    }
                    catch
                    {
                        // Continue with wrapping if parsing fails
                    }
                }

                if (statusCode >= 200 && statusCode < 300)
                {
                    // Success response
                    object? data = null;
                    if (!string.IsNullOrEmpty(responseText))
                    {
                        data = JsonSerializer.Deserialize<object>(responseText);
                    }

                    var successResponse = new SuccessResponse<object>(data, statusCode);
                    return JsonSerializer.Serialize(successResponse, _jsonOptions);
                }
                else
                {
                    // Error response
                    string message = GetErrorMessage(responseText, statusCode);

                    var errorResponse = new ErrorResponse(message, statusCode);
                    return JsonSerializer.Serialize(errorResponse, _jsonOptions);
                }
            }
            catch
            {
                // Fallback if something goes wrong
                object fallbackResponse = statusCode >= 200 && statusCode < 300
                    ? new SuccessResponse<object>(responseText, statusCode)
                    : new ErrorResponse(responseText ?? "An error occurred", statusCode);

                return JsonSerializer.Serialize(fallbackResponse, _jsonOptions);
            }
        }

        private string GetErrorMessage(string responseText, int statusCode)
        {
            string defaultMessage = statusCode switch
            {
                401 => "Authentication failed",
                403 => "Access forbidden",
                404 => "Resource not found",
                400 => "Bad request",
                422 => "Unprocessable entity",
                429 => "Too many requests",
                405 => "Method not allowed",
                408 => "Request timeout",
                409 => "Conflict",
                410 => "Gone",
                500 => "Internal server error",
                _ => "An error occurred"
            };

            if (string.IsNullOrEmpty(responseText))
            {
                return defaultMessage;
            }

            try
            {
                var errorObj = JsonSerializer.Deserialize<JsonElement>(responseText);

                // Handle string responses (like NotFound("message"))
                if (errorObj.ValueKind == JsonValueKind.String)
                {
                    return errorObj.GetString() ?? defaultMessage;
                }

                // Handle object responses
                if (errorObj.ValueKind == JsonValueKind.Object)
                {
                    // Try common error message properties
                    var messageProperties = new[] { "message", "error", "title", "detail" };

                    foreach (var prop in messageProperties)
                    {
                        if (errorObj.TryGetProperty(prop, out var messageProp) &&
                            messageProp.ValueKind == JsonValueKind.String)
                        {
                            var message = messageProp.GetString();
                            if (!string.IsNullOrEmpty(message))
                            {
                                return message;
                            }
                        }
                    }
                }

                // If we can't parse it, return the raw response as message
                return responseText.Length > 200 ? responseText.Substring(0, 200) + "..." : responseText;
            }
            catch
            {
                return responseText.Length > 200 ? responseText.Substring(0, 200) + "..." : responseText;
            }
        }
    }
}