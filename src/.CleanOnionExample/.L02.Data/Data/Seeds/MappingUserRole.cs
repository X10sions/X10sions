using Microsoft.AspNetCore.Identity;
using X10sions.Fake.Features.User;

namespace CleanOnionExample.Data.Seeds;
public static class MappingUserRole {
  public static List<IdentityUserRole<string>> IdentityUserRoleList() {
    return new List<IdentityUserRole<string>>() {
      new IdentityUserRole<string> {        RoleId = UserRole .Basic,        UserId = UserRole .BasicUser      },
      new IdentityUserRole<string> {        RoleId = UserRole .SuperAdmin,   UserId = UserRole .SuperAdminUser      },
      new IdentityUserRole<string> {        RoleId = UserRole .Admin,        UserId = UserRole .SuperAdminUser      },
      new IdentityUserRole<string> {        RoleId = UserRole .Moderator,    UserId = UserRole .SuperAdminUser      },
      new IdentityUserRole<string> {        RoleId = UserRole .Basic,        UserId = UserRole .SuperAdminUser      }
    };
  }
}