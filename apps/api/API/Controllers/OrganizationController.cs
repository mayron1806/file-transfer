using API.Dto;
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
    public async Task<IActionResult> GetById([FromRoute] int organizationId)
    {
        var organization = await _unitOfWork.Organization.GetByIdAsync(organizationId);
        if (organization == null) throw new HttpException(404, "Organizacao não encontrada");
        return Ok(GetOrganizationByIdDto.Map(organization));
    }

    [HttpGet]
    public async Task<IActionResult> GetByUser()
    {
        var userId = GetUserId();
        var organization = await _unitOfWork.Organization.GetFirstAsync(x => x.Members != null && x.Members.Any(y => y.UserId == userId), "Members");
        if (organization == null) throw new HttpException(404, "Organização não encontrada");
        return Ok(GetOrganizationByIdDto.Map(organization));
    }
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var userId = GetUserId();
        var data = await _createOrganizationUseCase.Execute(new() { UserId = userId });
        return Ok(data);
    }
}
