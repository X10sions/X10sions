using System.ComponentModel.DataAnnotations;

namespace Common.Features.DummyFakeExamples.Auth;

public record ForgotPasswordRequest([Required, EmailAddress] string Email);
