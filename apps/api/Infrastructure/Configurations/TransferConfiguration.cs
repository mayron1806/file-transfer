using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Infrastructure.Configurations;

public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder
            .HasMany(x => x.Files)
            .WithOne(x => x.Transfer)
            .HasForeignKey(x => x.TransferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(24);

        builder
            .Property(x => x.Name)
            .HasMaxLength(100);

        builder
            .HasIndex(x => x.Key)
            .IsUnique(true);

        builder
            .HasOne(x => x.Organization)
            .WithMany(x => x.Transfers)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW()");
        
        builder
            .Property(x => x.ExpiresAt)
            .IsRequired();

        builder
            .Property(x => x.Size)
            .IsRequired();
        
        builder
            .Property(x => x.FilesCount)
            .IsRequired();

        builder
            .Property(x => x.Expired)
            .HasDefaultValue(false)
            .IsRequired();
    
        builder
            .Property(x => x.Path)
            .IsRequired();
        
        builder
            .Property(x => x.TransferType)
            .IsRequired();
        builder.OwnsOne(x => x.Receive, r => {
            r.Property(p => p.Status).HasDefaultValue(ReceiveStatus.Pending);
            r.Property(p => p.Message).HasMaxLength(500);
            r.Property(p => p.MaxFiles).HasDefaultValue(null);
            r.Property(p => p.AcceptedFiles)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonConvert.SerializeObject(v), 
                    v => JsonConvert.DeserializeObject<IEnumerable<string>>(v)
                )
                .Metadata.SetValueComparer(new ValueComparer<IEnumerable<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                ));
            r.Property(p => p.MaxSize).IsRequired();
            r.Property(p => p.Password).HasMaxLength(100);
        });
           
        
         builder.OwnsOne(x => x.Send, r => {
            r.Property(p => p.Message).HasMaxLength(500);
            r.Property(p => p.Password).HasMaxLength(100);
            r.Property(p => p.Downloads).HasDefaultValue(0);
            r.Property(p => p.ExpiresOnDowload).HasDefaultValue(false);
            r.Property(p => p.Destination)
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonConvert.SerializeObject(v), 
                    v => JsonConvert.DeserializeObject<IEnumerable<string>>(v)
                )
                .Metadata.SetValueComparer(new ValueComparer<IEnumerable<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                ));
        });
           
    }
}
