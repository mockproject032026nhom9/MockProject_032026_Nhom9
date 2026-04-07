namespace QuanLyVanPhongCongChung.Domain.Entities.Geography;

using QuanLyVanPhongCongChung.Domain.Common;

public class State : BaseEntity
{
    public string StateCode { get; private set; } = null!;
    public string StateName { get; private set; } = null!;

    private State() { }

    public static State Create(string stateCode, string stateName)
    {
        return new State
        {
            StateCode = stateCode,
            StateName = stateName
        };
    }
}
