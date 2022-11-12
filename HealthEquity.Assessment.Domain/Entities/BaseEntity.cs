using HealthEquity.Assessment.Domain.DomainEvents;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HealthEquity.Assessment.Domain.Entities;
public abstract class BaseEntity
{
    [JsonInclude]
    public long Id { get; protected set; }

    [NotMapped]
    public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();

    public void QueueDomainEvent(IDomainEvent @event)
    {
        DomainEvents.Add(@event);
    }
}
