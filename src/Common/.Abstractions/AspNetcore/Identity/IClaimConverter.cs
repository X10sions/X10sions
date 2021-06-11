using System.Security.Claims;

namespace Common.AspNetCore.Identity {
  public interface IClaimConverter {
    void InitializeFromClaim(Claim other);
    Claim ToClaim();
    string ClaimType { get; set; }
    string ClaimValue { get; set; }
  }
}