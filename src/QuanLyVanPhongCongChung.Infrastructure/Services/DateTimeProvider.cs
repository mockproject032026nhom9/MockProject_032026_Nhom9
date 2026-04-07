namespace QuanLyVanPhongCongChung.Infrastructure.Services;

using QuanLyVanPhongCongChung.Application.Common.Interfaces;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
