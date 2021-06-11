namespace xEFCore.xAS400.Metadata.Internal {
  public static class AS400AnnotationNames {
    public const string Prefix = "AS400:";
    //public const string Clustered = Prefix + nameof(Clustered);
    public const string HiLoSequenceName = Prefix + nameof(HiLoSequenceName);
    public const string HiLoSequenceSchema = Prefix + nameof(HiLoSequenceSchema);
    //public const string MemoryOptimized = Prefix + nameof(MemoryOptimized);
    public const string NamingConvention = Prefix + nameof(NamingConvention);
    public const string ValueGenerationStrategy = Prefix + nameof(ValueGenerationStrategy);
  }
}