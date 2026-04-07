namespace QuanLyVanPhongCongChung.Domain.Entities.Identity;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class User : BaseAuditableEntity, IAggregateRoot
{
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string? PhoneNumber { get; private set; }
    public UserStatus Status { get; private set; }
    public string FullName { get; private set; } = null!;
    public DateOnly? DateOfBirth { get; private set; }
    public string? Address { get; private set; }
    public Guid RoleId { get; private set; }

    public Role Role { get; private set; } = null!;

    private User() { }

    public static User Create(string email, string passwordHash, string fullName, Guid roleId)
    {
        return new User
        {
            Email = email,
            PasswordHash = passwordHash,
            FullName = fullName,
            RoleId = roleId,
            Status = UserStatus.Active
        };
    }

    public void UpdateStatus(UserStatus status) => Status = status;
    public void UpdateProfile(string fullName, string? phoneNumber, string? address)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Address = address;
    }
}
