namespace Common.Data;

public interface IDomainEventBase : INotification {
  DateTime DateOccurred { get; }
}

public abstract record DomainEventBase : IDomainEventBase {
  public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}