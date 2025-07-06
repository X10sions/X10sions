using System.Data;

namespace Common.Sql {
  public interface ISqlColumn {
    string Heading { get; set; }
    string Name { get; set; }
    string SqlExpression { get; set; }
    string Sql { get; }
    string SqlSelect { get; }
    DataColumn ToDataColumn();
  }

}
