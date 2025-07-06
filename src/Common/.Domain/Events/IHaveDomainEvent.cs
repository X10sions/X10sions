namespace Common.Domain.Events;

public interface IHaveDomainEvent {
  public List<IDomainEvent> DomainEvents { get; set; }
}
