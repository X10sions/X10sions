using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity.Providers.Raven {
  public class _BaseIdentityUserStoreWithContext_RavenDb<TUser, TKey> : _BaseIdentityUserStoreWithContext<IIdentityContext_RavenDb, TUser, TKey>
    where TUser : _BaseIdentityUser_WithConcurrency<TKey>
    where TKey : IEquatable<TKey> {

    // https://www.eximiaco.tech/en/2019/07/27/writing-an-asp-net-core-identity-storage-provider-from-scratch-with-ravendb/

    public _BaseIdentityUserStoreWithContext_RavenDb(IIdentityContext_RavenDb context, IdentityErrorDescriber errorDescriber)
      : base(context, errorDescriber) { }

    #region IDisposable
    public override void Dispose() {
      base.Dispose();
      Context.Session.Dispose();
    }
    #endregion

  }
}
