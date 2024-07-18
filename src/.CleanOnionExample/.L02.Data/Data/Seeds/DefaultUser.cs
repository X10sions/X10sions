using CleanOnionExample.Data.Entities;
using X10sions.Fake.Features.User;
namespace CleanOnionExample.Data.Seeds;

public static class DefaultUser {
  public static List<ApplicationUser> IdentityBasicUserList() {
    return new List<ApplicationUser>() {
      new ApplicationUser {
        Id = UserRole. SuperAdminUser,
        UserName = "superadmin",
        Email = "superadmin@gmail.com",
        FirstName = "Amit",
        LastName = "Naik",
        EmailConfirmed = true,
        PhoneNumberConfirmed = true,
        // Password@123
        PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
        NormalizedEmail= "SUPERADMIN@GMAIL.COM",
        NormalizedUserName="SUPERADMIN"
      },
      new ApplicationUser {
        Id = UserRole. BasicUser,
        UserName = "basicuser",
        Email = "basicuser@gmail.com",
        FirstName = "Basic",
        LastName = "User",
        EmailConfirmed = true,
        PhoneNumberConfirmed = true,
        // Password@123
        PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
        NormalizedEmail= "BASICUSER@GMAIL.COM",
        NormalizedUserName="BASICUSER"
      },
    };
  }
}
