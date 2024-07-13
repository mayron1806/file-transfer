using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ResetPasswordTokenConfiguration : IEntityTypeConfiguration<ResetPasswordToken>
{
    public void Configure(EntityTypeBuilder<ResetPasswordToken> builder)
    {
        builder.HasKey(x => x.Id);
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Content).IsRequired().HasMaxLength(100);
        
        builder
            .HasOne(x => x.User)
            .WithOne(x => x.ResetPasswordToken)
            .HasForeignKey<ResetPasswordToken>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("NOW()");
        builder.Property(x => x.ExpiresAt).IsRequired();
    }
}
