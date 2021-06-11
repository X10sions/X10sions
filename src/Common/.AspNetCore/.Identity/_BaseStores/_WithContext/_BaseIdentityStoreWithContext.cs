using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity {
  public class _BaseIdentityStoreWithContext<TContext> : _BaseIdentityStore, IIdentityStoreWithContext<TContext>
      where TContext : class, IIdentityContext {

    public _BaseIdentityStoreWithContext(TContext context, IdentityErrorDescriber errorDescriber) : base(errorDescriber) {
      Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public TContext Context { get; }
    //IIdentityContext IIdentityStore.Context => Context;

    public bool Disposed { get; set; }

    public virtual void Dispose() => Disposed = true;

  }
}