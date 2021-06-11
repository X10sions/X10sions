using Microsoft.EntityFrameworkCore.Metadata;

namespace xEFCore.xAS400.Metadata {
  public interface IAS400PropertyAnnotations : IRelationalPropertyAnnotations {
    AS400ValueGenerationStrategy? ValueGenerationStrategy { get; }
    string HiLoSequenceName { get; }
    string HiLoSequenceSchema { get; }
    ISequence FindHiLoSequence();
  }
}
