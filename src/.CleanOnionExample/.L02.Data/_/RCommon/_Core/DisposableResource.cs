﻿using System.Diagnostics;

namespace RCommon;

public abstract class DisposableResource : IDisposable, IAsyncDisposable {
  ~DisposableResource() {
    Dispose(false);
  }

  [DebuggerStepThrough]
  public void Dispose() {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  public async ValueTask DisposeAsync() {
    await this.DisposeAsync(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {  }

  protected async virtual Task DisposeAsync(bool disposing) {
    await Task.Yield();
  }
}
