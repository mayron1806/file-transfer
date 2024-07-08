using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories
{
    public class ResetPasswordTokenRepository(DatabaseContext context) : Repository<ResetPasswordToken, int>(context), IResetPasswordTokenRepository
    {
    }
}