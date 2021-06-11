using Microsoft.EntityFrameworkCore.Metadata;

namespace xEFCore.xAS400.Metadata {
  public interface IAS400ModelAnnotations : IRelationalModelAnnotations {
    AS400ValueGenerationStrategy? ValueGenerationStrategy { get; }
    string HiLoSequenceName { get; }
    string HiLoSequenceSchema { get; }
  }
}