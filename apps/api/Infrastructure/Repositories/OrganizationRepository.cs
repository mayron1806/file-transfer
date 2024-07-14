using Domain;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrganizationRepository(DatabaseContext context) : Repository<Organization, int>(context), IOrganizationRepository
    {
        public void DeleteList(List<Organization> list) 
        {
            _dbSet.RemoveRange(list);
        }

        public async Task ResetUploadDayCount() => await _dbSet.ExecuteUpdateAsync(x => x.SetProperty(p => p.DayUploadCount, 0));
        
    }
}