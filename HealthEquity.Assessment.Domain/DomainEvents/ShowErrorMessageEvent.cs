namespace HealthEquity.Assessment.Domain.DomainEvents;

public record ShowErrorMessageEvent(string Message) : IDomainEvent;
