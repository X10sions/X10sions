using System;

namespace Common.AspNetCore.Identity {
  public interface IId<TKey> where TKey : IEquatable<TKey> {
    TKey Id { get; }
  }
}