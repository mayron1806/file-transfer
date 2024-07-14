using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync(IDbContextTransaction transaction);
    Task RollbackTransactionAsync(IDbContextTransaction transaction);
    IUserRepository User { get; }
    IActiveAccountTokenRepository ActiveAccountToken { get; }
    IResetPasswordTokenRepository ResetPasswordToken { get; }
    IFileRepository File { get; }
    ITransferRepository Transfer { get; }
    IOrganizationRepository Organization { get; }
    IMemberRepository Member { get; }
}