using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class UserRepository(DatabaseContext context) : Repository<User, int>(context), IUserRepository
    {
    }
}