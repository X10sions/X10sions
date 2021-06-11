using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace xEFCore.xAS400.ValueGeneration.Internal {
  public class AS400SequenceValueGeneratorState : HiLoValueGeneratorState {
    public AS400SequenceValueGeneratorState([NotNull] ISequence sequence)
           : base(Check.NotNull(sequence, nameof(sequence)).IncrementBy) {
      Sequence = sequence;
    }
    public virtual ISequence Sequence { get; }
  }
}
