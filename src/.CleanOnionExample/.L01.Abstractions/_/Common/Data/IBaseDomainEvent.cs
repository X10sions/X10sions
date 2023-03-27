namespace Common.Data;

public interface IBaseDomainEvent : INotification {
  DateTime DateOccurred { get;  }
}

public abstract record BaseDomainEvent : IBaseDomainEvent {
  public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}