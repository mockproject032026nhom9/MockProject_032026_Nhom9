namespace QuanLyVanPhongCongChung.Persistence.Extensions;

using Microsoft.EntityFrameworkCore;
using System.Text;

public static class ModelBuilderExtensions
{
    public static void ApplySqlServerNamingConventions(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (string.IsNullOrWhiteSpace(tableName))
                continue;

            entityType.SetTableName(ToPascalCase(tableName));
            entityType.SetSchema("dbo");
        }
    }

    private static string ToPascalCase(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        var buffer = new StringBuilder(value.Length);
        var capitalizeNext = true;

        foreach (var ch in value)
        {
            if (!char.IsLetterOrDigit(ch))
            {
                capitalizeNext = true;
                continue;
            }

            buffer.Append(capitalizeNext ? char.ToUpperInvariant(ch) : ch);
            capitalizeNext = false;
        }

        return buffer.Length == 0 ? value : buffer.ToString();
    }
}
