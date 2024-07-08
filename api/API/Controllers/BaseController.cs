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
}
