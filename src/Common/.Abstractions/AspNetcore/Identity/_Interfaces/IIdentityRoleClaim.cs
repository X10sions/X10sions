using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityRoleClaim<TKey> : IClaimConverter where TKey : IEquatable<TKey> {
    int Id { get; set; }
    TKey RoleId { get; set; }

    IIdentityRole<TKey> Role { get; set; }

  }


  public static class IIdentityRoleClaimExtensions {
    #region IQueryable


    #endregion
  }
}