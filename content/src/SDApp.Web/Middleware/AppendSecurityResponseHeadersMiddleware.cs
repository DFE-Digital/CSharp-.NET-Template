namespace SDApp.Web.Middleware;

public class AppendSecurityResponseHeadersMiddleware(RequestDelegate next)
{
    public Task InvokeAsync(HttpContext context)
    {
        var response = context.Response;

        // Matches the CSP's frame-ancestors 'none' for browsers that don't support it
        response.Headers["X-Frame-Options"] = "DENY";
        response.Headers["X-Xss-Protection"] = "0";
        response.Headers["X-Content-Type-Options"] = "nosniff";
        response.Headers["X-Permitted-Cross-Domain-Policies"] = "none";
        response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        return next(context);
    }
}
