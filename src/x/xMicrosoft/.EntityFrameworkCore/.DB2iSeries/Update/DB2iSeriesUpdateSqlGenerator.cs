using Microsoft.EntityFrameworkCore.Update;
using System.Globalization;
using System.Text;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Update;

public class DB2iSeriesUpdateSqlGenerator : UpdateAndSelectSqlGenerator {

  public DB2iSeriesUpdateSqlGenerator(UpdateSqlGeneratorDependencies dependencies) : base(dependencies) { }


  protected override void AppendIdentityWhereCondition(StringBuilder commandStringBuilder, IColumnModification columnModification) {
    commandStringBuilder.AppendFormat("{0} = IDENTITY_VAL_LOCAL()", SqlGenerationHelper.DelimitIdentifier(columnModification.ColumnName));
  }

  //public override ResultSetMapping AppendInsertOperation(StringBuilder commandStringBuilder, IReadOnlyModificationCommand command, int commandPosition, out bool requiresTransaction) => base.AppendInsertOperation(commandStringBuilder, command, commandPosition, out requiresTransaction);
  //public override ResultSetMapping AppendUpdateOperation(StringBuilder commandStringBuilder, IReadOnlyModificationCommand command, int commandPosition, out bool requiresTransaction) => base.AppendUpdateOperation(commandStringBuilder, command, commandPosition, out requiresTransaction);
  //public override ResultSetMapping AppendDeleteOperation(StringBuilder commandStringBuilder, IReadOnlyModificationCommand command, int commandPosition, out bool requiresTransaction) => base.AppendDeleteOperation(commandStringBuilder, command, commandPosition, out requiresTransaction);

  protected override void AppendRowsAffectedWhereCondition(StringBuilder commandStringBuilder, int expectedRowsAffected) => commandStringBuilder.AppendFormat("ROW_COUNT() = {0}", expectedRowsAffected.ToString(CultureInfo.InvariantCulture));


//  protected override ResultSetMapping AppendSelectAffectedCommand(StringBuilder commandStringBuilder, string name, string schema, IReadOnlyList<IColumnModification> readOperations, IReadOnlyList<IColumnModification> conditionOperations, int commandPosition) => base.AppendSelectAffectedCommand(commandStringBuilder, name, schema, readOperations, conditionOperations, commandPosition);


  protected override ResultSetMapping AppendSelectAffectedCountCommand(StringBuilder commandStringBuilder, string name, string schema, int commandPosition) {
    commandStringBuilder
        .Append("SELECT ROW_COUNT() FROM SYSIBM.SYSDUMMY1")
        .Append(SqlGenerationHelper.StatementTerminator)
        .AppendLine();
    return ResultSetMapping.LastInResultSet;
  }

}

