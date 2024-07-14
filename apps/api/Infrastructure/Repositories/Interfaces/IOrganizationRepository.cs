using Domain;

namespace Infrastructure.Repositories.Interfaces;

public interface IOrganizationRepository : IRepository<Organization, int> {
    void DeleteList(List<Organization> list);
    Task ResetUploadDayCount();
}
