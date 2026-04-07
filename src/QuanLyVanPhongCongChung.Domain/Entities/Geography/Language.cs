namespace QuanLyVanPhongCongChung.Domain.Entities.Geography;

using QuanLyVanPhongCongChung.Domain.Common;

public class Language : BaseEntity
{
    public string LangCode { get; private set; } = null!;
    public string LangName { get; private set; } = null!;

    private Language() { }

    public static Language Create(string langCode, string langName)
    {
        return new Language
        {
            LangCode = langCode,
            LangName = langName
        };
    }
}
