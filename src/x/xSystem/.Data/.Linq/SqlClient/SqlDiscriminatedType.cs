using System.Data.Linq.Mapping;
using System.Linq.Expressions;

namespace System.Data.Linq.SqlClient {
  internal class SqlDiscriminatedType : SqlExpression {
    private ProviderType sqlType;
    internal override ProviderType SqlType => sqlType;
    internal SqlExpression Discriminator { get; set; }
    internal MetaType TargetType { get; }

    internal SqlDiscriminatedType(ProviderType sqlType, SqlExpression discriminator, MetaType targetType, Expression sourceExpression)
      : base(SqlNodeType.DiscriminatedType, typeof(Type), sourceExpression) {
      Discriminator = discriminator ?? throw System.Data.Linq.SqlClient.Error.ArgumentNull("discriminator");
      TargetType = targetType;
      this.sqlType = sqlType;
    }
  }



}
