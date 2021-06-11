using IBM.Data.DB2.iSeries;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace xEFCore.xAS400.Infrastructure.Internal {
  public interface IAS400Options : ISingletonOptions {
    bool RowNumberPagingEnabled { get; }
    iDB2NamingConvention NamingConvention { get; }
  }
}
