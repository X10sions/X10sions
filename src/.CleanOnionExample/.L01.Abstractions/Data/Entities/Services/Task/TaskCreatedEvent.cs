namespace CleanOnionExample.Data.Entities.Services;

public record TaskCreatedEvent(Guid Id, string Description, string Summary);

