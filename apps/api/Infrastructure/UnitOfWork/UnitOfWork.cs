using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitOfWork;

public class UnitOfWork: IUnitOfWork
{
    private readonly DatabaseContext _context;

    #region Repositories
    public IUserRepository User { get; }
    public IActiveAccountTokenRepository ActiveAccountToken { get; }
    public IResetPasswordTokenRepository ResetPasswordToken { get; }
    public IFileRepository File { get; }
    public ITransferRepository Transfer { get; }
    public IOrganizationRepository Organization { get; }
    public IMemberRepository Member { get; }
    #endregion
    
    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
        User = new UserRepository(_context);
        ActiveAccountToken = new ActiveAccountTokenRepository(_context);
        ResetPasswordToken = new ResetPasswordTokenRepository(_context);
        File = new FileRepository(_context);
        Transfer = new TransferRepository(_context);
        Organization = new OrganizationRepository(_context);
        Member = new MemberRepository(_context);
    }
    public async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
    public async Task CommitTransactionAsync(IDbContextTransaction transaction) => await transaction.CommitAsync();
    public async Task RollbackTransactionAsync(IDbContextTransaction transaction) => await transaction.RollbackAsync();
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    #region Dispose
    private bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}