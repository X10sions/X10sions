namespace CleanOnionExample.Data.Entities.Services;

public class ToDoItemEventHandler {
  public async Task HandleTaskCreatedEvent(ToDoItemCreatedEvent taskCreatedEvent) {
    // Here you can do whatever you need with this event, you can propagate the data using a queue, calling another API or sending a notification or whatever
    // With this scenario, you are building a event driven architecture with microservices and DDD
  }

  public async Task HandleTaskDeletedEvent(ToDoItemDeletedEvent taskDeletedEvent) {
    // Here you can do whatever you need with this event, you can propagate the data using a queue, calling another API or sending a notification or whatever
    // With this scenario, you are building a event driven architecture with microservices and DDD
  }
}