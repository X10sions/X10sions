namespace CleanOnionExample.Services.Auth;

public record AuthTokenDTO(string AccessToken, int ExpiresIn);