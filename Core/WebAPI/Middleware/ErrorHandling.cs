using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Humteria.Middleware;

public class ErrorHandling
{
    private readonly ILogger<ErrorHandling> logger;
    private readonly RequestDelegate next;
    public ErrorHandling(ILogger<ErrorHandling> logger, RequestDelegate next)
    {
        this.logger = logger;
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var start = DateTime.UtcNow;
        try
        {
            Console.WriteLine("1");
            await next.Invoke(context);
            Console.WriteLine("3");
#pragma warning disable CA2254 // Template should be a static expression
            logger.LogInformation($"Request {context.Request.Path} took: {(DateTime.UtcNow - start).TotalMilliseconds}ms"); // replace with log to file system later
        }
        catch (Exception e)
        {
            ProblemDetails problem = new()
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "Server error",
                Title = "You broke it!",
                Detail = "An internal server error has occurred!"
            };
            string json = JsonSerializer.Serialize(problem);

            logger.LogError(e, e.Message); // replace with log to file system later
#pragma warning restore CA2254 // Template should be a static expression

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(json);
        }
    }
}

public static class ErrorHandlingExtensions
{
    public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ErrorHandling>();
    }
}