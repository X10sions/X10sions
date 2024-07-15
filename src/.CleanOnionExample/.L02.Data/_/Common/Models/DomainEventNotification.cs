using Common.Domain.Events;
using MediatR;

namespace Common.Models;

public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : DomainEventBase, INotification {
  public DomainEventNotification(TDomainEvent domainEvent) {
    DomainEvent = domainEvent;
  }

  public TDomainEvent DomainEvent { get; }
}
