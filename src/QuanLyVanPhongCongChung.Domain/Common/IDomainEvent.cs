namespace QuanLyVanPhongCongChung.Domain.Common;

public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
}
