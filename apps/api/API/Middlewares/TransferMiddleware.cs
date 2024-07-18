using System.Security.Claims;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace API.Middlewares;

public class TransferMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    public async Task InvokeAsync(HttpContext context)
    {
        var allowAnonymous = context.GetEndpoint()!.Metadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
        if (allowAnonymous) {
            await _next(context);
            return;
        }
        Console.WriteLine("[TRANSFER MIDDLEWARE]");
        var transferIdRoute = context.Request.RouteValues["transferId"]?.ToString();
        if (!string.IsNullOrEmpty(transferIdRoute)) {
            if (!int.TryParse(transferIdRoute, out var transferId)) {
                await WriteForbiddenResponse(context, "Transferência inválida");
                return;
            }
            
            var userIdString = context.User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId)) {
                await WriteForbiddenResponse(context, "Usuário inválida");
                return;
            }

            var organizationIdString = context.Items["organizationId"]?.ToString();
            if (string.IsNullOrEmpty(organizationIdString) || !int.TryParse(organizationIdString, out var organizationId)) {
                await WriteForbiddenResponse(context, "Organização inválida");
                return;
            }
            // pega ou cria organização no cache
            var logger = context.RequestServices.GetRequiredService<ILogger<TransferMiddleware>>();
            var memoryCache = context.RequestServices.GetRequiredService<IMemoryCache>();
            var canAccessTransfer = await memoryCache.GetOrCreateAsync(
                "transfer_" + transferIdRoute + "_user_" + userIdString,
                async entry => {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
                    var uow = context.RequestServices.GetRequiredService<IUnitOfWork>();
                    var members = await uow.Member.GetFirstAsync(x => x.UserId == userId && x.OrganizationId == organizationId);
                    if (members == null) return false;

                    var transfer = await uow.Transfer.GetByIdAsync(transferId, "Organization");
                    if (transfer == null) return false;
                    if (transfer.OrganizationId != members.OrganizationId) return false;
                    
                    return true;
                }
            );
            if (!canAccessTransfer)
            {
                await WriteForbiddenResponse(context, "Usuário não tem autorizacão para essa transferência");
                return;
            }
        }
        await _next(context);
    }
    private static async Task WriteForbiddenResponse(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";
        var response = new { error = message };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
}
