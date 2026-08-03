using CricInfo.Domain.Entities;
using System.Net;
using System.Text.Json;

namespace CricInfo.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            
            await _next(context);
        }
        catch (Exception ex)
        {
           
            _logger.LogError(ex,
                "Unhandled Exception Occurred. RequestPath: {Path}, Method: {Method}",
                context.Request.Path,
                context.Request.Method);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,

            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

            ArgumentException => StatusCodes.Status400BadRequest,

            InvalidOperationException => StatusCodes.Status400BadRequest,
            NullReferenceException => StatusCodes.Status500InternalServerError,

            _ => StatusCodes.Status500InternalServerError
        };

        var response = new ErrorResponse
        {
            Success = false,
            StatusCode = context.Response.StatusCode,
            Message = exception switch
            {
                KeyNotFoundException =>
                    "Requested resource was not found.",

                UnauthorizedAccessException =>
                    "Unauthorized access.",

                ArgumentException =>
                    exception.Message,

                InvalidOperationException =>
                    exception.Message,
                NullReferenceException =>
            "A null reference occurred while processing the request.",

                _ =>
                    "An unexpected error occurred."
            },
            TraceId = context.TraceIdentifier
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}