using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity.Providers.Raven {
  public class _BaseIdentityRoleStoreWithContext_RavenDb<TRole, TKey> : _BaseIdentityRoleStoreWithContext<IIdentityContext_RavenDb, TRole, TKey>
    where TRole : _BaseIdentityRole_WithConcurrency<TKey>
    where TKey : IEquatable<TKey> {

    // https://www.eximiaco.tech/en/2019/07/27/writing-an-asp-net-core-identity-storage-provider-from-scratch-with-ravendb/

    public _BaseIdentityRoleStoreWithContext_RavenDb(IIdentityContext_RavenDb context, IdentityErrorDescriber errorDescriber)
      : base(context, errorDescriber) { }

    #region IDisposable
    public override void Dispose() {
      base.Dispose();
      Context.Session.Dispose();
    }
    #endregion

  }
}
