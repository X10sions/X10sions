using Common.Data;

namespace Common.Domain;

public interface IDomainEvent : INotification {
  DateTime DateOccurred { get; }
  bool IsPublished { get; set; }
}
public interface IHaveDomainEvent {
  public List<IDomainEvent> DomainEvents { get; set; }
}

public interface IDomainEventService {
  Task Publish(IDomainEvent domainEvent);
}

public abstract record DomainEventBase : IDomainEvent {
  public bool IsPublished { get; set; }
  public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
  //public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
}