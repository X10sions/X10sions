using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {

  public class _BaseIdentityRoleStoreWithContext_LinqToDB<TContext, TRole, TKey>
    : _BaseIdentityRoleStoreWithContext<TContext, TRole, TKey> //IIdentityRoleStore<TRole, TKey>
    where TContext : class, IIdentityContext, IIdentityContext_LinqToDB//, IIdentityContext_WithUserAndRoles<TRole, TKey, TUserRole>
    where TRole : class, IIdentityRoleWithConcurrency<TKey>
    //where TUserRole : class, IIdentityUserRole<TKey>
    where TKey : IEquatable<TKey> {

    public _BaseIdentityRoleStoreWithContext_LinqToDB(TContext context, IdentityErrorDescriber errorDescriber) : base(context, errorDescriber) { }

    //#region IDisposable
    //public override void Dispose() {
    //  base.Dispose();
    //  Context.Dispose();
    //}
    //#endregion

  }
}
