using System.ComponentModel.DataAnnotations;

namespace Common.Features.DummyFakeExamples.Auth;

public record ResetPasswordRequest([Required][EmailAddress] string Email, [Required] string Token, [Required][MinLength(6)] string Password) {
  [Required][Compare(nameof(Password))] public string ConfirmPassword { get; set; }
};
