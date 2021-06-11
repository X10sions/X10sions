using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserStore<TUser, TKey>
    : IQueryableUserStore<TUser>
    , IIdentityStore
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithConcurrency<TKey> {
    //IQueryable<TRole> RoleQueryable { get; }
    //IIdentityDatabaseTable<TRole> RoleDatabaseTable{ get; set; }
  }

}