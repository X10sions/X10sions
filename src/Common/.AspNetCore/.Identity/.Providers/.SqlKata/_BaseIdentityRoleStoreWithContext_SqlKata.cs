using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity.Providers.SqlKata {

  public class _BaseIdentityRoleStoreWithContext_SqlKata<TContext, TRole, TKey>
    : _BaseIdentityRoleStoreWithContext<TContext, TRole, TKey> //IIdentityRoleStore<TRole, TKey>
    where TContext : class, IIdentityContext, IIdentityContext_SqlKata//, IIdentityContext_WithUserAndRoles<TRole, TKey, TUserRole>
    where TRole : class, IIdentityRoleWithConcurrency<TKey>
    //where TUserRole : class, IIdentityUserRole<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityRoleStoreWithContext_SqlKata(TContext context, IdentityErrorDescriber errorDescriber)
      : base(context, errorDescriber) { }

    //#region IDisposable
    //public override void Dispose() {
    //  base.Dispose();
    //  Context.Dispose();
    //}
    //#endregion

  }
}
