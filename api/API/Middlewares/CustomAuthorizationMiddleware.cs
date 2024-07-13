namespace API.Middlewares;

public class CustomAuthorizationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("The AuthorizationPolicy named: 'Bearer' was not found"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var response = new { error = "Unauthorized", message = ex.Message };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
