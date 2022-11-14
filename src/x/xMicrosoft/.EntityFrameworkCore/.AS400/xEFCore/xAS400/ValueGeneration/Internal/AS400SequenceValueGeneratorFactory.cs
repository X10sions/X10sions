using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using xEFCore.xAS400.Storage.Internal;
using xEFCore.xAS400.Update.Internal;

namespace xEFCore.xAS400.ValueGeneration.Internal {
  public class AS400SequenceValueGeneratorFactory : IAS400SequenceValueGeneratorFactory {

    public AS400SequenceValueGeneratorFactory(
         [NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder,
         [NotNull] IAS400UpdateSqlGenerator sqlGenerator) {
      Check.NotNull(rawSqlCommandBuilder, nameof(rawSqlCommandBuilder));
      Check.NotNull(sqlGenerator, nameof(sqlGenerator));
      _rawSqlCommandBuilder = rawSqlCommandBuilder;
      _sqlGenerator = sqlGenerator;
    }

    readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;
    readonly IAS400UpdateSqlGenerator _sqlGenerator;

    public virtual ValueGenerator Create(IProperty property, AS400SequenceValueGeneratorState generatorState, IAS400RelationalConnection connection) {
      Check.NotNull(property, nameof(property));
      Check.NotNull(generatorState, nameof(generatorState));
      Check.NotNull(connection, nameof(connection));

      var type = property.ClrType.UnwrapNullableType().UnwrapEnumType();

      if (type == typeof(long)) {
        return new AS400SequenceHiLoValueGenerator<long>(_rawSqlCommandBuilder, _sqlGenerator, generatorState, connection);
      }
      if (type == typeof(int)) {
        return new AS400SequenceHiLoValueGenerator<int>(_rawSqlCommandBuilder, _sqlGenerator, generatorState, connection);
      }
      if (type == typeof(short)) {
        return new AS400SequenceHiLoValueGenerator<short>(_rawSqlCommandBuilder, _sqlGenerator, generatorState, connection);
      }
      if (type == typeof(byte)) {
        return new AS400SequenceHiLoValueGenerator<byte>(_rawSqlCommandBuilder, _sqlGenerator, generatorState, connection);
      }
      if (type == typeof(char)) {
        return new AS400SequenceHiLoValueGenerator<char>(_rawSqlCommandBuilder, _sqlGenerator, generatorState, connection);
      }
      if (type == typeof(ulong)) {
        return new AS400SequenceHiLoValueGenerator<ulong>(_rawSqlCommandBuilder, _sqlGenerator, generatorState, connection);
      }
      if (type == typeof(uint)) {
        return new AS400SequenceHiLoValueGenerator<uint>(_rawSqlCommandBuilder, _sqlGenerator, generatorState, connection);
      }
      if (type == typeof(ushort)) {
        return new AS400SequenceHiLoValueGenerator<ushort>(_rawSqlCommandBuilder, _sqlGenerator, generatorState, connection);
      }
      if (type == typeof(sbyte)) {
        return new AS400SequenceHiLoValueGenerator<sbyte>(_rawSqlCommandBuilder, _sqlGenerator, generatorState, connection);
      }
      throw new ArgumentException(CoreStrings.InvalidValueGeneratorFactoryProperty(
          nameof(AS400SequenceValueGeneratorFactory), property.Name, property.DeclaringEntityType.DisplayName()));
    }

  }
}