using Application.Exceptions;
using Application.UseCases.CreateOrganization;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrganizationController(
    ILogger<OrganizationController> logger,
    ICreateOrganizationUseCase createOrganizationUseCase,
    IUnitOfWork unitOfWork
    ) : BaseController(logger)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICreateOrganizationUseCase _createOrganizationUseCase = createOrganizationUseCase;
    [HttpGet("{organizationId}")]
    public async Task<IActionResult> Get([FromRoute] int organizationId)
    {
        var organization = await _unitOfWork.Organization.GetByIdAsync(organizationId);
        return Ok(organization);
    }
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var userId = GetUserId();
        return Ok(await _createOrganizationUseCase.Execute(new() { UserId = userId }));
    }
}
