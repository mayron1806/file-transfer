using Application.Exceptions;
using Application.Services.PlanService;
using Domain;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CreateOrganization;

public class CreateOrganizationUseCase(
    ILogger<CreateOrganizationUseCase> logger,
    IUnitOfWork unitOfWork,
    IPlanService planService
    ) : UseCase<CreateOrganizationInputDto, bool>(logger), ICreateOrganizationUseCase
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPlanService _planService = planService;

    public override async Task<bool> Execute(CreateOrganizationInputDto input)
    {
        var user = await _unitOfWork.User.GetByIdAsync(input.UserId, "Members");
        if (user == null) throw new HttpException(400, "Usuário nao encontrado");
        if(user.Members?.ToList().Count > 0) throw new HttpException(400, "Usuário ja possui uma organização");
        var organization = new Organization(plan: _planService.GetPlan(PlanName.Free).Name, planActive: true);
        organization.AddMember(user);
        _unitOfWork.Organization.Add(organization);

        if (await _unitOfWork.SaveChangesAsync() == 0) {
            throw new HttpException(500, "Ocorreu um erro ao tentar criar a organização, tente mais tarde");
        }
        return true;
    }
}
