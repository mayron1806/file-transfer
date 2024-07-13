using Application.Exceptions;
using Domain;
using Domain.Plan;
using Infrastructure.UnitOfWork;

namespace Application.Services.PlanService;

public enum PlanName {
    Free,
    Starter,
    Pro,
    ProMax
}
public class PlanService(IUnitOfWork unitOfWork): IPlanService
{
    private readonly List<Plan> plans = [Plans.Free, Plans.Starter, Plans.Pro, Plans.ProMax];
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Plan> GetPlanByOrganizationIdAsync(int organizationId)
    {
        var org = await _unitOfWork.Organization.GetByIdAsync(organizationId);
        if (org == null) throw new HttpException(400, "Organizacao nao encontrada");
        return GetPlanByOrganization(org);
    }
    public Plan GetPlanByOrganization(Organization org) => plans.Find(x => x.Name == org.Plan) ?? Plans.Free;
    public Plan GetPlan(PlanName plan) {
        return plan switch
        {
            PlanName.Free => Plans.Free,
            PlanName.Starter => Plans.Starter,
            PlanName.Pro => Plans.Pro,
            PlanName.ProMax => Plans.ProMax,
            _ => throw new HttpException(400, "Plano invalido"),
        };
    }
    public Plan GetPlan(string plan) {
        return plan switch
        {
            "Free" => Plans.Free,
            "Starter" => Plans.Starter,
            "Pro" => Plans.Pro,
            "ProMax" => Plans.ProMax,
            _ => throw new HttpException(400, "Plano invalido"),
        };
    }
}
