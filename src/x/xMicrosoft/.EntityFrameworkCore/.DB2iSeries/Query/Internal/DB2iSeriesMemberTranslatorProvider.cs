using Microsoft.EntityFrameworkCore.Query;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Query.Internal;

 class DB2iSeriesMemberTranslatorProvider : RelationalMemberTranslatorProvider {

  public DB2iSeriesMemberTranslatorProvider(RelationalMemberTranslatorProviderDependencies dependencies) :base(dependencies){
    var sqlExpressionFactory = dependencies.SqlExpressionFactory;
    AddTranslators(
      [new DB2iSeriesDateTimeMemberTranslator(sqlExpressionFactory)
    ]);
  }

}