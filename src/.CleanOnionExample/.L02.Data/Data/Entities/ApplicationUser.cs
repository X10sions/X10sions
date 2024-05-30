using Common.Features.DummyFakeExamples.Auth;
using Microsoft.AspNetCore.Identity;

namespace CleanOnionExample.Data.Entities;
public class ApplicationUser : IdentityUser {
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public List<RefreshToken> RefreshTokens { get; set; }
  public bool OwnsToken(string token) => RefreshTokens?.Find(x => x.Token == token) != null;
}