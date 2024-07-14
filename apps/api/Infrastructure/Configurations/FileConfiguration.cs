using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class FileConfiguration : IEntityTypeConfiguration<Domain.File>
{
    public void Configure(EntityTypeBuilder<Domain.File> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder
            .Property(x => x.OriginalName)
            .IsRequired();
        
        builder
            .Property(x => x.Path)
            .IsRequired();

        builder
            .Property(x => x.Size)
            .IsRequired();
        
        builder
            .Property(x => x.ContentType)
            .IsRequired();

        builder
            .Property(x => x.Key);

        builder
            .Property(x => x.Status)
            .IsRequired();

        builder
            .Property(x => x.ErrorMessage)
            .HasDefaultValue(null)
            .IsRequired(false);
        builder
            .HasOne(x => x.Transfer)
            .WithMany(x => x.Files)
            .HasForeignKey(x => x.TransferId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
