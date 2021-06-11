using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public class _BaseIdentityUserStoreWithContext_LinqToDB<TContext, TUser, TKey> : _BaseIdentityUserStoreWithContext<TContext, TUser, TKey>
    where TContext : class, IIdentityContext, IIdentityContext_LinqToDB//, IIdentityContext_WithUsers<TUser, TKey>
    where TUser : class, IIdentityUserWithConcurrency<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityUserStoreWithContext_LinqToDB(TContext context, IdentityErrorDescriber errorDescriber) : base(context, errorDescriber) { }

    //#region IDisposable
    //public override void Dispose() {
    //  base.Dispose();
    //  Context.Dispose();
    //}
    //#endregion

  }
}
