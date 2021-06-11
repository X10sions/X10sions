using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using xEFCore.xAS400.Metadata.Internal;
using xEFCoreDev.Microsoft.EntityFrameworkCore.Metadata;

namespace xEFCore.xAS400.Metadata {
  public class AS400PropertyAnnotations : RelationalPropertyAnnotations, IAS400PropertyAnnotations {

    public AS400PropertyAnnotations([NotNull] IProperty property)
        : base(property) {
    }

    protected AS400PropertyAnnotations([NotNull] RelationalAnnotations annotations)
        : base(annotations) {
    }

    public virtual string HiLoSequenceName {
      get { return (string)Annotations.Metadata[AS400AnnotationNames.HiLoSequenceName]; }
      [param: CanBeNull]
      set { SetHiLoSequenceName(value); }
    }
    protected virtual bool SetHiLoSequenceName([CanBeNull] string value)
        => Annotations.SetAnnotation(
            AS400AnnotationNames.HiLoSequenceName,
            Check.NullButNotEmpty(value, nameof(value)));

    public virtual string HiLoSequenceSchema {
      get { return (string)Annotations.Metadata[AS400AnnotationNames.HiLoSequenceSchema]; }
      [param: CanBeNull]
      set { SetHiLoSequenceSchema(value); }
    }
    protected virtual bool SetHiLoSequenceSchema([CanBeNull] string value)
        => Annotations.SetAnnotation(
            AS400AnnotationNames.HiLoSequenceSchema,
            Check.NullButNotEmpty(value, nameof(value)));

    public virtual ISequence FindHiLoSequence() {
      var modelExtensions = Property.DeclaringEntityType.Model.AS400();
      if (ValueGenerationStrategy != AS400ValueGenerationStrategy.SequenceHiLo) {
        return null;
      }
      var sequenceName = HiLoSequenceName ?? modelExtensions.HiLoSequenceName ?? EFCoreConstants.Metadata.DefaultHiLoSequenceName;
      var sequenceSchema = HiLoSequenceSchema ?? modelExtensions.HiLoSequenceSchema;
      return modelExtensions.FindSequence(sequenceName, sequenceSchema);
    }

    public virtual AS400ValueGenerationStrategy? ValueGenerationStrategy {
      get { return GetAS400ValueGenerationStrategy(true); }
      [param: CanBeNull]
      set { SetValueGenerationStrategy(value); }
    }

    public virtual AS400ValueGenerationStrategy? GetAS400ValueGenerationStrategy(bool fallbackToModel) {
      var value = (AS400ValueGenerationStrategy?)Annotations.Metadata[AS400AnnotationNames.ValueGenerationStrategy];
      if (value != null) {
        return value;
      }
      var relationalProperty = Property.Relational();
      if (!fallbackToModel
          || Property.ValueGenerated != ValueGenerated.OnAdd
          || relationalProperty.DefaultValue != null
          || relationalProperty.DefaultValueSql != null
          || relationalProperty.ComputedColumnSql != null) {
        return null;
      }
      var modelStrategy = Property.DeclaringEntityType.Model.AS400().ValueGenerationStrategy;
      if (modelStrategy == AS400ValueGenerationStrategy.SequenceHiLo && IsCompatibleSequenceHiLo(Property.ClrType)) {
        return AS400ValueGenerationStrategy.SequenceHiLo;
      }
      if (modelStrategy == AS400ValueGenerationStrategy.IdentityColumn && IsCompatibleIdentityColumn(Property.ClrType)) {
        return AS400ValueGenerationStrategy.IdentityColumn;
      }
      return null;
    }

    protected virtual bool SetValueGenerationStrategy(AS400ValueGenerationStrategy? value) {
      if (value != null) {
        var propertyType = Property.ClrType;
        if (value == AS400ValueGenerationStrategy.IdentityColumn && !IsCompatibleIdentityColumn(propertyType)) {
          if (ShouldThrowOnInvalidConfiguration) {
            throw new ArgumentException(EFCoreStrings.IdentityBadType(
                Property.Name, Property.DeclaringEntityType.DisplayName(), propertyType.ShortDisplayName()));
          }
          return false;
        }
        if (value == AS400ValueGenerationStrategy.SequenceHiLo && !IsCompatibleSequenceHiLo(propertyType)) {
          if (ShouldThrowOnInvalidConfiguration) {
            throw new ArgumentException(EFCoreStrings.SequenceBadType(
                Property.Name, Property.DeclaringEntityType.DisplayName(), propertyType.ShortDisplayName()));
          }
          return false;
        }
      }
      if (!CanSetValueGenerationStrategy(value)) {
        return false;
      }
      if (!ShouldThrowOnConflict && ValueGenerationStrategy != value && value != null) {
        ClearAllServerGeneratedValues();
      }
      return Annotations.SetAnnotation(AS400AnnotationNames.ValueGenerationStrategy, value);
    }

    protected virtual bool CanSetValueGenerationStrategy(AS400ValueGenerationStrategy? value) {
      if (GetAS400ValueGenerationStrategy(false) == value) {
        return true;
      }
      if (!Annotations.CanSetAnnotation(AS400AnnotationNames.ValueGenerationStrategy, value)) {
        return false;
      }
      if (ShouldThrowOnConflict) {
        if (GetDefaultValue(false) != null) {
          throw new InvalidOperationException(RelationalStrings.ConflictingColumnServerGeneration(nameof(ValueGenerationStrategy), Property.Name, nameof(DefaultValue)));
        }
        if (GetDefaultValueSql(false) != null) {
          throw new InvalidOperationException(RelationalStrings.ConflictingColumnServerGeneration(nameof(ValueGenerationStrategy), Property.Name, nameof(DefaultValueSql)));
        }
        if (GetComputedColumnSql(false) != null) {
          throw new InvalidOperationException(RelationalStrings.ConflictingColumnServerGeneration(nameof(ValueGenerationStrategy), Property.Name, nameof(ComputedColumnSql)));
        }
      } else if (value != null && (!CanSetDefaultValue(null) || !CanSetDefaultValueSql(null) || !CanSetComputedColumnSql(null))) {
        return false;
      }
      return true;
    }

    protected override object GetDefaultValue(bool fallback) {
      if (fallback && ValueGenerationStrategy != null) {
        return null;
      }
      return base.GetDefaultValue(fallback);
    }

    protected override bool CanSetDefaultValue(object value) {
      if (ShouldThrowOnConflict) {
        if (ValueGenerationStrategy != null) {
          throw new InvalidOperationException(RelationalStrings.ConflictingColumnServerGeneration(nameof(DefaultValue), Property.Name, nameof(ValueGenerationStrategy)));
        }
      } else if (value != null && !CanSetValueGenerationStrategy(null)) {
        return false;
      }
      return base.CanSetDefaultValue(value);
    }

    protected override string GetDefaultValueSql(bool fallback) {
      if (fallback && ValueGenerationStrategy != null) {
        return null;
      }
      return base.GetDefaultValueSql(fallback);
    }

    protected override bool CanSetDefaultValueSql(string value) {
      if (ShouldThrowOnConflict) {
        if (ValueGenerationStrategy != null) {
          throw new InvalidOperationException(
              RelationalStrings.ConflictingColumnServerGeneration(nameof(DefaultValueSql), Property.Name, nameof(ValueGenerationStrategy)));
        }
      } else if (value != null && !CanSetValueGenerationStrategy(null)) {
        return false;
      }
      return base.CanSetDefaultValueSql(value);
    }

    protected override string GetComputedColumnSql(bool fallback) {
      if (fallback && ValueGenerationStrategy != null) {
        return null;
      }
      return base.GetComputedColumnSql(fallback);
    }

    protected override bool CanSetComputedColumnSql(string value) {
      if (ShouldThrowOnConflict) {
        if (ValueGenerationStrategy != null) {
          throw new InvalidOperationException(RelationalStrings.ConflictingColumnServerGeneration(nameof(ComputedColumnSql), Property.Name, nameof(ValueGenerationStrategy)));
        }
      } else if (value != null && !CanSetValueGenerationStrategy(null)) {
        return false;
      }
      return base.CanSetComputedColumnSql(value);
    }

    protected override void ClearAllServerGeneratedValues() {
      SetValueGenerationStrategy(null);
      base.ClearAllServerGeneratedValues();
    }

    static bool IsCompatibleIdentityColumn(Type type) => type.IsInteger() || type == typeof(decimal);
    static bool IsCompatibleSequenceHiLo(Type type) => type.IsInteger();

  }
}
