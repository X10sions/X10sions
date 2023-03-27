using Common.Events;

namespace CleanOnionExample.Services;

public interface IDomainEventService {
  Task Publish(DomainEvent domainEvent);
}
