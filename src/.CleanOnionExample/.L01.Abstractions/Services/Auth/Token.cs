using System.Text.Json.Serialization;

namespace CleanOnionExample.Services.Auth;

public class Token {
  public record Request(string Email, string Password);

  public record Response(string Id,
    string UserName,
     string Email,
     List<string> Roles,
     bool IsVerified,
     string JWToken,
     DateTime IssuedOn,
     DateTime ExpiresOn) {
    [JsonIgnore] public string RefreshToken { get; set; }
  };
}
