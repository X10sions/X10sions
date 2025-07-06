using System.Collections;
using System.Data.Common;
using System.Data.Odbc;

namespace Common.Data.Odbc {
  public class _BaseOdbcConnectionStringBuilder {
    public _BaseOdbcConnectionStringBuilder() { }

    public _BaseOdbcConnectionStringBuilder(string connectionString) {
      ConnectionString = connectionString;
    }
    OdbcConnectionStringBuilder csb = new OdbcConnectionStringBuilder();
    #region OdbcConnectionStringBuilder
    public string Driver { get => csb.Driver; set => csb.Driver = value; }
    public string Dsn { get => csb.Dsn; set => csb.Dsn = value; }
    #endregion
    #region DbConnectionStringBuilder
    public object this[string keyword] { get => csb[keyword]; set => csb[keyword] = value; }
    public bool IsReadOnly => csb.IsReadOnly;
    public virtual bool IsFixedSize => csb.IsFixedSize;
    public virtual int Count => csb.Count;
    //public virtual string ConnectionString { get => csb.ConnectionString; set => csb.ConnectionString = value; }
    public virtual string ConnectionString { get => csb.ConnectionString; set => this.SetConnectionString(value); }
    public bool BrowsableConnectionString { get => csb.BrowsableConnectionString; set => csb.BrowsableConnectionString = value; }
    public virtual ICollection Values => csb.Values;
    public ICollection Keys => csb.Keys;
    public void Add(string keyword, object value) => csb.Add(keyword, value);
    public void Clear() => csb.Clear();
    public bool ContainsKey(string keyword) => csb.ContainsKey(keyword);
    public virtual bool EquivalentTo(DbConnectionStringBuilder connectionStringBuilder) => csb.EquivalentTo(connectionStringBuilder);
    public bool Remove(string keyword) => csb.Remove(keyword);
    public virtual bool ShouldSerialize(string keyword) => csb.ShouldSerialize(keyword);
    public override string ToString() => csb.ToString();
    public bool TryGetValue(string keyword, out object value) => csb.TryGetValue(keyword, out value);
    #endregion
  }
}
