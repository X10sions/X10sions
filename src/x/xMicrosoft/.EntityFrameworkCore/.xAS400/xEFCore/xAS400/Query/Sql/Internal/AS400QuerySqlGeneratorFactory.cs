using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.Sql;
using System;
using xEFCore.xAS400.Infrastructure.Internal;

namespace xEFCore.xAS400.Query.Sql.Internal {
  public class AS400QuerySqlGeneratorFactory : QuerySqlGeneratorFactoryBase {
    private readonly IAS400Options _AS400Options; public AS400QuerySqlGeneratorFactory([NotNull] QuerySqlGeneratorDependencies dependencies,
         [NotNull] IAS400Options AS400Options)
         : base(dependencies) {
      _AS400Options = AS400Options;
    }
    public override IQuerySqlGenerator CreateDefault(SelectExpression selectExpression)
       => new AS400QuerySqlGenerator(
           Dependencies,
           Check.NotNull(selectExpression, nameof(selectExpression)),
           _AS400Options.RowNumberPagingEnabled);
  }
}
