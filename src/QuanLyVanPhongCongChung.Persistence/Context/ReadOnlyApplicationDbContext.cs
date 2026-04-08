namespace QuanLyVanPhongCongChung.Persistence.Context;

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

public class ReadOnlyApplicationDbContext(
    DbContextOptions<ReadOnlyApplicationDbContext> options
) : DbContext(options), IReadOnlyApplicationDbContext
{
    // Identity
    public IQueryable<User> Users => Set<User>().AsNoTracking();
    public IQueryable<Role> Roles => Set<Role>().AsNoTracking();

    // Notary
    public IQueryable<Notary> Notaries => Set<Notary>().AsNoTracking();
    public IQueryable<NotaryCommission> NotaryCommissions => Set<NotaryCommission>().AsNoTracking();
    public IQueryable<NotaryBond> NotaryBonds => Set<NotaryBond>().AsNoTracking();
    public IQueryable<NotaryInsurance> NotaryInsurances => Set<NotaryInsurance>().AsNoTracking();
    public IQueryable<NotaryServiceArea> NotaryServiceAreas => Set<NotaryServiceArea>().AsNoTracking();
    public IQueryable<NotaryAvailability> NotaryAvailabilities => Set<NotaryAvailability>().AsNoTracking();
    public IQueryable<NotaryCapability> NotaryCapabilities => Set<NotaryCapability>().AsNoTracking();
    public IQueryable<NotaryDocument> NotaryDocuments => Set<NotaryDocument>().AsNoTracking();
    public IQueryable<NotaryStatusHistory> NotaryStatusHistories => Set<NotaryStatusHistory>().AsNoTracking();
    public IQueryable<NotaryAuditLog> NotaryAuditLogs => Set<NotaryAuditLog>().AsNoTracking();
    public IQueryable<NotaryIncident> NotaryIncidents => Set<NotaryIncident>().AsNoTracking();
    public IQueryable<AuthorityScope> AuthorityScopes => Set<AuthorityScope>().AsNoTracking();
    public IQueryable<RonTechnology> RonTechnologies => Set<RonTechnology>().AsNoTracking();

    // Jobs
    public IQueryable<Job> Jobs => Set<Job>().AsNoTracking();
    public IQueryable<JobAssignment> JobAssignments => Set<JobAssignment>().AsNoTracking();
    public IQueryable<JobStatusLog> JobStatusLogs => Set<JobStatusLog>().AsNoTracking();
    public IQueryable<Event> Events => Set<Event>().AsNoTracking();
    public IQueryable<Notification> Notifications => Set<Notification>().AsNoTracking();

    // Notarial Acts
    public IQueryable<NotarialAct> NotarialActs => Set<NotarialAct>().AsNoTracking();
    public IQueryable<ActSignature> ActSignatures => Set<ActSignature>().AsNoTracking();
    public IQueryable<ActLogEntry> ActLogEntries => Set<ActLogEntry>().AsNoTracking();
    public IQueryable<ComplianceReview> ComplianceReviews => Set<ComplianceReview>().AsNoTracking();

    // Journal
    public IQueryable<JournalEntry> JournalEntries => Set<JournalEntry>().AsNoTracking();
    public IQueryable<Signer> Signers => Set<Signer>().AsNoTracking();
    public IQueryable<FeeBreakdown> FeeBreakdowns => Set<FeeBreakdown>().AsNoTracking();
    public IQueryable<BiometricData> BiometricDatas => Set<BiometricData>().AsNoTracking();
    public IQueryable<ExportHistory> ExportHistories => Set<ExportHistory>().AsNoTracking();

    // Seals
    public IQueryable<Seal> Seals => Set<Seal>().AsNoTracking();
    public IQueryable<DigitalSignature> DigitalSignatures => Set<DigitalSignature>().AsNoTracking();
    public IQueryable<SealUsageLog> SealUsageLogs => Set<SealUsageLog>().AsNoTracking();
    public IQueryable<Certificate> Certificates => Set<Certificate>().AsNoTracking();
    public IQueryable<CertificateAuthority> CertificateAuthorities => Set<CertificateAuthority>().AsNoTracking();
    public IQueryable<HsmKeyStorage> HsmKeyStorages => Set<HsmKeyStorage>().AsNoTracking();
    public IQueryable<Device> Devices => Set<Device>().AsNoTracking();

    // Security
    public IQueryable<AuditLog> AuditLogs => Set<AuditLog>().AsNoTracking();
    public IQueryable<Revocation> Revocations => Set<Revocation>().AsNoTracking();
    public IQueryable<Replacement> Replacements => Set<Replacement>().AsNoTracking();

    // Organizations
    public IQueryable<Organization> Organizations => Set<Organization>().AsNoTracking();
    public IQueryable<ServiceRequest> ServiceRequests => Set<ServiceRequest>().AsNoTracking();
    public IQueryable<Verification> Verifications => Set<Verification>().AsNoTracking();
    public IQueryable<Delivery> Deliveries => Set<Delivery>().AsNoTracking();
    public IQueryable<Payment> Payments => Set<Payment>().AsNoTracking();
    public IQueryable<Document> Documents => Set<Document>().AsNoTracking();

    // Geography
    public IQueryable<State> States => Set<State>().AsNoTracking();
    public IQueryable<Language> Languages => Set<Language>().AsNoTracking();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplySqlServerNamingConventions();

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

    public override int SaveChanges()
        => throw new InvalidOperationException("Read-only context cannot save changes.");

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("Read-only context cannot save changes.");
}
