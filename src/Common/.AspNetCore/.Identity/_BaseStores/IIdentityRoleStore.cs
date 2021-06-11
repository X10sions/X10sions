using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityRoleStore<TRole, TKey>
    : IQueryableRoleStore<TRole>
    , IIdentityStore//<IIdentityContext_WithUserAndRoles<TKey>>
    where TKey : IEquatable<TKey>
    where TRole : class, IIdentityRoleWithConcurrency<TKey> {
    //IQueryable<TRole> RoleQueryable { get; }
    //IIdentityDatabaseTable<TRole> RoleDatabaseTable{ get; set; }
  }

  public static class IIdentityRoleStoreExtensions {
  }

}