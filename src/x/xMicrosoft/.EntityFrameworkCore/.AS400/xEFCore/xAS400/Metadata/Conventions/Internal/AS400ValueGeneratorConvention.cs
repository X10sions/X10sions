using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using xEFCore.xAS400.Metadata.Internal;

namespace xEFCore.xAS400.Metadata.Conventions.Internal {
  public class AS400ValueGeneratorConvention : RelationalValueGeneratorConvention {
    public override Annotation Apply(InternalPropertyBuilder propertyBuilder, string name, Annotation annotation, Annotation oldAnnotation) {
      if (name == AS400AnnotationNames.ValueGenerationStrategy) {
        propertyBuilder.ValueGenerated(GetValueGenerated(propertyBuilder.Metadata), ConfigurationSource.Convention);
        return annotation;
      }

      return base.Apply(propertyBuilder, name, annotation, oldAnnotation);
    }

    public override ValueGenerated? GetValueGenerated(Property property) {
      var valueGenerated = base.GetValueGenerated(property);
      if (valueGenerated != null) {
        return valueGenerated;
      }

      return property.AS400().GetAS400ValueGenerationStrategy(false) != null
          ? ValueGenerated.OnAdd
          : (ValueGenerated?)null;
    }
  }
}
