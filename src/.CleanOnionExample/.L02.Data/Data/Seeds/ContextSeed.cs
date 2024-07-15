using CleanOnionExample.Data.DbContexts;
using CleanOnionExample.Data.Entities;
using Common.ValueObjects;
using Common.Features.DummyFakeExamples.ToDo;
using Common.Features.DummyFakeExamples.ToDo.Item;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using X10sions.Fake.Features.ToDo.Item;
using X10sions.Fake.Features.ToDo;

namespace CleanOnionExample.Data.Seeds;

public static class ContextSeed {
  public static void Seed(this ModelBuilder modelBuilder) {
    CreateRoles(modelBuilder);
    CreateBasicUsers(modelBuilder);
    MapUserRole(modelBuilder);
  }

  private static void CreateRoles(ModelBuilder modelBuilder) {
    var roles = DefaultRoles.IdentityRoleList();
    modelBuilder.Entity<IdentityRole>().HasData(roles);
  }

  private static void CreateBasicUsers(ModelBuilder modelBuilder) {
    var users = DefaultUser.IdentityBasicUserList();
    modelBuilder.Entity<ApplicationUser>().HasData(users);
  }

  private static void MapUserRole(ModelBuilder modelBuilder) {
    var identityUserRoles = MappingUserRole.IdentityUserRoleList();
    modelBuilder.Entity<IdentityUserRole<string>>().HasData(identityUserRoles);
  }
}


public static class ApplicationDbContextSeed {
  public static async System.Threading.Tasks.Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
    var administratorRole = new IdentityRole("Administrator");
    if (roleManager.Roles.All(r => r.Name != administratorRole.Name)) {
      await roleManager.CreateAsync(administratorRole);
    }
    var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

    if (userManager.Users.All(u => u.UserName != administrator.UserName)) {
      await userManager.CreateAsync(administrator, "Administrator1!");
      await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
    }
  }

  public static async Task SeedSampleDataAsync(ApplicationDbContext context) {
    // Seed, if necessary
    if (!context.TodoLists.Any()) {
      context.TodoLists.Add(new ToDoList {
        Title = "Shopping",
        Colour = ColourCode.Blue,
        Items = {
          new ToDoItem { Title = "Apples", IsDone = true },
          new ToDoItem { Title = "Milk", IsDone = true },
          new ToDoItem { Title = "Bread", IsDone = true },
          new ToDoItem { Title = "Toilet paper" },
          new ToDoItem { Title = "Pasta" },
          new ToDoItem { Title = "Tissues" },
          new ToDoItem { Title = "Tuna" },
          new ToDoItem { Title = "Water" }
        }
      });

      await context.SaveChangesAsync();
    }
  }
}
