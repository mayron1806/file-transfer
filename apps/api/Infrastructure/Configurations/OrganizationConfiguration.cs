using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder
            .HasMany(x => x.Members)
            .WithOne(x => x.Organization)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Transfers)
            .WithOne(x => x.Organization)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.StoredSize)
            .HasDefaultValue(0)
            .HasColumnType("bigint")
            .IsRequired();
        
        builder
            .Property(x => x.DayUploadCount)
            .HasDefaultValue(0)
            .IsRequired();
        
        builder
            .Property(x => x.Plan)
            .HasMaxLength(100)
            .HasDefaultValue(null);
        
        builder
            .Property(x => x.PlanActive)
            .HasDefaultValue(false);
        
        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW()");
        
    }
}
