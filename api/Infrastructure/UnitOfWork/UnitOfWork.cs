using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.UnitOfWork;

public class UnitOfWork: IUnitOfWork
{
    private readonly DatabaseContext _context;
    public IUserRepository User { get; }

    public IActiveAccountTokenRepository ActiveAccountToken { get; }

    public IResetPasswordTokenRepository ResetPasswordToken { get; }

    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
        User = new UserRepository(_context);
        ActiveAccountToken = new ActiveAccountTokenRepository(_context);
        ResetPasswordToken = new ResetPasswordTokenRepository(_context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
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
}