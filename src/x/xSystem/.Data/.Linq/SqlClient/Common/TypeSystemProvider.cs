using System.Data.Common;
using System.Data.Linq.SqlClient;

namespace System.Data.Linq.SqlClient.Common;
internal abstract class TypeSystemProvider {
  internal abstract ProviderType PredictTypeForUnary(SqlNodeType unaryOp, ProviderType operandType);
  internal abstract ProviderType PredictTypeForBinary(SqlNodeType binaryOp, ProviderType leftType, ProviderType rightType);
  internal abstract ProviderType From(Type runtimeType);
  internal abstract ProviderType From(object o);
  internal abstract ProviderType From(Type type, int? size);
  internal abstract ProviderType Parse(string text);
  internal abstract ProviderType GetApplicationType(int index);
  internal abstract ProviderType MostPreciseTypeInFamily(ProviderType type);
  internal abstract ProviderType GetBestLargeType(ProviderType type);
  internal abstract ProviderType GetBestType(ProviderType typeA, ProviderType typeB);
  internal abstract ProviderType ReturnTypeOfFunction(SqlFunctionCall functionCall);
  internal abstract ProviderType ChangeTypeFamilyTo(ProviderType type, ProviderType typeWithFamily);
  internal abstract void InitializeParameter(ProviderType type, DbParameter parameter, object value);
}
