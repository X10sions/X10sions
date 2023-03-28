using System.ComponentModel.DataAnnotations;

namespace CleanOnionExample.Services.Auth;

public record RegisterRequest(
  [Required] string FirstName,
  [Required] string LastName,
  [Required][EmailAddress] string Email,
  [Required][MinLength(6)] string UserName,
  [Required][MinLength(6)] string Password) {
  [Required, Compare(nameof(Password))] public string ConfirmPassword { get; set; } = string.Empty;
}