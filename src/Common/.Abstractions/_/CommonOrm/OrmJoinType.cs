using System;
using System.Linq;
namespace CommonOrm {
  public enum OrmJoinType {
    //Auto = 0,
    Inner = 1,
    Left = 2,
    //CrossApply = 3,
    //OuterApply = 4,
    //Right = 5,
    //Full = 6
  }

  public static class OrmJoinTypeExtensions {

    public static OrmJoinTypeEnumClass GetEnumClass(this OrmJoinType o) {

      return new OrmJoinTypeEnumClass {
        CanBeNull = !(new[] { OrmJoinType.Inner }.Contains(o))
      };
    }

  }

  public class OrmJoinTypeEnumClass {
    public bool CanBeNull { get; set; }

  }


}