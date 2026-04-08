namespace QuanLyVanPhongCongChung.Persistence.Seed;

using System.Globalization;
using System.IO;
using System.Reflection;
using QuanLyVanPhongCongChung.Domain.Entities.Geography;
using QuanLyVanPhongCongChung.Domain.Entities.Identity;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;
using QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;
using QuanLyVanPhongCongChung.Domain.Entities.Jobs;
using QuanLyVanPhongCongChung.Domain.Entities.Organizations;
using QuanLyVanPhongCongChung.Domain.Entities.Security;
using QuanLyVanPhongCongChung.Domain.Enums;
using QuanLyVanPhongCongChung.Persistence.Context;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Seed Roles
        if (!context.Roles.Any())
        {
            var roleNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in LoadSimpleCsvRows("role.csv"))
            {
                if (row.Length > 1 && !string.IsNullOrWhiteSpace(row[1]))
                {
                    roleNames.Add(row[1].Trim());
                }
            }

            var requiredSystemRoles = new[]
            {
                "Admin",
                "Notary",
                "LeadAuditor",
                "Compliance",
                "Regulator",
                "Client"
            };

            foreach (var role in requiredSystemRoles)
            {
                roleNames.Add(role);
            }

            var roles = roleNames.Select(Role.Create).ToArray();

            context.Roles.AddRange(roles);
            await context.SaveChangesAsync();
        }

        // Seed Languages
        if (!context.Languages.Any())
        {
            var languageRows = LoadSimpleCsvRows("languages.csv");

            var languages = languageRows
                .Where(row => row.Length > 2)
                .Select(row => Language.Create(row[1].Trim().ToUpperInvariant(), row[2].Trim()))
                .ToArray();

            if (languages.Length == 0)
            {
                languages =
                [
                    Language.Create("EN", "English"),
                    Language.Create("ES", "Spanish"),
                    Language.Create("FR", "French"),
                    Language.Create("VI", "Vietnamese")
                ];
            }

            context.Languages.AddRange(languages);
            await context.SaveChangesAsync();
        }

        // Seed States
        if (!context.States.Any())
        {
            var stateRows = LoadSimpleCsvRows("states.csv");

            var states = stateRows
                .Where(row => row.Length > 2)
                .Select(row => State.Create(row[1].Trim().ToUpperInvariant(), row[2].Trim()))
                .ToArray();

            if (states.Length == 0)
            {
                states =
                [
                    State.Create("WA", "Washington"),
                    State.Create("CA", "California"),
                    State.Create("TX", "Texas"),
                    State.Create("NY", "New York"),
                    State.Create("FL", "Florida"),
                    State.Create("CO", "Colorado"),
                    State.Create("NV", "Nevada"),
                    State.Create("AZ", "Arizona"),
                    State.Create("IL", "Illinois"),
                    State.Create("OH", "Ohio"),
                    State.Create("PA", "Pennsylvania"),
                    State.Create("MI", "Michigan"),
                    State.Create("NC", "North Carolina")
                ];
            }

            context.States.AddRange(states);
            await context.SaveChangesAsync();
        }

        // Seed Users and Notaries
        if (!context.Users.Any())
        {
            var notaryRole = context.Roles.FirstOrDefault(r => r.RoleName == "Notary")
                ?? throw new InvalidOperationException("Notary role not found");

            var notaryUsers = new[]
            {
                new { UserEmail = "j.smith@mail.com", Ssn = "123-45-6789", FullName = "James Smith", DateOfBirth = new DateOnly(1985, 2, 15), PhotoUrl = "/img/jsmith.jpg", Phone = "(555) 123-4567", NotaryEmail = "j.smith@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2021, 1, 6), InternalNotes = "Top performer 2022", Status = NotaryStatus.Active, ResidentialAddress = "123 Maple St, Seattle, WA 98101" },
                new { UserEmail = "emily.j@mail.com", Ssn = "234-56-7890", FullName = "Emily Johnson", DateOfBirth = new DateOnly(1990, 8, 22), PhotoUrl = "/img/ejohnson.jpg", Phone = "(555) 234-5678", NotaryEmail = "emily.j@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2022, 1, 15), InternalNotes = (string?)null, Status = NotaryStatus.Active, ResidentialAddress = "456 Oak Ave, Austin, TX 73301" },
                new { UserEmail = "m.williams@mail.com", Ssn = "345-67-8901", FullName = "Michael Williams", DateOfBirth = new DateOnly(1988, 11, 30), PhotoUrl = "/img/mwilliams.jpg", Phone = "(555) 345-6789", NotaryEmail = "m.williams@mail.com", EmploymentType = EmploymentType.IndependentContract, StartDate = new DateOnly(2023, 10, 3), InternalNotes = "Background check renewed", Status = NotaryStatus.Active, ResidentialAddress = "789 Pine Ln, Chicago, IL 60601" },
                new { UserEmail = "jessica.b@mail.com", Ssn = "456-78-9012", FullName = "Jessica Brown", DateOfBirth = new DateOnly(1992, 12, 4), PhotoUrl = "/img/jbrown.jpg", Phone = "(555) 456-7890", NotaryEmail = "jessica.b@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2021, 1, 9), InternalNotes = "On maternity leave", Status = NotaryStatus.Inactive, ResidentialAddress = "321 Cedar Blvd, Denver, CO 80202" },
                new { UserEmail = "d.jones@mail.com", Ssn = "567-89-0123", FullName = "David Jones", DateOfBirth = new DateOnly(1986, 7, 7), PhotoUrl = "/img/djones.jpg", Phone = "(555) 567-8901", NotaryEmail = "d.jones@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2020, 11, 20), InternalNotes = (string?)null, Status = NotaryStatus.Active, ResidentialAddress = "654 Elm St, Boston, MA 02108" },
                new { UserEmail = "sarah.g@mail.com", Ssn = "678-90-1234", FullName = "Sarah Garcia", DateOfBirth = new DateOnly(1995, 9, 25), PhotoUrl = "/img/sgarcia.jpg", Phone = "(555) 678-9012", NotaryEmail = "sarah.g@mail.com", EmploymentType = EmploymentType.IndependentContract, StartDate = new DateOnly(2024, 5, 1), InternalNotes = "Pending state license", Status = NotaryStatus.Inactive, ResidentialAddress = "987 Birch Rd, Miami, FL 33101" },
                new { UserEmail = "r.miller@mail.com", Ssn = "789-01-2345", FullName = "Robert Miller", DateOfBirth = new DateOnly(1983, 12, 12), PhotoUrl = "/img/rmiller.jpg", Phone = "(555) 789-0123", NotaryEmail = "r.miller@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2019, 5, 15), InternalNotes = "Policy violation reported", Status = NotaryStatus.Blocked, ResidentialAddress = "159 Walnut Ct, Phoenix, AZ 85001" },
                new { UserEmail = "ashley.d@mail.com", Ssn = "890-12-3456", FullName = "Ashley Davis", DateOfBirth = new DateOnly(1991, 8, 3), PhotoUrl = "/img/adavis.jpg", Phone = "(555) 890-1234", NotaryEmail = "ashley.d@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2022, 10, 8), InternalNotes = (string?)null, Status = NotaryStatus.Active, ResidentialAddress = "753 Cherry Way, Atlanta, GA 30301" },
                new { UserEmail = "w.rodriguez@mail.com", Ssn = "901-23-4567", FullName = "William Rodriguez", DateOfBirth = new DateOnly(1989, 6, 18), PhotoUrl = "/img/wrodriguez.jpg", Phone = "(555) 901-2345", NotaryEmail = "w.rodriguez@mail.com", EmploymentType = EmploymentType.IndependentContract, StartDate = new DateOnly(2023, 1, 11), InternalNotes = "Bilingual (Spanish)", Status = NotaryStatus.Active, ResidentialAddress = "852 Ash Pl, Dallas, TX 75201" },
                new { UserEmail = "amanda.m@mail.com", Ssn = "012-34-5678", FullName = "Amanda Martinez", DateOfBirth = new DateOnly(1994, 10, 20), PhotoUrl = "/img/amartinez.jpg", Phone = "(555) 012-3456", NotaryEmail = "amanda.m@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2021, 2, 28), InternalNotes = "Senior Notary", Status = NotaryStatus.Active, ResidentialAddress = "951 Spruce Dr, San Diego, CA 92101" },
                new { UserEmail = "j.hernandez@mail.com", Ssn = "111-22-3333", FullName = "Joseph Hernandez", DateOfBirth = new DateOnly(1987, 5, 1), PhotoUrl = "/img/jhernandez.jpg", Phone = "(555) 111-2233", NotaryEmail = "j.hernandez@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2020, 7, 15), InternalNotes = (string?)null, Status = NotaryStatus.Active, ResidentialAddress = "124 King St, Portland, OR 97204" },
                new { UserEmail = "melissa.l@mail.com", Ssn = "222-33-4444", FullName = "Melissa Lopez", DateOfBirth = new DateOnly(1993, 5, 14), PhotoUrl = "/img/mlopez.jpg", Phone = "(555) 222-3344", NotaryEmail = "melissa.l@mail.com", EmploymentType = EmploymentType.IndependentContract, StartDate = new DateOnly(2024, 10, 2), InternalNotes = (string?)null, Status = NotaryStatus.Active, ResidentialAddress = "567 Queen Ave, Las Vegas, NV 89101" },
                new { UserEmail = "c.gonzalez@mail.com", Ssn = "333-44-5555", FullName = "Charles Gonzalez", DateOfBirth = new DateOnly(1984, 9, 8), PhotoUrl = "/img/cgonzalez.jpg", Phone = "(555) 333-4455", NotaryEmail = "c.gonzalez@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2018, 4, 20), InternalNotes = "Branch Manager", Status = NotaryStatus.Active, ResidentialAddress = "890 Prince Rd, Orlando, FL 32801" },
                new { UserEmail = "nicole.w@mail.com", Ssn = "444-55-6666", FullName = "Nicole Wilson", DateOfBirth = new DateOnly(1996, 2, 12), PhotoUrl = "/img/nwilson.jpg", Phone = "(555) 444-5566", NotaryEmail = "nicole.w@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2023, 5, 9), InternalNotes = (string?)null, Status = NotaryStatus.Active, ResidentialAddress = "234 Main St, Columbus, OH 43215" },
                new { UserEmail = "t.anderson@mail.com", Ssn = "555-66-7777", FullName = "Thomas Anderson", DateOfBirth = new DateOnly(1982, 11, 11), PhotoUrl = "/img/tanderson.jpg", Phone = "(555) 555-6677", NotaryEmail = "t.anderson@mail.com", EmploymentType = EmploymentType.IndependentContract, StartDate = new DateOnly(2021, 12, 12), InternalNotes = "Customer complaints", Status = NotaryStatus.Blocked, ResidentialAddress = "345 Oak Ave, Charlotte, NC 28202" },
                new { UserEmail = "samantha.t@mail.com", Ssn = "666-77-8888", FullName = "Samantha Thomas", DateOfBirth = new DateOnly(1990, 2, 28), PhotoUrl = "/img/sthomas.jpg", Phone = "(555) 666-7788", NotaryEmail = "samantha.t@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2022, 5, 18), InternalNotes = (string?)null, Status = NotaryStatus.Active, ResidentialAddress = "456 Pine Ln, Detroit, MI 48226" },
                new { UserEmail = "c.taylor@mail.com", Ssn = "777-88-9999", FullName = "Christopher Taylor", DateOfBirth = new DateOnly(1988, 7, 24), PhotoUrl = "/img/ctaylor.jpg", Phone = "(555) 777-8899", NotaryEmail = "c.taylor@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2020, 10, 10), InternalNotes = (string?)null, Status = NotaryStatus.Active, ResidentialAddress = "567 Maple St, Nashville, TN 37203" },
                new { UserEmail = "e.moore@mail.com", Ssn = "888-99-0000", FullName = "Elizabeth Moore", DateOfBirth = new DateOnly(1995, 4, 16), PhotoUrl = "/img/emoore.jpg", Phone = "(555) 888-9900", NotaryEmail = "e.moore@mail.com", EmploymentType = EmploymentType.IndependentContract, StartDate = new DateOnly(2023, 6, 25), InternalNotes = "Personal leave", Status = NotaryStatus.Inactive, ResidentialAddress = "678 Cedar Blvd, Houston, TX 77002" },
                new { UserEmail = "j.jackson@mail.com", Ssn = "999-00-1111", FullName = "Joshua Jackson", DateOfBirth = new DateOnly(1985, 9, 9), PhotoUrl = "/img/jjackson.jpg", Phone = "(555) 999-0011", NotaryEmail = "j.jackson@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2019, 8, 8), InternalNotes = (string?)null, Status = NotaryStatus.Active, ResidentialAddress = "789 Elm St, San Jose, CA 95113" },
                new { UserEmail = "lauren.m@mail.com", Ssn = "000-11-2222", FullName = "Lauren Martin", DateOfBirth = new DateOnly(1992, 1, 31), PhotoUrl = "/img/lmartin.jpg", Phone = "(555) 000-1122", NotaryEmail = "lauren.m@mail.com", EmploymentType = EmploymentType.FullTime, StartDate = new DateOnly(2021, 3, 15), InternalNotes = (string?)null, Status = NotaryStatus.Active, ResidentialAddress = "890 Birch Rd, Philadelphia, PA 19104" }
            };

            foreach (var row in notaryUsers)
            {
                // Create User
                var user = User.Create(row.UserEmail, row.UserEmail, row.FullName, notaryRole.Id);
                user.UpdateProfile(row.FullName, row.Phone, row.ResidentialAddress);
                SetPropertyValue(user, typeof(User), "DateOfBirth", row.DateOfBirth);
                context.Users.Add(user);

                var notary = Notary.Create(user.Id, row.Ssn, row.FullName, row.DateOfBirth, row.EmploymentType, row.StartDate);
                notary.UpdateStatus(row.Status);
                notary.SetInternalNotes(row.InternalNotes);
                SetPropertyValue(notary, typeof(Notary), "PhotoUrl", row.PhotoUrl);
                SetPropertyValue(notary, typeof(Notary), "Phone", row.Phone);
                SetPropertyValue(notary, typeof(Notary), "Email", row.NotaryEmail);
                SetPropertyValue(notary, typeof(Notary), "ResidentialAddress", row.ResidentialAddress);

                context.Notaries.Add(notary);
            }

            await context.SaveChangesAsync();
        }

        // Seed NotarialActs and ActSignatures
        if (!context.NotarialActs.Any())
        {
            var notaries = context.Notaries.ToList();
            var users = context.Users.ToList();

            if (notaries.Any() && users.Any())
            {
                var acts = new List<NotarialAct>();

                // Aligned with docs/csv_output/notary_acts.csv (unsupported types/statuses are mapped)
                var notarialActsData = new[]
                {
                    (1, 1, 1, "ACKNOWLEDGMENT", "COMPLETED"),
                    (2, 2, 2, "JURAT", "COMPLETED"),
                    (3, 3, 3, "LOAN_SIGNING", "PENDING"),
                    (4, 4, 4, "POWER_OF_ATTORNEY", "COMPLETED"),
                    (5, 5, 5, "COPY_CERTIFICATION", "CANCELLED"),
                    (6, 6, 1, "RON", "IN_PROGRESS"),
                    (7, 7, 6, "AFFIDAVIT", "COMPLETED"),
                    (8, 8, 7, "OATH_AFFIRMATION", "COMPLETED"),
                    (9, 9, 8, "APOSTILLE", "PENDING"),
                    (10, 10, 9, "LOAN_SIGNING", "COMPLETED"),
                    (11, 11, 10, "ACKNOWLEDGMENT", "IN_PROGRESS"),
                    (12, 12, 2, "JURAT", "COMPLETED"),
                    (13, 13, 11, "POWER_OF_ATTORNEY", "PENDING"),
                    (14, 14, 12, "RON", "COMPLETED"),
                    (15, 15, 3, "AFFIDAVIT", "CANCELLED"),
                    (16, 16, 13, "COPY_CERTIFICATION", "COMPLETED"),
                    (17, 17, 14, "LOAN_SIGNING", "IN_PROGRESS"),
                    (18, 18, 15, "OATH_AFFIRMATION", "PENDING"),
                    (19, 19, 4, "APOSTILLE", "COMPLETED"),
                    (20, 20, 1, "ACKNOWLEDGMENT", "COMPLETED")
                };

                // Ensure ServiceRequests exist so NotarialActs.request_id always points to a valid FK.
                var serviceRequests = context.ServiceRequests
                    .OrderBy(x => x.CreatedAt)
                    .ToList();

                var requiredRequestCount = notarialActsData.Max(x => x.Item2);
                if (serviceRequests.Count < requiredRequestCount)
                {
                    for (int i = serviceRequests.Count; i < requiredRequestCount; i++)
                    {
                        var customerId = users[i % users.Count].Id;
                        var request = ServiceRequest.Create(RequestStatus.Pending, customerId);
                        context.ServiceRequests.Add(request);
                        serviceRequests.Add(request);
                    }

                    await context.SaveChangesAsync();
                }

                // Create NotarialActs
                foreach (var (_, requestIndex, notaryIndex, typeText, statusText) in notarialActsData)
                {
                    if (requestIndex <= 0 || requestIndex > serviceRequests.Count)
                    {
                        continue;
                    }

                    // Map notaryIndex to actual notary (1-based index)
                    var actualNotaryIndex = ((notaryIndex - 1) % notaries.Count);
                    var notary = notaries[actualNotaryIndex];
                    var requestId = serviceRequests[requestIndex - 1].Id;
                    var type = MapNotarialActType(typeText);
                    var status = MapNotarialActStatus(statusText);

                    var act = NotarialAct.Create(
                        requestId,
                        notary.Id,       // notaryId
                        type,
                        jurisdictionId: null);

                    // Set the status
                    act.UpdateStatus(status);

                    acts.Add(act);
                    context.NotarialActs.Add(act);
                }

                await context.SaveChangesAsync();

                // Seed ActSignatures aligned with docs/csv_output/signature.csv
                var signatureData = new[]
                {
                    (1, 7, 1, "data:image/png;base64,iVBORw0KGg...", "SIGNED"),
                    (1, 1, 2, "data:image/png;base64,R0lGODlhAQ...", "SIGNED"),
                    (2, 8, 1, "data:image/png;base64,SUQzBAAAAA...", "SIGNED"),
                    (2, 2, 2, "", "PENDING"),
                    (3, 9, 1, "", "PENDING"),
                    (3, 10, 2, "", "PENDING"),
                    (3, 3, 3, "", "PENDING"),
                    (4, 11, 1, "data:image/png;base64,UklGRiQAA...", "SIGNED"),
                    (4, 4, 2, "", "DECLINED"),
                    (5, 12, 1, "data:image/png;base64,PHN2ZyB4b...", "SIGNED"),
                    (5, 13, 2, "data:image/png;base64,iVBORw0KGg...", "SIGNED"),
                    (5, 5, 3, "data:image/png;base64,R0lGODlhAQ...", "SIGNED"),
                    (6, 14, 1, "data:image/png;base64,SUQzBAAAAA...", "SIGNED"),
                    (6, 6, 2, "", "PENDING"),
                    (7, 15, 1, "", "PENDING"),
                    (7, 1, 2, "", "PENDING"),
                    (8, 16, 1, "data:image/png;base64,UklGRiQAA...", "SIGNED"),
                    (8, 2, 2, "data:image/png;base64,PHN2ZyB4b...", "SIGNED"),
                    (9, 17, 1, "", "DECLINED"),
                    (9, 3, 2, "", "PENDING")
                };

                foreach (var (actId, userId, orderIndex, sigData, statusText) in signatureData)
                {
                    var actIndex = (actId - 1) % acts.Count;
                    var userIndex = (userId - 1) % users.Count;

                    var signature = ActSignature.Create(
                        acts[actIndex].Id,
                        users[userIndex].Id,
                        orderIndex);

                    // Set status based on data
                    if (statusText.Equals("SIGNED", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(sigData))
                    {
                        signature.Sign(sigData);
                    }
                    else if (statusText.Equals("DECLINED", StringComparison.OrdinalIgnoreCase))
                    {
                        SetPropertyValue(signature, typeof(ActSignature), "Status", SignatureStatus.Rejected);
                    }

                    context.ActSignatures.Add(signature);
                }

                await context.SaveChangesAsync();
            }
        }

        // Seed ActLogEntries
        if (!context.ActLogEntries.Any())
        {
            var acts = context.NotarialActs.ToList();
            var notaries = context.Notaries.ToList();

            if (acts.Any() && notaries.Any())
            {
                // ActLogEntry timestamps aligned with docs/csv_output/LogEntry.csv
                var logTimestamps = new[]
                {
                    "2026-03-20 08:00:00",
                    "2026-03-20 08:15:00",
                    "2026-03-20 08:30:00",
                    "2026-03-20 09:00:00",
                    "2026-03-20 09:15:00",
                    "2026-03-20 09:30:00",
                    "2026-03-20 10:00:00",
                    "2026-03-20 10:15:00",
                    "2026-03-20 10:30:00",
                    "2026-03-20 11:00:00",
                    "2026-03-20 11:15:00",
                    "2026-03-20 11:30:00",
                    "2026-03-20 13:00:00",
                    "2026-03-20 13:15:00",
                    "2026-03-20 13:30:00",
                    "2026-03-20 14:00:00",
                    "2026-03-20 14:15:00",
                    "2026-03-20 14:30:00",
                    "2026-03-20 15:00:00",
                    "2026-03-20 15:15:00"
                };

                var logEntries = new List<ActLogEntry>();

                // Create 20 ActLogEntry records with timestamps
                for (int i = 0; i < 20; i++)
                {
                    var actIndex = i % acts.Count;
                    var notaryIndex = i % Math.Min(notaries.Count, 10);

                    var logEntry = ActLogEntry.Create(
                        acts[actIndex].Id,
                        notaries[notaryIndex].Id);

                    if (TryParseDateTimeOffset(logTimestamps[i], out var parsedTimestamp))
                    {
                        SetPropertyValue(logEntry, typeof(ActLogEntry), "Timestamp", parsedTimestamp);
                    }

                    logEntries.Add(logEntry);
                }

                context.ActLogEntries.AddRange(logEntries);
                await context.SaveChangesAsync();
            }
        }

        // Seed ComplianceReviews
        if (!context.ComplianceReviews.Any())
        {
            var acts = context.NotarialActs.ToList();

            if (acts.Any())
            {
                // ComplianceReview data from Excel (20 entries)
                var complianceData = new[]
                {
                    "Approved",
                    "Rejected",
                    "Approved",
                    "Pending",
                    "Approved",
                    "Rejected",
                    "Approved",
                    "Pending",
                    "Approved",
                    "Rejected",
                    "Approved",
                    "Pending",
                    "Approved",
                    "Rejected",
                    "Approved",
                    "Pending",
                    "Approved",
                    "Rejected",
                    "Approved",
                    "Pending"
                };

                var complianceReviews = new List<ComplianceReview>();

                // Create 20 ComplianceReview records
                for (int i = 0; i < 20; i++)
                {
                    var actIndex = i % acts.Count;
                    var result = complianceData[i];

                    var review = ComplianceReview.Create(
                        acts[actIndex].Id,
                        result);

                    complianceReviews.Add(review);
                }

                context.ComplianceReviews.AddRange(complianceReviews);
                await context.SaveChangesAsync();
            }
        }

        // Seed NotaryStatusHistory
        if (!context.NotaryStatusHistories.Any())
        {
            var notaries = context.Notaries.ToList();

            if (notaries.Any())
            {
                var nshData = new[]
                {
                    (1, "ACTIVE", "", "15/01/2023 8:30:00", "1"),
                    (2, "ACTIVE", "", "10/02/2023 9:15:00", "2"),
                    (3, "ACTIVE", "", "05/03/2023 10:00:00", "1"),
                    (4, "INACTIVE", "insurance has expired.", "12/03/2023 11:45:00", "1"),
                    (5, "ACTIVE", "", "01/04/2023 14:20:00", "1"),
                    (6, "INACTIVE", "license outdate.", "18/05/2023 8:00:00", "1"),
                    (7, "BLOCKED", "banned by Department of Justice", "10/06/2023 16:30:00", "2"),
                    (8, "ACTIVE", "", "15/06/2023 9:10:00", "1"),
                    (9, "ACTIVE", "", "02/07/2023 11:25:00", "1"),
                    (10, "ACTIVE", "", "20/07/2023 13:40:00", "1"),
                    (11, "ACTIVE", "", "05/08/2023 15:55:00", "1"),
                    (12, "ACTIVE", "", "22/08/2023 10:15:00", "2"),
                    (13, "ACTIVE", "", "11/09/2023 14:05:00", "1"),
                    (14, "ACTIVE", "", "30/09/2023 8:50:00", "1"),
                    (15, "BLOCKED", "banned by Department of Justice", "14/10/2023 12:30:00", "1"),
                    (16, "ACTIVE", "", "02/11/2023 16:10:00", "1"),
                    (17, "ACTIVE", "", "25/11/2023 9:45:00", "2"),
                    (18, "INACTIVE", "insurance has expired.", "10/12/2023 11:20:00", "1"),
                    (19, "ACTIVE", "", "05/01/2024 15:00:00", "1"),
                    (20, "ACTIVE", "", "20/01/2024 10:35:00", "1")
                };

                var list = new List<NotaryStatusHistory>();

                foreach (var (notaryIndex, statusText, reason, tsText, createdBy) in nshData)
                {
                    if (notaryIndex <= 0 || notaryIndex > notaries.Count)
                        continue;

                    var notary = notaries[notaryIndex - 1];

                    var status = statusText.ToUpperInvariant() switch
                    {
                        "ACTIVE" => NotaryStatus.Active,
                        "INACTIVE" => NotaryStatus.Inactive,
                        "BLOCKED" => NotaryStatus.Blocked,
                        _ => NotaryStatus.Active
                    };

                    var effectiveDate = ParseDateOnly(tsText);

                    var nsh = NotaryStatusHistory.Create(notary.Id, status, string.IsNullOrWhiteSpace(reason) ? null : reason, effectiveDate, createdBy);
                    list.Add(nsh);
                }

                context.NotaryStatusHistories.AddRange(list);
                await context.SaveChangesAsync();
            }
        }

        // Seed Jobs and JobStatusLogs
        if (!context.Jobs.Any())
        {
            var users = context.Users.ToList();

            if (users.Any())
            {
                var jobs = new List<Job>();

                var jobData = new[]
                {
                    ("Notarization", "Office", "123 Nguyen Van Linh, Da Nang", "2026-03-21 08:00:00", "2026-03-21 09:00:00", 2, "Pending"),
                    ("Translation", "Home", "45 Le Duan, Da Nang", "2026-03-21 09:30:00", "2026-03-21 10:30:00", 1, "Assigned"),
                    ("Certification", "Office", "78 Tran Phu, Da Nang", "2026-03-21 10:00:00", "2026-03-21 11:00:00", 3, "Completed"),
                    ("Notarization", "Home", "22 Hoang Dieu, Da Nang", "2026-03-21 13:00:00", "2026-03-21 14:00:00", 2, "Pending"),
                    ("Translation", "Office", "90 Bach Dang, Da Nang", "2026-03-21 14:30:00", "2026-03-21 15:30:00", 1, "Cancelled"),
                    ("Certification", "Home", "15 Hai Phong, Da Nang", "2026-03-22 08:00:00", "2026-03-22 09:00:00", 4, "Assigned"),
                    ("Notarization", "Office", "200 Nguyen Chi Thanh, Da Nang", "2026-03-22 09:00:00", "2026-03-22 10:00:00", 2, "Completed"),
                    ("Translation", "Home", "55 Phan Chau Trinh, Da Nang", "2026-03-22 10:30:00", "2026-03-22 11:30:00", 1, "Pending"),
                    ("Certification", "Office", "300 Le Loi, Da Nang", "2026-03-22 13:00:00", "2026-03-22 14:00:00", 2, "Assigned"),
                    ("Notarization", "Home", "12 Ong Ich Khiem, Da Nang", "2026-03-22 14:30:00", "2026-03-22 15:30:00", 3, "Completed"),
                    ("Translation", "Office", "66 Hung Vuong, Da Nang", "2026-03-23 08:00:00", "2026-03-23 09:00:00", 1, "Pending"),
                    ("Certification", "Home", "99 Ly Thai To, Da Nang", "2026-03-23 09:30:00", "2026-03-23 10:30:00", 2, "Assigned"),
                    ("Notarization", "Office", "150 Ton Duc Thang, Da Nang", "2026-03-23 10:00:00", "2026-03-23 11:00:00", 2, "Completed"),
                    ("Translation", "Home", "88 Nguyen Tat Thanh, Da Nang", "2026-03-23 13:00:00", "2026-03-23 14:00:00", 1, "Cancelled"),
                    ("Certification", "Office", "10 Vo Nguyen Giap, Da Nang", "2026-03-23 14:30:00", "2026-03-23 15:30:00", 3, "Pending"),
                    ("Notarization", "Home", "45 Pham Van Dong, Da Nang", "2026-03-24 08:00:00", "2026-03-24 09:00:00", 2, "Assigned"),
                    ("Translation", "Office", "77 Nguyen Huu Tho, Da Nang", "2026-03-24 09:30:00", "2026-03-24 10:30:00", 1, "Completed"),
                    ("Certification", "Home", "23 Tran Hung Dao, Da Nang", "2026-03-24 10:00:00", "2026-03-24 11:00:00", 2, "Pending"),
                    ("Notarization", "Office", "11 Phan Dang Luu, Da Nang", "2026-03-24 13:00:00", "2026-03-24 14:00:00", 3, "Assigned"),
                    ("Translation", "Home", "5 Ngo Quyen, Da Nang", "2026-03-24 14:30:00", "2026-03-24 15:30:00", 1, "Completed")
                };

                for (int i = 0; i < jobData.Length; i++)
                {
                    var (serviceText, locationText, locationDetails, startText, endText, signerCount, statusText) = jobData[i];
                    var client = users[i % users.Count];

                    if (!TryParseDateTimeOffset(startText, out var requestedStart))
                    {
                        requestedStart = DateTimeOffset.UtcNow;
                    }

                    if (!TryParseDateTimeOffset(endText, out var requestedEnd))
                    {
                        requestedEnd = requestedStart.AddHours(1);
                    }

                    var job = Job.Create(
                        client.Id,
                        MapServiceType(serviceText),
                        MapLocationType(locationText),
                        locationDetails,
                        requestedStart,
                        requestedEnd,
                        signerCount);

                    job.UpdateStatus(MapJobStatus(statusText));

                    jobs.Add(job);
                    context.Jobs.Add(job);
                }

                await context.SaveChangesAsync();

                // JobStatusLog data aligned with docs/csv_output/job_status_logs.csv
                var jobStatusLogData = new[]
                {
                    ("Pending", "2023-01-15 08:30:00", "", ""),
                    ("Assigned", "2023-01-16 08:30:00", "", ""),
                    ("Completed", "2023-01-17 08:30:00", "2h", "signer come later"),
                    ("Pending", "2023-01-15 08:30:00", "", ""),
                    ("Cancelled", "2023-01-16 08:30:00", "", "client cancel service"),
                    ("Pending", "2023-01-11 08:30:00", "", ""),
                    ("Assigned", "2023-01-15 08:30:00", "", ""),
                    ("Completed", "2023-01-16 08:30:00", "", ""),
                    ("Pending", "2023-01-11 08:30:00", "", ""),
                    ("Assigned", "2023-01-15 08:30:00", "", ""),
                    ("Completed", "2023-01-16 08:30:00", "", ""),
                    ("Pending", "2023-01-11 08:30:00", "", ""),
                    ("Assigned", "2023-01-15 08:30:00", "", ""),
                    ("Completed", "2023-01-16 08:30:00", "", ""),
                    ("Pending", "2023-01-11 08:30:00", "", ""),
                    ("Assigned", "2023-01-15 08:30:00", "", ""),
                    ("Completed", "2023-01-16 08:30:00", "1d", "customer change sign place"),
                    ("Pending", "2023-01-11 08:30:00", "", ""),
                    ("Assigned", "2023-01-15 08:30:00", "", ""),
                    ("Completed", "2023-01-16 08:30:00", "", "")
                };

                var jobStatusLogs = new List<Domain.Entities.Jobs.JobStatusLog>();

                // Create 20 JobStatusLog records
                for (int i = 0; i < 20; i++)
                {
                    var jobIndex = i % jobs.Count;
                    var (statusText, timestamp, delay, note) = jobStatusLogData[i];

                    var log = Domain.Entities.Jobs.JobStatusLog.Create(
                        jobs[jobIndex].Id,
                        MapJobStatus(statusText),
                        string.IsNullOrEmpty(note) ? null : note);

                    // Set additional properties using reflection
                    // Parse timestamp - format: "15/01/2023 8:30:00" or "16/01/2023 08:30:00"
                    if (TryParseDateTimeOffset(timestamp, out var parsedTimestamp))
                    {
                        SetPropertyValue(log, typeof(Domain.Entities.Jobs.JobStatusLog), "Timestamp", parsedTimestamp);
                    }

                    if (!string.IsNullOrEmpty(delay))
                    {
                        SetPropertyValue(log, typeof(Domain.Entities.Jobs.JobStatusLog), "Delay", delay);
                    }

                    jobStatusLogs.Add(log);
                }

                context.JobStatusLogs.AddRange(jobStatusLogs);
                await context.SaveChangesAsync();
            }
        }

        // Seed NotaryIncidents
        if (!context.NotaryIncidents.Any())
        {
            var notaries = context.Notaries.ToList();

            if (notaries.Any())
            {
                var incidentsData = new[]
                {
                    (7, "POLICY_VIOLATION", "Customer reports regarding unauthorized fee charges", "CRITICAL", "UNDER_REVIEW", ""),
                    (15, "CUSTOMER_COMPLAINT", "Customers complain about unprofessional service.", "HIGH", "OPEN", ""),
                    (1, "LATE_ARRIVAL", "I arrived 15 minutes late due to traffic, but I had notified them in advance", "LOW", "DISMISSED", "2022-08-15 10:30:00"),
                    (3, "DOCUMENT_ERROR", "Missing signature on page 4 of the power of attorney agreement.", "MEDIUM", "RESOLVED", "2023-05-20 14:00:00"),
                    (15, "NO_SHOW", "Do not show up at the notary office without prior notice.", "CRITICAL", "UNDER_REVIEW", ""),
                    (8, "COMPLIANCE_ISSUE", "The notarized stamp is unclear and needs to be redone.", "MEDIUM", "RESOLVED", "2023-01-10 16:45:00"),
                    (10, "SYSTEM_ISSUE", "System error: Unable to load e-Notary documents", "LOW", "RESOLVED", "2022-11-05 09:15:00"),
                    (7, "POLICY_VIOLATION", "Failing to carefully check the ID of the signatory.", "CRITICAL", "OPEN", ""),
                    (18, "CUSTOMER_COMPLAINT", "The client was dissatisfied with the notary's attire.", "LOW", "DISMISSED", "2023-08-20 11:00:00"),
                    (12, "DOCUMENT_ERROR", "Incorrect date entered on the certificate", "MEDIUM", "RESOLVED", "2024-03-05 15:30:00"),
                    (4, "LATE_ARRIVAL", "Arriving 30 minutes late for the appointment", "LOW", "RESOLVED", "2022-04-12 10:00:00"),
                    (6, "COMPLIANCE_ISSUE", "State license renewal delayed", "HIGH", "OPEN", ""),
                    (13, "SYSTEM_ISSUE", "Forgot the portal login password", "LOW", "RESOLVED", "2020-05-18 08:30:00"),
                    (9, "CUSTOMER_COMPLAINT", "Client complains about overly complicated explanation process", "LOW", "DISMISSED", "2024-01-20 14:20:00"),
                    (2, "DOCUMENT_ERROR", "Forgot to bring the embosser", "MEDIUM", "RESOLVED", "2022-07-11 09:45:00"),
                    (11, "LATE_ARRIVAL", "Traffic jam due to a highway accident", "LOW", "RESOLVED", "2021-09-15 11:10:00"),
                    (19, "COMPLIANCE_ISSUE", "Notary journal lacks detail in entries", "MEDIUM", "RESOLVED", "2020-10-05 16:00:00"),
                    (5, "CUSTOMER_COMPLAINT", "Incorrectly calculated travel service fee", "MEDIUM", "RESOLVED", "2021-12-02 13:30:00"),
                    (16, "DOCUMENT_ERROR", "Mixed documents between two clients", "HIGH", "UNDER_REVIEW", ""),
                    (14, "SYSTEM_ISSUE", "Tablet runs out of battery during the signing session", "LOW", "DISMISSED", "2024-02-15 10:05:00")
                };

                var incidents = new List<NotaryIncident>();

                foreach (var (notaryIndex, type, description, severityText, statusText, resolvedAtText) in incidentsData)
                {
                    if (notaryIndex <= 0 || notaryIndex > notaries.Count)
                        continue;

                    var notary = notaries[notaryIndex - 1];

                    var severity = severityText.ToUpperInvariant() switch
                    {
                        "CRITICAL" => SeverityLevel.Critical,
                        "HIGH" => SeverityLevel.High,
                        "MEDIUM" => SeverityLevel.Medium,
                        "LOW" => SeverityLevel.Low,
                        _ => SeverityLevel.Low
                    };

                    var status = statusText.ToUpperInvariant() switch
                    {
                        "OPEN" => IncidentStatus.Open,
                        "UNDER_REVIEW" => IncidentStatus.UnderReview,
                        "RESOLVED" => IncidentStatus.Resolved,
                        "DISMISSED" => IncidentStatus.Dismissed,
                        _ => IncidentStatus.Open
                    };

                    var incident = NotaryIncident.Create(notary.Id, type, description, severity);

                    // Set status using reflection if not default Open
                    if (status != IncidentStatus.Open)
                    {
                        SetPropertyValue(incident, typeof(NotaryIncident), "Status", status);
                    }

                    // Parse and set ResolvedAt if provided
                    if (!string.IsNullOrWhiteSpace(resolvedAtText) && TryParseDateTimeOffset(resolvedAtText, out var resolvedAtValue))
                    {
                        SetPropertyValue(incident, typeof(NotaryIncident), "ResolvedAt", resolvedAtValue);
                    }

                    incidents.Add(incident);
                }

                context.NotaryIncidents.AddRange(incidents);
                await context.SaveChangesAsync();
            }
        }

        // Seed SecurityIncidents from docs/csv_output/incidents.csv
        if (!context.SecurityIncidents.Any())
        {
            var users = context.Users.ToList();
            var seals = context.Seals.ToList();
            var certificates = context.Certificates.ToList();

            if (users.Any())
            {
                var rows = LoadSimpleCsvRows("incidents.csv");
                var incidents = new List<SecurityIncident>();

                foreach (var row in rows)
                {
                    if (row.Length < 7)
                    {
                        continue;
                    }

                    var title = row[1].Trim();
                    var description = row[2].Trim();

                    var reportedByIndex = ParseCsvOneBasedIndex(row[3]);
                    if (reportedByIndex is null || reportedByIndex < 1 || reportedByIndex > users.Count)
                    {
                        continue;
                    }

                    if (!TryParseDateTimeOffset(row[4], out var reportedAt))
                    {
                        reportedAt = DateTimeOffset.UtcNow;
                    }

                    var sealIndex = ParseCsvOneBasedIndex(row[5]);
                    var certificateIndex = ParseCsvOneBasedIndex(row[6]);

                    var sealId = sealIndex is not null && sealIndex >= 1 && sealIndex <= seals.Count
                        ? seals[sealIndex.Value - 1].Id
                        : (Guid?)null;

                    var certificateId = certificateIndex is not null && certificateIndex >= 1 && certificateIndex <= certificates.Count
                        ? certificates[certificateIndex.Value - 1].Id
                        : (Guid?)null;

                    var incident = SecurityIncident.Create(
                        title,
                        description,
                        users[reportedByIndex.Value - 1].Id,
                        reportedAt,
                        sealId,
                        certificateId);

                    incidents.Add(incident);
                }

                if (incidents.Count > 0)
                {
                    context.SecurityIncidents.AddRange(incidents);
                    await context.SaveChangesAsync();
                }
            }
        }
    }

    private static string[][] LoadSimpleCsvRows(string fileName)
    {
        var csvPath = ResolveCsvPath(fileName);
        if (csvPath is null)
        {
            return [];
        }

        return File.ReadAllLines(csvPath)
            .Skip(1)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(ParseSimpleCsvLine)
            .Where(columns => columns.Length > 0)
            .ToArray();
    }

    private static string[] ParseSimpleCsvLine(string line)
    {
        var values = new List<string>();
        var current = new System.Text.StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var ch = line[i];

            if (ch == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }

                continue;
            }

            if (ch == ',' && !inQuotes)
            {
                values.Add(current.ToString().Trim());
                current.Clear();
                continue;
            }

            current.Append(ch);
        }

        values.Add(current.ToString().Trim());
        return values.ToArray();
    }

    private static string? ResolveCsvPath(string fileName)
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null)
        {
            var candidate = Path.Combine(current.FullName, "docs", "csv_output", fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            current = current.Parent;
        }

        return null;
    }

    private static NotarialActType MapNotarialActType(string csvType) => csvType.ToUpperInvariant() switch
    {
        "ACKNOWLEDGMENT" => NotarialActType.Acknowledgment,
        "JURAT" => NotarialActType.Jurat,
        "COPY_CERTIFICATION" => NotarialActType.CopyCertification,
        "OATH_AFFIRMATION" => NotarialActType.Oath,
        "AFFIDAVIT" => NotarialActType.Affirmation,
        "POWER_OF_ATTORNEY" => NotarialActType.Acknowledgment,
        "LOAN_SIGNING" => NotarialActType.Jurat,
        "RON" => NotarialActType.Oath,
        "APOSTILLE" => NotarialActType.CopyCertification,
        _ => NotarialActType.Acknowledgment
    };

    private static NotarialActStatus MapNotarialActStatus(string csvStatus) => csvStatus.ToUpperInvariant() switch
    {
        "COMPLETED" => NotarialActStatus.Completed,
        "IN_PROGRESS" => NotarialActStatus.InProgress,
        "PENDING" => NotarialActStatus.Draft,
        "CANCELLED" => NotarialActStatus.Voided,
        _ => NotarialActStatus.Draft
    };

    private static ServiceType MapServiceType(string csvServiceType) => csvServiceType.ToUpperInvariant() switch
    {
        "NOTARIZATION" => ServiceType.MobileNotary,
        "TRANSLATION" => ServiceType.General,
        "CERTIFICATION" => ServiceType.Apostille,
        _ => ServiceType.General
    };

    private static LocationType MapLocationType(string csvLocationType) => csvLocationType.ToUpperInvariant() switch
    {
        "OFFICE" => LocationType.Onsite,
        "HOME" => LocationType.Mobile,
        _ => LocationType.Online
    };

    private static JobStatus MapJobStatus(string csvStatus) => csvStatus.ToUpperInvariant() switch
    {
        "PENDING" => JobStatus.New,
        "ASSIGNED" => JobStatus.Scheduled,
        "COMPLETED" => JobStatus.Completed,
        "CANCELLED" => JobStatus.Cancelled,
        _ => JobStatus.New
    };

    private static DateOnly ParseDateOnly(string value)
    {
        if (TryParseDateTimeOffset(value, out var dto))
        {
            return DateOnly.FromDateTime(dto.DateTime);
        }

        return DateOnly.FromDateTime(DateTime.UtcNow);
    }

    private static bool TryParseDateTimeOffset(string value, out DateTimeOffset result)
    {
        var formats = new[]
        {
            "d/M/yyyy H:mm:ss",
            "dd/MM/yyyy H:mm:ss",
            "dd/MM/yyyy HH:mm:ss",
            "yyyy-MM-dd H:mm:ss",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ssK"
        };

        if (DateTimeOffset.TryParseExact(
                value,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces,
                out result))
        {
            return true;
        }

        return DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out result);
    }

    private static int? ParseCsvOneBasedIndex(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        text = text.Trim();

        if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
        {
            return intValue;
        }

        if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue))
        {
            return (int)Math.Round(doubleValue);
        }

        return null;
    }

    private static void SetPropertyValue(object target, Type targetType, string propertyName, object? value)
    {
        var property = targetType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        property?.SetValue(target, value);
    }
}
