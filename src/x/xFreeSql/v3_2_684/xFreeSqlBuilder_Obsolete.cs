using FreeSql.v3_2_684.Internal;

namespace FreeSql.v3_2_684 {
  public partial class xFreeSqlBuilder_v3_2_684 {

    [Obsolete]
    void EntityPropertyNameConvert_v3_2_684(IFreeSql fsql) {
      if (_entityPropertyConvertType != StringConvertType.None) {
        string PascalCaseToUnderScore(string str) => string.IsNullOrWhiteSpace(str) ? str : string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
        switch (_entityPropertyConvertType) {
          case StringConvertType.Lower:
            fsql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = e.ModifyResult.Name?.ToLower();
            break;
          case StringConvertType.Upper:
            fsql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = e.ModifyResult.Name?.ToUpper();
            break;
          case StringConvertType.PascalCaseToUnderscore:
            fsql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name);
            break;
          case StringConvertType.PascalCaseToUnderscoreWithLower:
            fsql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name)?.ToLower();
            break;
          case StringConvertType.PascalCaseToUnderscoreWithUpper:
            fsql.Aop.ConfigEntityProperty += (_, e) => e.ModifyResult.Name = PascalCaseToUnderScore(e.ModifyResult.Name)?.ToUpper();
            break;
          default:
            break;
        }
      }
    }


  }

}
