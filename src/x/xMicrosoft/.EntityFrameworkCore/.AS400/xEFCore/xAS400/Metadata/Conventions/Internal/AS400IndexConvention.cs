using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace xEFCore.xAS400.Metadata.Conventions.Internal {
  public class AS400IndexConvention :
      IIndexAddedConvention,
      IIndexUniquenessChangedConvention,
      IIndexAnnotationChangedConvention,
      IPropertyNullabilityChangedConvention,
      IPropertyAnnotationChangedConvention {
     readonly ISqlGenerationHelper _sqlGenerationHelper;

    public AS400IndexConvention([NotNull] ISqlGenerationHelper sqlGenerationHelper) {
      _sqlGenerationHelper = sqlGenerationHelper;
    }

    InternalIndexBuilder IIndexAddedConvention.Apply(InternalIndexBuilder indexBuilder)
        => SetIndexFilter(indexBuilder);

    bool IIndexUniquenessChangedConvention.Apply(InternalIndexBuilder indexBuilder) {
      SetIndexFilter(indexBuilder);
      return true;
    }

    public virtual bool Apply(InternalPropertyBuilder propertyBuilder) {
      foreach (var index in propertyBuilder.Metadata.GetContainingIndexes()) {
        SetIndexFilter(index.Builder);
      }
      return true;
    }

    public virtual Annotation Apply(InternalIndexBuilder indexBuilder, string name, Annotation annotation, Annotation oldAnnotation) {
      //if (name == AS400AnnotationNames.Clustered) {
      //  SetIndexFilter(indexBuilder);
      //}
      return annotation;
    }

    public virtual Annotation Apply(InternalPropertyBuilder propertyBuilder, string name, Annotation annotation, Annotation oldAnnotation) {
      if (name == RelationalAnnotationNames.ColumnName) {
        foreach (var index in propertyBuilder.Metadata.GetContainingIndexes()) {
          SetIndexFilter(index.Builder,  true);
        }
      }
      return annotation;
    }

    private InternalIndexBuilder SetIndexFilter(InternalIndexBuilder indexBuilder, bool columnNameChanged = false) {
      // TODO: compare with a cached filter to avoid overriding if it was set by a different convention
      var index = indexBuilder.Metadata;
      if (index.IsUnique
          //&& indexBuilder.Metadata.AS400().IsClustered != true
          && index.Properties
              .Any(property => property.IsColumnNullable())) {
        if (columnNameChanged
            || index.AS400().Filter == null) {
          indexBuilder.AS400(ConfigurationSource.Convention).HasFilter(CreateIndexFilter(index));
        }
      } else {
        if (index.AS400().Filter != null) {
          indexBuilder.AS400(ConfigurationSource.Convention).HasFilter(null);
        }
      }

      return indexBuilder;
    }

    string CreateIndexFilter(IIndex index) {
      var nullableColumns = index.Properties
          .Where(property => property.IsColumnNullable())
          .Select(property => property.AS400().ColumnName)
          .ToList();
      var builder = new StringBuilder();
      for (var i = 0; i < nullableColumns.Count; i++) {
        if (i != 0) {
          builder.Append(" AND ");
        }
        builder
            .Append(_sqlGenerationHelper.DelimitIdentifier(nullableColumns[i]))
            .Append(" IS NOT NULL");
      }
      return builder.ToString();
    }

  }
}
