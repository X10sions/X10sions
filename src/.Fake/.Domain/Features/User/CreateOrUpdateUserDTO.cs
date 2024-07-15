namespace X10sions.Fake.Features.User;

public record CreateOrUpdateUserDTO(string EmailAddress, string Password, UserRole.Enum Role);
