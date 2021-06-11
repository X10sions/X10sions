using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xEFCore.xAS400.Metadata;

namespace xEFCore.xAS400.Internal {
  public class AS400ModelValidator : RelationalModelValidator {
    public AS400ModelValidator(
        [NotNull] ModelValidatorDependencies dependencies,
        [NotNull] RelationalModelValidatorDependencies relationalDependencies)
        : base(dependencies, relationalDependencies) {
    }

    public override void Validate(IModel model) {
      base.Validate(model);

      ValidateDefaultDecimalMapping(model);
      ValidateByteIdentityMapping(model);
      ValidateNonKeyValueGeneration(model);
    }

    protected virtual void ValidateDefaultDecimalMapping([NotNull] IModel model) {
      var properties = from p in model.GetRootEntityTypesDeclaredProperties(typeof(decimal))
                       where p.AS400().ColumnType == null
                       select p;
      foreach (var property in properties) {
        Dependencies.Logger.DecimalTypeDefaultWarning(property);
      }
    }

    protected virtual void ValidateByteIdentityMapping([NotNull] IModel model) {
      var properties = from p in model.GetRootEntityTypesDeclaredProperties(typeof(byte))
                       where p.AS400().ValueGenerationStrategy == AS400ValueGenerationStrategy.IdentityColumn
                       select p;
      foreach (var property in properties) {
        Dependencies.Logger.ByteIdentityColumnWarning(property);
      }
    }

    protected virtual void ValidateNonKeyValueGeneration([NotNull] IModel model) {
      var properties = from p in model.GetRootEntityTypesDeclaredProperties()
                       where ((AS400PropertyAnnotations)p.AS400()).GetAS400ValueGenerationStrategy(false) == AS400ValueGenerationStrategy.SequenceHiLo
                          && !p.IsKey()
                       select p;
      foreach (var property in properties) {
        throw new InvalidOperationException(EFCoreStrings.NonKeyValueGeneration(property.Name, property.DeclaringEntityType.DisplayName()));
      }
    }

    //protected override void ValidateSharedTableCompatibility(
    //    IReadOnlyList<IEntityType> mappedTypes, string tableName) {
    //  var firstMappedType = mappedTypes[0];
    //  var isMemoryOptimized = firstMappedType.AS400().IsMemoryOptimized;

    //  foreach (var otherMappedType in mappedTypes.Skip(1)) {
    //    if (isMemoryOptimized != otherMappedType.AS400().IsMemoryOptimized) {
    //      throw new InvalidOperationException(
    //          EFCoreStrings.IncompatibleTableMemoryOptimizedMismatch(
    //              tableName, firstMappedType.DisplayName(), otherMappedType.DisplayName(),
    //              isMemoryOptimized ? firstMappedType.DisplayName() : otherMappedType.DisplayName(),
    //              !isMemoryOptimized ? firstMappedType.DisplayName() : otherMappedType.DisplayName()));
    //    }
    //  }
    //  base.ValidateSharedTableCompatibility(mappedTypes, tableName);
    //}

    protected override void ValidateSharedColumnsCompatibility(IReadOnlyList<IEntityType> mappedTypes, string tableName) {
      base.ValidateSharedColumnsCompatibility(mappedTypes, tableName);

      var identityColumns = mappedTypes.SelectMany(et => et.GetDeclaredProperties())
          .Where(p => p.AS400().ValueGenerationStrategy == AS400ValueGenerationStrategy.IdentityColumn)
          .Distinct((p1, p2) => p1.Name == p2.Name)
          .ToList();
      if (identityColumns.Count > 1) {
        var sb = new StringBuilder()
            .AppendJoin(identityColumns.Select(p => "'" + p.DeclaringEntityType.DisplayName() + "." + p.Name + "'"));
        throw new InvalidOperationException(EFCoreStrings.MultipleIdentityColumns(sb, tableName));
      }
    }

  }
}