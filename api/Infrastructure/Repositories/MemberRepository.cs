using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class MemberRepository(DatabaseContext context) : Repository<Member, int>(context), IMemberRepository
    {
    }
}