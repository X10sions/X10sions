using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using X10sions.Fake.Features.ToDo.Item;

namespace CleanOnionExample.Data.Entities.Services;

public class ToDoItemCompletedEventHandler : INotificationHandler<DomainEventNotification<ToDoItemCompletedEvent>> {
  private readonly ILogger<ToDoItemCompletedEventHandler> _logger;

  public ToDoItemCompletedEventHandler(ILogger<ToDoItemCompletedEventHandler> logger) {
    _logger = logger;
  }

  public Task Handle(DomainEventNotification<ToDoItemCompletedEvent> notification, CancellationToken cancellationToken) {
    var domainEvent = notification.DomainEvent;
    _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);
    return Task.CompletedTask;
  }

}
