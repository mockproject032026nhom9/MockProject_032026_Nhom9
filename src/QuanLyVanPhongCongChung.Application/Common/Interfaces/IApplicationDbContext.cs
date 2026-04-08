namespace QuanLyVanPhongCongChung.Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;
using QuanLyVanPhongCongChung.Domain.Entities.Identity;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;
using QuanLyVanPhongCongChung.Domain.Entities.Jobs;
using QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;
using QuanLyVanPhongCongChung.Domain.Entities.Journal;
using QuanLyVanPhongCongChung.Domain.Entities.Seals;
using QuanLyVanPhongCongChung.Domain.Entities.Security;
using QuanLyVanPhongCongChung.Domain.Entities.Organizations;
using QuanLyVanPhongCongChung.Domain.Entities.Geography;

public interface IApplicationDbContext
{
    // Identity
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }

    // Notary
    DbSet<Notary> Notaries { get; }
    DbSet<NotaryCommission> NotaryCommissions { get; }
    DbSet<NotaryBond> NotaryBonds { get; }
    DbSet<NotaryInsurance> NotaryInsurances { get; }
    DbSet<NotaryServiceArea> NotaryServiceAreas { get; }
    DbSet<NotaryAvailability> NotaryAvailabilities { get; }
    DbSet<NotaryCapability> NotaryCapabilities { get; }
    DbSet<NotaryDocument> NotaryDocuments { get; }
    DbSet<NotaryStatusHistory> NotaryStatusHistories { get; }
    DbSet<NotaryAuditLog> NotaryAuditLogs { get; }
    DbSet<NotaryIncident> NotaryIncidents { get; }
    DbSet<AuthorityScope> AuthorityScopes { get; }
    DbSet<RonTechnology> RonTechnologies { get; }

    // Jobs
    DbSet<Job> Jobs { get; }
    DbSet<JobAssignment> JobAssignments { get; }
    DbSet<JobStatusLog> JobStatusLogs { get; }
    DbSet<Event> Events { get; }
    DbSet<Notification> Notifications { get; }

    // Notarial Acts
    DbSet<NotarialAct> NotarialActs { get; }
    DbSet<ActSignature> ActSignatures { get; }
    DbSet<ActLogEntry> ActLogEntries { get; }
    DbSet<ComplianceReview> ComplianceReviews { get; }

    // Journal
    DbSet<JournalEntry> JournalEntries { get; }
    DbSet<Signer> Signers { get; }
    DbSet<FeeBreakdown> FeeBreakdowns { get; }
    DbSet<BiometricData> BiometricDatas { get; }
    DbSet<ExportHistory> ExportHistories { get; }

    // Seals
    DbSet<Seal> Seals { get; }
    DbSet<DigitalSignature> DigitalSignatures { get; }
    DbSet<SealUsageLog> SealUsageLogs { get; }
    DbSet<Certificate> Certificates { get; }
    DbSet<CertificateAuthority> CertificateAuthorities { get; }
    DbSet<HsmKeyStorage> HsmKeyStorages { get; }
    DbSet<Device> Devices { get; }

    // Security
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<SecurityIncident> SecurityIncidents { get; }
    DbSet<Revocation> Revocations { get; }
    DbSet<Replacement> Replacements { get; }

    // Organizations
    DbSet<Organization> Organizations { get; }
    DbSet<ServiceRequest> ServiceRequests { get; }
    DbSet<Verification> Verifications { get; }
    DbSet<Delivery> Deliveries { get; }
    DbSet<Payment> Payments { get; }
    DbSet<Document> Documents { get; }

    // Geography
    DbSet<State> States { get; }
    DbSet<Language> Languages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
