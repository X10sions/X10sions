using System;
using System.Collections.Concurrent;

namespace xNonFactors.MvcTemplate.Resources {
  internal class ResourceDictionary : ConcurrentDictionary<string, string?> {
    public ResourceDictionary() : base(StringComparer.OrdinalIgnoreCase) { }
  }
}