namespace QuanLyVanPhongCongChung.Persistence.Context;

using MediatR;
using Microsoft.EntityFrameworkCore;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Domain.Entities.Identity;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;
using QuanLyVanPhongCongChung.Domain.Entities.Jobs;
using QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;
using QuanLyVanPhongCongChung.Domain.Entities.Journal;
using QuanLyVanPhongCongChung.Domain.Entities.Seals;
using QuanLyVanPhongCongChung.Domain.Entities.Security;
using QuanLyVanPhongCongChung.Domain.Entities.Organizations;
using QuanLyVanPhongCongChung.Domain.Entities.Geography;
using QuanLyVanPhongCongChung.Persistence.Extensions;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IMediator mediator
) : DbContext(options), IApplicationDbContext, IUnitOfWork
{
    // Identity
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();

    // Notary
    public DbSet<Notary> Notaries => Set<Notary>();
    public DbSet<NotaryCommission> NotaryCommissions => Set<NotaryCommission>();
    public DbSet<NotaryBond> NotaryBonds => Set<NotaryBond>();
    public DbSet<NotaryInsurance> NotaryInsurances => Set<NotaryInsurance>();
    public DbSet<NotaryServiceArea> NotaryServiceAreas => Set<NotaryServiceArea>();
    public DbSet<NotaryAvailability> NotaryAvailabilities => Set<NotaryAvailability>();
    public DbSet<NotaryCapability> NotaryCapabilities => Set<NotaryCapability>();
    public DbSet<NotaryDocument> NotaryDocuments => Set<NotaryDocument>();
    public DbSet<NotaryStatusHistory> NotaryStatusHistories => Set<NotaryStatusHistory>();
    public DbSet<NotaryAuditLog> NotaryAuditLogs => Set<NotaryAuditLog>();
    public DbSet<NotaryIncident> NotaryIncidents => Set<NotaryIncident>();
    public DbSet<AuthorityScope> AuthorityScopes => Set<AuthorityScope>();
    public DbSet<RonTechnology> RonTechnologies => Set<RonTechnology>();

    // Jobs
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobAssignment> JobAssignments => Set<JobAssignment>();
    public DbSet<JobStatusLog> JobStatusLogs => Set<JobStatusLog>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Notification> Notifications => Set<Notification>();

    // Notarial Acts
    public DbSet<NotarialAct> NotarialActs => Set<NotarialAct>();
    public DbSet<ActSignature> ActSignatures => Set<ActSignature>();
    public DbSet<ActLogEntry> ActLogEntries => Set<ActLogEntry>();
    public DbSet<ComplianceReview> ComplianceReviews => Set<ComplianceReview>();

    // Journal
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<Signer> Signers => Set<Signer>();
    public DbSet<FeeBreakdown> FeeBreakdowns => Set<FeeBreakdown>();
    public DbSet<BiometricData> BiometricDatas => Set<BiometricData>();
    public DbSet<ExportHistory> ExportHistories => Set<ExportHistory>();

    // Seals
    public DbSet<Seal> Seals => Set<Seal>();
    public DbSet<DigitalSignature> DigitalSignatures => Set<DigitalSignature>();
    public DbSet<SealUsageLog> SealUsageLogs => Set<SealUsageLog>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<CertificateAuthority> CertificateAuthorities => Set<CertificateAuthority>();
    public DbSet<HsmKeyStorage> HsmKeyStorages => Set<HsmKeyStorage>();
    public DbSet<Device> Devices => Set<Device>();

    // Security
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Revocation> Revocations => Set<Revocation>();
    public DbSet<Replacement> Replacements => Set<Replacement>();

    // Organizations
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
    public DbSet<Verification> Verifications => Set<Verification>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Document> Documents => Set<Document>();

    // Geography
    public DbSet<State> States => Set<State>();
    public DbSet<Language> Languages => Set<Language>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Global convention: store all enums as strings
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType.IsEnum)
                {
                    property.SetProviderClrType(typeof(string));
                    property.SetMaxLength(50);
                }
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await mediator.DispatchDomainEventsAsync(this);
        return await base.SaveChangesAsync(cancellationToken);
    }
}
