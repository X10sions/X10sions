namespace CleanOnionExample.Data.Entities.Services;

public record CreateOrUpdateUserDTO(string EmailAddress, string Password, UserRole.Enum Role);
