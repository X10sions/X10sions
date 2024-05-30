using Common.Features.DummyFakeExamples.ToDo.Item;
using Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanOnionExample.Data.Entities.Services;

public class ToDoItemCreatedEventHandler : INotificationHandler<DomainEventNotification<ToDoItemCreatedEvent>> {
  private readonly ILogger<ToDoItemCreatedEventHandler> _logger;

  public TodoItemCreatedEventHandler(ILogger<ToDoItemCreatedEventHandler> logger) {
    _logger = logger;
  }

  public Task Handle(DomainEventNotification<ToDoItemCreatedEvent> notification, CancellationToken cancellationToken) {
    var domainEvent = notification.DomainEvent;
    _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);
    return Task.CompletedTask;
  }
}
