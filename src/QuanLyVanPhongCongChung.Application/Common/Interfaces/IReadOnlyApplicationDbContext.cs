namespace QuanLyVanPhongCongChung.Application.Common.Interfaces;

using QuanLyVanPhongCongChung.Domain.Entities.Identity;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;
using QuanLyVanPhongCongChung.Domain.Entities.Jobs;
using QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;
using QuanLyVanPhongCongChung.Domain.Entities.Journal;
using QuanLyVanPhongCongChung.Domain.Entities.Seals;
using QuanLyVanPhongCongChung.Domain.Entities.Security;
using QuanLyVanPhongCongChung.Domain.Entities.Organizations;
using QuanLyVanPhongCongChung.Domain.Entities.Geography;

public interface IReadOnlyApplicationDbContext
{
    // Identity
    IQueryable<User> Users { get; }
    IQueryable<Role> Roles { get; }

    // Notary
    IQueryable<Notary> Notaries { get; }
    IQueryable<NotaryCommission> NotaryCommissions { get; }
    IQueryable<NotaryBond> NotaryBonds { get; }
    IQueryable<NotaryInsurance> NotaryInsurances { get; }
    IQueryable<NotaryServiceArea> NotaryServiceAreas { get; }
    IQueryable<NotaryAvailability> NotaryAvailabilities { get; }
    IQueryable<NotaryCapability> NotaryCapabilities { get; }
    IQueryable<NotaryDocument> NotaryDocuments { get; }
    IQueryable<NotaryStatusHistory> NotaryStatusHistories { get; }
    IQueryable<NotaryAuditLog> NotaryAuditLogs { get; }
    IQueryable<NotaryIncident> NotaryIncidents { get; }
    IQueryable<AuthorityScope> AuthorityScopes { get; }
    IQueryable<RonTechnology> RonTechnologies { get; }

    // Jobs
    IQueryable<Job> Jobs { get; }
    IQueryable<JobAssignment> JobAssignments { get; }
    IQueryable<JobStatusLog> JobStatusLogs { get; }
    IQueryable<Event> Events { get; }
    IQueryable<Notification> Notifications { get; }

    // Notarial Acts
    IQueryable<NotarialAct> NotarialActs { get; }
    IQueryable<ActSignature> ActSignatures { get; }
    IQueryable<ActLogEntry> ActLogEntries { get; }
    IQueryable<ComplianceReview> ComplianceReviews { get; }

    // Journal
    IQueryable<JournalEntry> JournalEntries { get; }
    IQueryable<Signer> Signers { get; }
    IQueryable<FeeBreakdown> FeeBreakdowns { get; }
    IQueryable<BiometricData> BiometricDatas { get; }
    IQueryable<ExportHistory> ExportHistories { get; }

    // Seals
    IQueryable<Seal> Seals { get; }
    IQueryable<DigitalSignature> DigitalSignatures { get; }
    IQueryable<SealUsageLog> SealUsageLogs { get; }
    IQueryable<Certificate> Certificates { get; }
    IQueryable<CertificateAuthority> CertificateAuthorities { get; }
    IQueryable<HsmKeyStorage> HsmKeyStorages { get; }
    IQueryable<Device> Devices { get; }

    // Security
    IQueryable<AuditLog> AuditLogs { get; }
    IQueryable<Revocation> Revocations { get; }
    IQueryable<Replacement> Replacements { get; }

    // Organizations
    IQueryable<Organization> Organizations { get; }
    IQueryable<ServiceRequest> ServiceRequests { get; }
    IQueryable<Verification> Verifications { get; }
    IQueryable<Delivery> Deliveries { get; }
    IQueryable<Payment> Payments { get; }
    IQueryable<Document> Documents { get; }

    // Geography
    IQueryable<State> States { get; }
    IQueryable<Language> Languages { get; }
}
