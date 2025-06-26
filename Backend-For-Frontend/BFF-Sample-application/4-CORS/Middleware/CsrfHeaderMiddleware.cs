namespace BFFDemo_1_StartProject.Middleware;

public class CsrfHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public CsrfHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant();

        // Check if request is for protected endpoints
        if (ShouldCheckCsrfHeader(path))
        {
            var csrfHeader = context.Request.Headers["X-CSRF"].FirstOrDefault();

            if (csrfHeader != "1")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Missing or invalid X-CSRF header");
                return;
            }
        }

        await _next(context);
    }

    private static bool ShouldCheckCsrfHeader(string? path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        return path.StartsWith("/bff/session") || path.StartsWith("/api/");
    }
}

public static class CsrfHeaderMiddlewareExtensions
{
    public static IApplicationBuilder CheckForCsrfHeader(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CsrfHeaderMiddleware>();
    }
}