﻿using System.Data.Common;

namespace Common.Data.GetSchemaTyped {
  public class GetSchemaDictionary : DbMetaDataCollectionDictionary {
    public GetSchemaDictionary() { }
    public GetSchemaDictionary(DbConnection dbConnection, bool doSchemasWithRestrictions) : base(dbConnection, doSchemasWithRestrictions) { }
  }
}