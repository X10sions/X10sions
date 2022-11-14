using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using xEFCore.xAS400.Storage.Internal;
using xEFCore.xAS400.Update.Internal;

namespace xEFCore.xAS400.ValueGeneration.Internal {
  public class AS400SequenceHiLoValueGenerator<TValue> : HiLoValueGenerator<TValue> {

    public AS400SequenceHiLoValueGenerator(
         [NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder,
         [NotNull] IAS400UpdateSqlGenerator sqlGenerator,
         [NotNull] AS400SequenceValueGeneratorState generatorState,
         [NotNull] IAS400RelationalConnection connection)
         : base(generatorState) {
      Check.NotNull(rawSqlCommandBuilder, nameof(rawSqlCommandBuilder));
      Check.NotNull(sqlGenerator, nameof(sqlGenerator));
      Check.NotNull(connection, nameof(connection));

      _sequence = generatorState.Sequence;
      _rawSqlCommandBuilder = rawSqlCommandBuilder;
      _sqlGenerator = sqlGenerator;
      _connection = connection;
    }

    readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;
    readonly IAS400UpdateSqlGenerator _sqlGenerator;
    readonly IAS400RelationalConnection _connection;
    readonly ISequence _sequence;

    protected override long GetNewLowValue()
       => (long)Convert.ChangeType(_rawSqlCommandBuilder
               .Build(_sqlGenerator.GenerateNextSequenceValueOperation(_sequence.Name, _sequence.Schema))
               .ExecuteScalar(_connection),
           typeof(long),
           CultureInfo.InvariantCulture);

    protected override async Task<long> GetNewLowValueAsync(CancellationToken cancellationToken = default)
        => (long)Convert.ChangeType(await _rawSqlCommandBuilder
                .Build(_sqlGenerator.GenerateNextSequenceValueOperation(_sequence.Name, _sequence.Schema))
                .ExecuteScalarAsync(_connection, cancellationToken: cancellationToken),
            typeof(long),
            CultureInfo.InvariantCulture);

    public override bool GeneratesTemporaryValues => false;

  }
}