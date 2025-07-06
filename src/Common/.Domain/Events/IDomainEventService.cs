namespace Common.Domain.Events;

public interface IDomainEventService {
  Task Publish(IDomainEvent domainEvent);
}
