namespace HealthEquity.Assessment.Domain.DomainEvents;

public record ShowSuccessMessageEvent(string Message) : IDomainEvent;
