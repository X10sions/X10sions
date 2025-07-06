namespace Common.Domain.Events;

public abstract record DomainEventBase : IDomainEvent {
  public bool IsPublished { get; set; }
  public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
  //public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
}