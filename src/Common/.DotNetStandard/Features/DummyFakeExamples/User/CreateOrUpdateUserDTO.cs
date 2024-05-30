namespace Common.Features.DummyFakeExamples.User;

public record CreateOrUpdateUserDTO(string EmailAddress, string Password, UserRole.Enum Role);
