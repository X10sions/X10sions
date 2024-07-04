using Common.Data;
using Common.Domain;

namespace Common.Models;

public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : DomainEventBase, MediatR.INotification {
  public DomainEventNotification(TDomainEvent domainEvent) {
    DomainEvent = domainEvent;
  }

  public TDomainEvent DomainEvent { get; }
}
