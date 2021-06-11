using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Update;

namespace xEFCore.xAS400.Update.Internal {
  public interface IAS400UpdateSqlGenerator : IUpdateSqlGenerator {
    ResultSetMapping AppendBulkInsertOperation([NotNull] StringBuilder commandStringBuilder, [NotNull] IReadOnlyList<ModificationCommand> modificationCommands, int commandPosition);
  }
}
