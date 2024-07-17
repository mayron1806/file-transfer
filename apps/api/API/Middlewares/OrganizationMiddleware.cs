using System.Security.Claims;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace API.Middlewares;

public class OrganizationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    public async Task InvokeAsync(HttpContext context)
    {
        var allowAnonymous = context.GetEndpoint()?.Metadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
        if (allowAnonymous != null && allowAnonymous.Value) {
            Console.WriteLine("[ORGANIZATION MIDDLEWARE - ALLOW ANONYMOUS]");
            await _next(context);
            return;
        }
        Console.WriteLine("[ORGANIZATION MIDDLEWARE]");
        var organizationIdRoute = context.Request.RouteValues["organizationId"]?.ToString();
        if (!string.IsNullOrEmpty(organizationIdRoute)) {
            var logger = context.RequestServices.GetRequiredService<ILogger<OrganizationMiddleware>>();
            var userIdRoute = context.User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userIdRoute))
            {
                await WriteForbiddenResponse(context, "Usuário não tem autorizacão para essa organização");
                return;
            }
            if(!int.TryParse(userIdRoute, out var userId)) {
                await WriteForbiddenResponse(context, "Organização inválida");
                return;
            }
            if(!int.TryParse(organizationIdRoute, out var organizationId)) {
                await WriteForbiddenResponse(context, "Organização inválida");
                return;
            }
            // pega ou cria organização no cache
            var memoryCache = context.RequestServices.GetRequiredService<IMemoryCache>();
            var organization = await memoryCache.GetOrCreateAsync(
                "organization_" + organizationIdRoute + "_user_" + userIdRoute,
                async entry => {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
                    var uow = context.RequestServices.GetRequiredService<IUnitOfWork>();
                    var members = await uow.Member.GetFirstAsync(x => x.UserId == userId && x.OrganizationId == organizationId);
                    if (members == null) return null;
                    
                    var org = await uow.Organization.GetByIdAsync(organizationId);
                    if(org == null) return null;
                    
                    logger.LogInformation("[MISS] Organization: " + JsonConvert.SerializeObject(org));
                    return org;
                }
            );
            if (organization == null)
            {
                await WriteForbiddenResponse(context, "Usuário não tem autorizacão para essa organização");
                return;
            }
            context.Items["organizationId"] = organizationIdRoute;
        }
        Console.WriteLine("[ORGANIZATION MIDDLEWARE - NEXT]");
        await _next(context);
    }
    private static async Task WriteForbiddenResponse(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";
        var response = new { Message = message };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
}
