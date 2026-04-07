namespace QuanLyVanPhongCongChung.Domain.Entities.Identity;

using QuanLyVanPhongCongChung.Domain.Common;

public class Role : BaseEntity
{
    public string RoleName { get; private set; } = null!;

    private readonly List<User> _users = [];
    public IReadOnlyCollection<User> Users => _users.AsReadOnly();

    private Role() { }

    public static Role Create(string roleName)
    {
        return new Role { RoleName = roleName };
    }
}
