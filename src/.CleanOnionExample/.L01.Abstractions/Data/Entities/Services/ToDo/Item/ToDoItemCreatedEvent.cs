namespace CleanOnionExample.Data.Entities.Services;

public record ToDoItemCreatedEvent(Guid Id, string Description, string Summary);

