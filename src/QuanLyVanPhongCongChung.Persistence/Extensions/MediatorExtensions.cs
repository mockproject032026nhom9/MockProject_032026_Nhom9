namespace QuanLyVanPhongCongChung.Persistence.Extensions;

using MediatR;
using Microsoft.EntityFrameworkCore;
using QuanLyVanPhongCongChung.Domain.Common;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}
