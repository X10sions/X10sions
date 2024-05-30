namespace Common.AspNetCore.Identity {
  public static class Resources {
    // https://github.com/aspnet/AspNetCore/blob/bfec2c14be1e65f7dd361a43950d4c848ad0cd35/src/Identity/EntityFrameworkCore/src/Resources.resx
    public const string CanOnlyProtectStrings = "[ProtectedPersonalData] only works strings by default.";
    public const string NotIdentityRole = "AddEntityFrameworkStores can only be called with a role that derives from IdentityRole<TKey>.";
    public const string NotIdentityUser = "AddEntityFrameworkStores can only be called with a user that derives from IdentityUser<TKey>.";
    public const string RoleNotFound = "Role {0} does not exist.";
    public const string ValueCannotBeNullOrEmpty = "Value cannot be null or empty.";
  }
}
