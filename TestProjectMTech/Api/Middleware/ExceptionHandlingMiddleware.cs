using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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

    public async Task InvokeAsync(HttpContext context, IProblemDetailsService problemDetailsService)
    {
        try
        {
            await _next(context);
        }
        catch (AppException exception)
        {
            await WriteProblemDetails(
                context,
                problemDetailsService,
                exception.StatusCode,
                exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception");

            await WriteProblemDetails(
                context,
                problemDetailsService,
                StatusCodes.Status500InternalServerError,
                "Internal server error");
        }
    }

    private static async Task WriteProblemDetails(HttpContext context,
        IProblemDetailsService problemDetailsService,
        int statusCode,
        string detail)
    {
        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Type = "about:blank",
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Status = statusCode,
            Detail = detail,
            Instance = context.Request.Path
        };

        await problemDetailsService.WriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails = problemDetails
            });
    }
}