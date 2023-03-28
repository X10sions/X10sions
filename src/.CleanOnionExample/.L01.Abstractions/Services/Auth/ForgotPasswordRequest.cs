using System.ComponentModel.DataAnnotations;

namespace CleanOnionExample.Services.Auth;
public record ForgotPasswordRequest([Required, EmailAddress] string Email);
