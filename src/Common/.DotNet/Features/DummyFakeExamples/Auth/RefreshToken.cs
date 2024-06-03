namespace Common.Features.DummyFakeExamples.Auth;

public record RefreshToken(string Token, DateTime Expires, DateTime Created, string CreatedByIp) {
  public int Id { get; set; }
  public DateTime? Revoked { get; set; }
  public string RevokedByIp { get; set; }
  public string ReplacedByToken { get; set; }
  public bool IsExpired => DateTime.UtcNow >= Expires;
  public bool IsActive => Revoked == null && !IsExpired;
};
