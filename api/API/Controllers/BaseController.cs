using System.Security.Claims;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public abstract class BaseController(ILogger logger) : ControllerBase
{
    protected readonly ILogger _logger = logger;
    protected int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.Name) ?? throw new HttpException(401, "Token invalido");
        return int.Parse(userIdClaim.Value);
    }
    protected int GetOrganizationId()
    {
        var item = HttpContext.Items["organizationId"];
        if (item == null) throw new HttpException(401, "Você não tem autorizacão para essa organização");
        if (!int.TryParse((string)item, out var id)) throw new HttpException(400, "Organização inválida");
        return id;
    }
}
