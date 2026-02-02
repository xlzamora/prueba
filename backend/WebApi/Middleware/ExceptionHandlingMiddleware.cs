using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace TelemedicinaOdonto.WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Unhandled exception");
            var problem = new ProblemDetails
            {
                Title = "Ocurrió un error",
                Detail = ex.Message,
                Status = (int)HttpStatusCode.BadRequest
            };

            context.Response.StatusCode = problem.Status ?? (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
