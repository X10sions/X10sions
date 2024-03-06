using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace xEFCore.xAS400.Metadata.Conventions.Internal {
  public class AS400ValueGenerationStrategyConvention : IModelInitializedConvention {

    public virtual InternalModelBuilder Apply(InternalModelBuilder modelBuilder) {
      modelBuilder.AS400(ConfigurationSource.Convention).ValueGenerationStrategy(AS400ValueGenerationStrategy.IdentityColumn);
      return modelBuilder;
    }

  }
}