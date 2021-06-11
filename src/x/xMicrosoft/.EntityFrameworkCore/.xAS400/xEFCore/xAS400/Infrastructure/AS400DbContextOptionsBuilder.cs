using IBM.Data.DB2.iSeries;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using xEFCore.xAS400.Infrastructure.Internal;
using xEFCore.xAS400.Storage.Internal;

namespace xEFCore.xAS400.Infrastructure {

  public class AS400DbContextOptionsBuilder
      : RelationalDbContextOptionsBuilder<AS400DbContextOptionsBuilder, AS400OptionsExtension> {

    public AS400DbContextOptionsBuilder([NotNull] DbContextOptionsBuilder optionsBuilder)
        : base(optionsBuilder) {
    }

    public virtual void UseNamingConventionDCO(iDB2NamingConvention naming = iDB2NamingConvention.System)
        => WithOption(e => e.WithNamingConvention(naming));

    public virtual void UseRowNumberForPaging(bool useRowNumberForPaging = true)
        => WithOption(e => e.WithRowNumberPaging(useRowNumberForPaging));

    public virtual AS400DbContextOptionsBuilder EnableRetryOnFailure()
        => ExecutionStrategy(c => new AS400RetryingExecutionStrategy(c));

    public virtual AS400DbContextOptionsBuilder EnableRetryOnFailure(int maxRetryCount)
        => ExecutionStrategy(c => new AS400RetryingExecutionStrategy(c, maxRetryCount));

    public virtual AS400DbContextOptionsBuilder EnableRetryOnFailure(
        int maxRetryCount,
        TimeSpan maxRetryDelay,
        [NotNull] ICollection<int> errorNumbersToAdd)
        => ExecutionStrategy(c => new AS400RetryingExecutionStrategy(c, maxRetryCount, maxRetryDelay, errorNumbersToAdd));

  }
}