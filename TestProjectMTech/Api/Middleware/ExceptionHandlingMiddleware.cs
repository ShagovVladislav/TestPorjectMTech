using System.Text.Json;
using TestProjectMTech.Api.Exceptions;

namespace TestProjectMTech.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (AppException exception)
        {
            await WriteErrorResponse(context, exception.StatusCode, exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception");

            await WriteErrorResponse(
                context,
                StatusCodes.Status500InternalServerError,
                "Internal server error");
        }
    }

    private static async Task WriteErrorResponse(
        HttpContext context,
        int statusCode,
        string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            statusCode,
            message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}