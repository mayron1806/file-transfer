using Domain;
using Domain.Plan;

namespace Application.Services.PlanService;

public interface IPlanService
{
    Task<Plan> GetPlanByOrganizationIdAsync(int organizationId);
    Plan GetPlanByOrganization(Organization org);
    Plan GetPlan(PlanName plan);
    Plan GetPlan(string plan);
}
