using Microsoft.AspNetCore.Identity;
using X10sions.Fake.Features.User;

namespace CleanOnionExample.Data.Seeds;
public static class DefaultRoles {
  public static List<IdentityRole> IdentityRoleList() {
    return new List<IdentityRole>() {
      new IdentityRole {
        Id = UserRole .SuperAdmin,
        Name = UserRole .SuperAdmin.ToString(),
        NormalizedName = UserRole .SuperAdmin.ToString()
      },
      new IdentityRole {
        Id = UserRole      .Admin,
        Name = UserRole.Admin.ToString(),
        NormalizedName = UserRole.Admin.ToString()
      },
      new IdentityRole {
        Id = UserRole .Moderator,
        Name = UserRole.Moderator.ToString(),
        NormalizedName = UserRole.Moderator.ToString()
      },
      new IdentityRole {
        Id = UserRole .Basic,
        Name = UserRole.Basic.ToString(),
        NormalizedName = UserRole.Basic.ToString()
      }
    };
  }
}
