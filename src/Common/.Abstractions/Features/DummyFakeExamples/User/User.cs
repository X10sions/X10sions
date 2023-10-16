using Common.Data.Entities;

namespace Common.Features.DummyFakeExamples.User;

public class User : EntityBase<int> {
  public string EmailAddress { get; set; }
  public UserRole.Enum Role { get; set; }
  public string Password { get; set; }
}

public static class UserRole {
  public enum Enum {
    Admin = 2,
    Basic = 1,
    Moderator = 4,
    Owner = 5,
    SuperAdmin = 3
  }

  //public const string Base = nameof(Base);
  //public const string Admin = nameof(Admin);
  //public const string Owner = nameof(Owner);

  public static readonly string Admin = Guid.NewGuid().ToString();
  public static readonly string Basic = Guid.NewGuid().ToString();
  public static readonly string Moderator = Guid.NewGuid().ToString();
  public static readonly string Owner = Guid.NewGuid().ToString();
  public static readonly string SuperAdmin = Guid.NewGuid().ToString();

  public static readonly string SuperAdminUser = Guid.NewGuid().ToString();
  public static readonly string BasicUser = Guid.NewGuid().ToString();
}
