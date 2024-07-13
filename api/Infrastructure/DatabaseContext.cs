using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DatabaseContext(DbContextOptions<DatabaseContext> options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }
    public DbSet<ActiveAccountToken> ActiveAccountTokens { get; set; }
    public DbSet<Domain.File> Files { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<Member> Members { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityByDefaultColumns();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}
