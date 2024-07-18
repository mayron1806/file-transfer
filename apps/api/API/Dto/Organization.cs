using Application.Exceptions;
using Application.Services.PlanService;
using Domain;
using Domain.Plan;
namespace API.Dto;

public class GetOrganizationByIdDto 
{
    public int Id { get; set; }
    public Plan? Plan { get; set; }
    public int DayUploadCount { get; set; }
    public long StoredSize { get; set; }
    public DateTime CreatedAt { get; set; }
    public static GetOrganizationByIdDto Map(Organization org) 
    {
        return new GetOrganizationByIdDto
        {
            Id = org.Id,
            Plan = PlanService.Plans.Find(x => x.Name == org.Plan) ?? throw new HttpException(400, "Plano invalido"),
            DayUploadCount = org.DayUploadCount,
            StoredSize = org.StoredSize,
            CreatedAt = org.CreatedAt
        };
    }
}