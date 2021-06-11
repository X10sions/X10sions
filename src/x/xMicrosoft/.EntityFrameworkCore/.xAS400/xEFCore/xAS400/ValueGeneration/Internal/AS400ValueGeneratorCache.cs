using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace xEFCore.xAS400.ValueGeneration.Internal {
  public class AS400ValueGeneratorCache : ValueGeneratorCache, IAS400ValueGeneratorCache {
    readonly ConcurrentDictionary<string, AS400SequenceValueGeneratorState> _sequenceGeneratorCache
       = new ConcurrentDictionary<string, AS400SequenceValueGeneratorState>();

    public AS400ValueGeneratorCache([NotNull] ValueGeneratorCacheDependencies dependencies)
        : base(dependencies) {
    }
    public virtual AS400SequenceValueGeneratorState GetOrAddSequenceState(IProperty property) {
      Check.NotNull(property, nameof(property));
      var sequence = property.AS400().FindHiLoSequence();
      Debug.Assert(sequence != null);
      return _sequenceGeneratorCache.GetOrAdd(
          GetSequenceName(sequence),
          sequenceName => new AS400SequenceValueGeneratorState(sequence));
    }

    static string GetSequenceName(ISequence sequence) => (sequence.Schema == null ? "" : sequence.Schema + ".") + sequence.Name;
  }
}
