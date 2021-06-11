using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public interface IIdentityUserWithPasswordStoreWithContext<TContext, TUser, TKey> : IUserPasswordStore<TUser>, IIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext//, IIdentityContext_WithUsers<TKey>
    where TKey : IEquatable<TKey>
    where TUser : class, IIdentityUserWithPassword<TKey> {
  }

  public static class IIdentityUserWithPasswordStoreWithContextExtensions {


  }
}