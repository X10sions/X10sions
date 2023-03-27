using CleanOnionExample.Data.Entities;
using Common.Models;
using Microsoft.Extensions.Logging;

namespace CleanOnionExample.Application.TodoItems;

public class TodoItemCreatedEventHandler : INotificationHandler<DomainEventNotification<ToDoItem.CreatedEvent>> {
  private readonly ILogger<TodoItemCreatedEventHandler> _logger;

  public TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger) {
    _logger = logger;
  }

  public System.Threading.Tasks.Task Handle(DomainEventNotification<ToDoItem.CreatedEvent> notification, CancellationToken cancellationToken) {
    var domainEvent = notification.DomainEvent;
    _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);
    return System.Threading.Tasks.Task.CompletedTask;
  }
}
