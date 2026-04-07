namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotaryEntity = QuanLyVanPhongCongChung.Domain.Entities.Notary.Notary;

public class NotaryConfiguration : IEntityTypeConfiguration<NotaryEntity>
{
    public void Configure(EntityTypeBuilder<NotaryEntity> builder)
    {
        builder.ToTable("notaries");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).HasColumnName("id");
        builder.Property(n => n.UserId).HasColumnName("user_id");
        builder.Property(n => n.Ssn).HasColumnName("ssn").HasMaxLength(20).IsRequired();
        builder.Property(n => n.FullName).HasColumnName("full_name").HasMaxLength(200).IsRequired();
        builder.Property(n => n.DateOfBirth).HasColumnName("date_of_birth");
        builder.Property(n => n.PhotoUrl).HasColumnName("photo_url").HasMaxLength(500);
        builder.Property(n => n.Phone).HasColumnName("phone").HasMaxLength(20);
        builder.Property(n => n.Email).HasColumnName("email").HasMaxLength(256);
        builder.Property(n => n.EmploymentType).HasColumnName("employment_type");
        builder.Property(n => n.StartDate).HasColumnName("start_date");
        builder.Property(n => n.InternalNotes).HasColumnName("internal_notes").HasMaxLength(2000);
        builder.Property(n => n.Status).HasColumnName("status");
        builder.Property(n => n.ResidentialAddress).HasColumnName("residential_address").HasMaxLength(500);
        builder.Property(n => n.CreatedAt).HasColumnName("created_at");
        builder.Property(n => n.CreatedBy).HasColumnName("created_by").HasMaxLength(100);
        builder.Property(n => n.LastModifiedAt).HasColumnName("last_modified_at");
        builder.Property(n => n.LastModifiedBy).HasColumnName("last_modified_by").HasMaxLength(100);

        builder.HasOne(n => n.User)
            .WithOne()
            .HasForeignKey<NotaryEntity>(n => n.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(n => n.Commissions)
            .WithOne(c => c.Notary)
            .HasForeignKey(c => c.NotaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(n => n.Bonds)
            .WithOne(b => b.Notary)
            .HasForeignKey(b => b.NotaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(n => n.Insurances)
            .WithOne(i => i.Notary)
            .HasForeignKey(i => i.NotaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(n => n.ServiceAreas)
            .WithOne(s => s.Notary)
            .HasForeignKey(s => s.NotaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(n => n.Documents)
            .WithOne(d => d.Notary)
            .HasForeignKey(d => d.NotaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(n => n.StatusHistories)
            .WithOne(s => s.Notary)
            .HasForeignKey(s => s.NotaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(n => n.AuditLogs)
            .WithOne(a => a.Notary)
            .HasForeignKey(a => a.NotaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(n => n.Incidents)
            .WithOne(i => i.Notary)
            .HasForeignKey(i => i.NotaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(n => n.Availability)
            .WithOne(a => a.Notary)
            .HasForeignKey<Domain.Entities.Notary.NotaryAvailability>(a => a.NotaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(n => n.Capability)
            .WithOne(c => c.Notary)
            .HasForeignKey<Domain.Entities.Notary.NotaryCapability>(c => c.NotaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(n => n.UserId).IsUnique();
        builder.HasIndex(n => n.Ssn).IsUnique();
        builder.HasIndex(n => n.Email);
        builder.HasIndex(n => n.Status);

        builder.Navigation(n => n.Commissions).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(n => n.Bonds).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(n => n.Insurances).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(n => n.ServiceAreas).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(n => n.Documents).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(n => n.StatusHistories).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(n => n.AuditLogs).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(n => n.Incidents).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
