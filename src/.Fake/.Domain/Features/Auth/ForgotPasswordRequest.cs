using System.ComponentModel.DataAnnotations;

namespace X10sions.Fake.Features.Auth;

public record ForgotPasswordRequest([Required, EmailAddress] string Email);
