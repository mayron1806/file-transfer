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
    public static readonly List<Plan> Plans = [Domain.Plan.Plans.Free, Domain.Plan.Plans.Starter, Domain.Plan.Plans.Pro, Domain.Plan.Plans.ProMax];
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Plan> GetPlanByOrganizationIdAsync(int organizationId)
    {
        var org = await _unitOfWork.Organization.GetByIdAsync(organizationId);
        if (org == null) throw new HttpException(400, "Organizacao nao encontrada");
        return GetPlanByOrganization(org);
    }
    public Plan GetPlanByOrganization(Organization org) => Plans.Find(x => x.Name == org.Plan) ?? Domain.Plan.Plans.Free;
    public Plan GetPlan(PlanName plan) {
        return plan switch
        {
            PlanName.Free => Domain.Plan.Plans.Free,
            PlanName.Starter => Domain.Plan.Plans.Starter,
            PlanName.Pro => Domain.Plan.Plans.Pro,
            PlanName.ProMax => Domain.Plan.Plans.ProMax,
            _ => throw new HttpException(400, "Plano invalido"),
        };
    }
    public Plan GetPlan(string plan) {
        return plan switch
        {
            "Free" => Domain.Plan.Plans.Free,
            "Starter" => Domain.Plan.Plans.Starter,
            "Pro" => Domain.Plan.Plans.Pro,
            "ProMax" => Domain.Plan.Plans.ProMax,
            _ => throw new HttpException(400, "Plano invalido"),
        };
    }
}
