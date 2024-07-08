using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ActiveAccountTokenConfiguration : IEntityTypeConfiguration<ActiveAccountToken>
{
    public void Configure(EntityTypeBuilder<ActiveAccountToken> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Content).IsRequired().HasMaxLength(100);
        
        builder
            .HasOne(x => x.User)
            .WithOne(x => x.ActiveAccountToken)
            .HasForeignKey<ActiveAccountToken>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
        builder.Property(x => x.ExpiresAt).IsRequired();
    }
}
