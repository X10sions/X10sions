
namespace Common.AspNetCore.Identity {
  public class IdentityConstants {

    public const string RoleTableName = "AspNetRoles";
    public const string RoleClaimTableName = "AspNetRoleClaims";
    public const string UserTableName = "AspNetUsers";
    public const string UserClaimTableName = "AspNetUserClaims";
    public const string UserLoginTableName = "AspNetUserLogins";
    public const string UserRoleTableName = "AspNetUserRoles";
    public const string UserTokenTableName = "AspNetUserTokens";

    public const string RoleTableNormalizedNameIndex = RoleTableName + "_Index_NormalizedName";
    public const string UserTableNormalizedNameIndex = UserTableName + "_Index_NormalizedName";

    #region IUserAuthenticationTokenStore
    public const string InternalLoginProvider = "[AspNetUserStore]";
    public const string AuthenticatorKeyTokenName = "AuthenticatorKey";
    public const string RecoveryCodeTokenName = "RecoveryCodes";
    #endregion

  }
}