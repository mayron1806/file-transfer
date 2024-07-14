using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories;

public class ActiveAccountTokenRepository(DatabaseContext context) : Repository<ActiveAccountToken, int>(context), IActiveAccountTokenRepository
{
}