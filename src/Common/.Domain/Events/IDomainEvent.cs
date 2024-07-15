using MediatR;

namespace Common.Domain.Events;

public interface IDomainEvent : INotification {
  DateTime DateOccurred { get; }
  bool IsPublished { get; set; }
}
