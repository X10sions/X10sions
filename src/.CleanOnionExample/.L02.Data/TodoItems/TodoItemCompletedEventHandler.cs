using CleanOnionExample.Data.Entities;
using Common.Models;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace CleanOnionExample.Application.TodoItem;

public class TodoItemCompletedEventHandler : INotificationHandler<DomainEventNotification<ToDoItem.CompletedEvent>> {
  private readonly ILogger<TodoItemCompletedEventHandler> _logger;

  public TodoItemCompletedEventHandler(ILogger<TodoItemCompletedEventHandler> logger) {
    _logger = logger;
  }

  public Task Handle(DomainEventNotification<ToDoItem.CompletedEvent> notification, CancellationToken cancellationToken) {
    var domainEvent = notification.DomainEvent;
    _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);
    return Task.CompletedTask;
  }
}
