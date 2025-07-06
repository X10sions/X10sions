namespace X10sions.Fake.Features.Auth;

public class JWTSettings {
  public string Audience { get; set; }
  public double DurationInMinutes { get; set; } = 10;
  public int ExpiryInMinutes { get; set; } = 10;
  public string Key { get; set; }
  public string Issuer { get; set; }
}