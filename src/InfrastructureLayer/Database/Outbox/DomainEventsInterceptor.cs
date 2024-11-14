using DomainLayer.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace InfrastructureLayer.Database.Outbox;

public sealed class DomainEventsInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {

        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var events = dbContext.ChangeTracker
              .Entries<AggregateRoot>()
              .Select(x => x.Entity)
              .SelectMany(aggregateRoot =>
              {
                  var domainEvents = aggregateRoot.GetDomainEvents();

                  aggregateRoot.ClearDomainEvents();

                  return domainEvents;
              })
              .Select(x => new OutboxMessageWrapper
              {
                  Payload = JsonConvert.SerializeObject(x),
                  MessageType = x.GetType().AssemblyQualifiedName!,
              })
              .ToList();

        var bus = dbContext.GetService<IPublishEndpoint>();
        bus.PublishBatch(events, cancellationToken);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
