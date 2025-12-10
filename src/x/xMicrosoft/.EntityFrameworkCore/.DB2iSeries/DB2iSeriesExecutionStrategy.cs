using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Text;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries;

/*
 
// EFCore.ISeries/ISeriesOptionsExtension.cs (ApplyServices)
services.AddScoped<IExecutionStrategyFactory, ExecutionStrategyFactory>(sp =>
{
    var deps = sp.GetRequiredService<ExecutionStrategyDependencies>();
    return new ExecutionStrategyFactory(deps, () => new ISeriesExecutionStrategy(deps));
});

 */

public sealed class ISeriesExecutionStrategy : ExecutionStrategy {
  public ISeriesExecutionStrategy(ExecutionStrategyDependencies deps) : base(deps, 5, TimeSpan.FromSeconds(2)) { }

  protected override bool ShouldRetryOn(Exception exception) {
    if (exception is OdbcException oex) {
      // IBM i transient codes (example placeholders; replace with your org’s known transient codes)
      foreach (OdbcError err in oex.Errors) {
        if (err.SQLState is "40001" /*deadlock*/ or "08001" /*connection*/ or "08S01" /*communication*/)
          return true;
      }
    }
    return false;
  }
}
