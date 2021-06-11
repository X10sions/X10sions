using IBM.Data.DB2.iSeries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using xEFCore.xAS400.Infrastructure;
using xEFCore.xAS400.Infrastructure.Internal;

namespace xEFCore.xAS400.Internal {
  public class AS400Options : IAS400Options {

    public virtual void Initialize(IDbContextOptions options) {
      var storeOptions = options.FindExtension<AS400OptionsExtension>() ?? new AS400OptionsExtension();
      RowNumberPagingEnabled = storeOptions.RowNumberPaging ?? false;
      NamingConvention = storeOptions.NamingConvention ?? iDB2NamingConvention.System;
    }

    public virtual void Validate(IDbContextOptions options) {
      var storeOptions = options.FindExtension<AS400OptionsExtension>() ?? new AS400OptionsExtension();

      if (RowNumberPagingEnabled != (storeOptions.RowNumberPaging ?? false)) {
        throw new InvalidOperationException(
            CoreStrings.SingletonOptionChanged(
                nameof(AS400DbContextOptionsBuilder.UseRowNumberForPaging),
                nameof(DbContextOptionsBuilder.UseInternalServiceProvider)));
      }
    }

    public virtual bool RowNumberPagingEnabled { get; private set; }
    public virtual iDB2NamingConvention NamingConvention { get; private set; }

  }
}
