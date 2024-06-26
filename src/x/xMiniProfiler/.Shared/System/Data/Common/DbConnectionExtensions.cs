﻿using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace System.Data.Common;
public static class DbConnectionExtensions {
  public static ProfiledDbConnection AsProfiledDbConnection(this DbConnection dbConnection, IDbProfiler? dbProfiler = null) => new ProfiledDbConnection(dbConnection, dbProfiler ?? MiniProfiler.Current);
  public static DbConnection UnwrapConnection(this DbConnection connection) => connection is ProfiledDbConnection c ? c.WrappedConnection : connection;
}