using System.Data;
using System.Data.Common;

namespace Common.Data;
public class DbMetaDataCollectionDictionary : Dictionary<string, DataTable> {
  public DbMetaDataCollectionDictionary() {
    DataSourceInformation = null;
    DataTypes = null;
    MetaDataCollections = null;
    ReservedWords = null;
    Restrictions = null;
  }

  public DbMetaDataCollectionDictionary(DbConnection dbConnection) {
    GetSchemas(dbConnection);
  }

  public void GetSchemas(DbConnection dbConnection) {
    DataSourceInformation = dbConnection.GetSchema(DbMetaDataCollectionNames.DataSourceInformation);
    DataTypes = dbConnection.GetSchema(DbMetaDataCollectionNames.DataTypes);
    MetaDataCollections = dbConnection.GetSchema(DbMetaDataCollectionNames.MetaDataCollections);
    ReservedWords = dbConnection.GetSchema(DbMetaDataCollectionNames.ReservedWords);
    Restrictions = dbConnection.GetSchema(DbMetaDataCollectionNames.Restrictions);
  }

  public DataTable? DataSourceInformation { get => this[DbMetaDataCollectionNames.DataSourceInformation]; set => this[DbMetaDataCollectionNames.DataSourceInformation] = value; }
  public DataTable? DataTypes { get => this[DbMetaDataCollectionNames.DataTypes]; set => this[DbMetaDataCollectionNames.DataTypes] = value; }
  public DataTable? MetaDataCollections { get => this[DbMetaDataCollectionNames.MetaDataCollections]; set => this[DbMetaDataCollectionNames.MetaDataCollections] = value; }
  public DataTable? ReservedWords { get => this[DbMetaDataCollectionNames.ReservedWords]; set => this[DbMetaDataCollectionNames.ReservedWords] = value; }
  public DataTable? Restrictions { get => this[DbMetaDataCollectionNames.Restrictions]; set => this[DbMetaDataCollectionNames.Restrictions] = value; }
}