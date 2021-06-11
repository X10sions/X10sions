﻿using RepoDb.Exceptions;
using RepoDb.Extensions;
using RepoDb.Interfaces;
using RepoDb.Resolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace RepoDb.StatementBuilders {
  public class DB2iSeries_SqlServerStatementBuilder<TConnection> : BaseStatementBuilder where TConnection : DbConnection, IDbConnection {

    public DB2iSeries_SqlServerStatementBuilder(IDbSetting dbSetting)
      : this(dbSetting, new DB2iSeriesConvertFieldResolver(), new ClientTypeToAverageableClientTypeResolver()) { }

    public DB2iSeries_SqlServerStatementBuilder(IDbSetting dbSetting, IResolver<Field, IDbSetting, string> convertFieldResolver = null, IResolver<Type, Type> averageableClientTypeResolver = null)
      : base(dbSetting, convertFieldResolver ?? new DB2iSeriesConvertFieldResolver(), averageableClientTypeResolver ?? new ClientTypeToAverageableClientTypeResolver()) { }

    #region CreateBatchQuery

    /// <summary>
    /// Creates a SQL Statement for batch query operation.
    /// </summary>
    /// <param name="queryBuilder">The query builder to be used.</param>
    /// <param name="tableName">The name of the target table.</param>
    /// <param name="fields">The list of fields to be queried.</param>
    /// <param name="page">The page of the batch.</param>
    /// <param name="rowsPerBatch">The number of rows per batch.</param>
    /// <param name="orderBy">The list of fields for ordering.</param>
    /// <param name="where">The query expression.</param>
    /// <param name="hints">The table hints to be used.</param>
    /// <returns>A sql statement for batch query operation.</returns>
    public override string CreateBatchQuery(QueryBuilder queryBuilder,
        string tableName,
        IEnumerable<Field> fields,
        int? page,
        int? rowsPerBatch,
        IEnumerable<OrderField> orderBy = null,
        QueryGroup where = null,
        string hints = null) {
      // Ensure with guards
      GuardTableName(tableName);

      // Validate the hints
      GuardHints(hints);

      // There should be fields
      if (fields?.Any() != true) {
        throw new NullReferenceException($"The list of queryable fields must not be null for '{tableName}'.");
      }

      // Validate order by
      if (orderBy == null || orderBy?.Any() != true) {
        throw new EmptyException("The argument 'orderBy' is required.");
      }

      // Validate the page
      if (page == null || page < 0) {
        throw new ArgumentOutOfRangeException("The page must be equals or greater than 0.");
      }

      // Validate the page
      if (rowsPerBatch == null || rowsPerBatch < 1) {
        throw new ArgumentOutOfRangeException($"The rows per batch must be equals or greater than 1.");
      }

      // Initialize the builder
      var builder = queryBuilder ?? new QueryBuilder();

      // Build the query
      builder.Clear()
          .With()
          .WriteText("CTE")
          .As()
          .OpenParen()
          .Select()
          .RowNumber()
          .Over()
          .OpenParen()
          .OrderByFrom(orderBy, DbSetting)
          .CloseParen()
          .As("[RowNumber],")
          .FieldsFrom(fields, DbSetting)
          .From()
          .TableNameFrom(tableName, DbSetting)
          .HintsFrom(hints)
          .WhereFrom(where, DbSetting)
          .CloseParen()
          .Select()
          .FieldsFrom(fields, DbSetting)
          .From()
          .WriteText("CTE")
          .WriteText(string.Concat("WHERE ([RowNumber] BETWEEN ", (page * rowsPerBatch) + 1, " AND ", (page + 1) * rowsPerBatch, ")"))
          .OrderByFrom(orderBy, DbSetting)
          .End();

      // Return the query
      return builder.GetString();
    }

    #endregion

    #region CreateCount

    /// <summary>
    /// Creates a SQL Statement for count operation.
    /// </summary>
    /// <param name="queryBuilder">The query builder to be used.</param>
    /// <param name="tableName">The name of the target table.</param>
    /// <param name="where">The query expression.</param>
    /// <param name="hints">The table hints to be used.</param>
    /// <returns>A sql statement for count operation.</returns>
    public override string CreateCount(QueryBuilder queryBuilder,
        string tableName,
        QueryGroup where = null,
        string hints = null) {
      // Ensure with guards
      GuardTableName(tableName);

      // Validate the hints
      GuardHints(hints);

      // Initialize the builder
      var builder = queryBuilder ?? new QueryBuilder();

      // Build the query
      builder.Clear()
          .Select()
          .CountBig(null, DbSetting)
          .WriteText("AS [CountValue]")
          .From()
          .TableNameFrom(tableName, DbSetting)
          .HintsFrom(hints)
          .WhereFrom(where, DbSetting)
          .End();

      // Return the query
      return builder.GetString();
    }

    #endregion

    #region CreateCountAll

    /// <summary>
    /// Creates a SQL Statement for count-all operation.
    /// </summary>
    /// <param name="queryBuilder">The query builder to be used.</param>
    /// <param name="tableName">The name of the target table.</param>
    /// <param name="hints">The table hints to be used.</param>
    /// <returns>A sql statement for count-all operation.</returns>
    public override string CreateCountAll(QueryBuilder queryBuilder,
        string tableName,
        string hints = null) {
      // Ensure with guards
      GuardTableName(tableName);

      // Validate the hints
      GuardHints(hints);

      // Initialize the builder
      var builder = queryBuilder ?? new QueryBuilder();

      // Build the query
      builder.Clear()
          .Select()
          .CountBig(null, DbSetting)
          .WriteText("AS [CountValue]")
          .From()
          .TableNameFrom(tableName, DbSetting)
          .HintsFrom(hints)
          .End();

      // Return the query
      return builder.GetString();
    }

    #endregion

    #region CreateInsert

    /// <summary>
    /// Creates a SQL Statement for insert operation.
    /// </summary>
    /// <param name="queryBuilder">The query builder to be used.</param>
    /// <param name="tableName">The name of the target table.</param>
    /// <param name="fields">The list of fields to be inserted.</param>
    /// <param name="primaryField">The primary field from the database.</param>
    /// <param name="identityField">The identity field from the database.</param>
    /// <param name="hints">The table hints to be used.</param>
    /// <returns>A sql statement for insert operation.</returns>
    public override string CreateInsert(QueryBuilder queryBuilder,
        string tableName,
        IEnumerable<Field> fields = null,
        DbField primaryField = null,
        DbField identityField = null,
        string hints = null) {
      // Initialize the builder
      var builder = queryBuilder ?? new QueryBuilder();

      // Call the base
      base.CreateInsert(builder,
          tableName,
          fields,
          primaryField,
          identityField,
          hints);

      // Variables needed
      var databaseType = "BIGINT";

      // Check for the identity
      if (identityField != null) {
        var dbType = new ClientTypeToDbTypeResolver().Resolve(identityField.Type);
        if (dbType != null) {
          databaseType = new DbTypeToDB2iSeriesStringNameResolver().Resolve(dbType.Value);
        }
      }

      // Set the return value
      var result = identityField != null ?
          string.Concat("CONVERT(", databaseType, ", SCOPE_IDENTITY())") :
              primaryField != null ? primaryField.Name.AsParameter(DbSetting) : "NULL";

      builder
          .Select()
          .WriteText(result)
          .As("[Result]")
          .End();

      // Return the query
      return builder.GetString();
    }

    #endregion

    #region CreateInsertAll

    /// <summary>
    /// Creates a SQL Statement for insert-all operation.
    /// </summary>
    /// <param name="queryBuilder">The query builder to be used.</param>
    /// <param name="tableName">The name of the target table.</param>
    /// <param name="fields">The list of fields to be inserted.</param>
    /// <param name="batchSize">The batch size of the operation.</param>
    /// <param name="primaryField">The primary field from the database.</param>
    /// <param name="identityField">The identity field from the database.</param>
    /// <param name="hints">The table hints to be used.</param>
    /// <returns>A sql statement for insert operation.</returns>
    public override string CreateInsertAll(QueryBuilder queryBuilder,
        string tableName,
        IEnumerable<Field> fields = null,
        int batchSize = Constant.DefaultBatchOperationSize,
        DbField primaryField = null,
        DbField identityField = null,
        string hints = null) {
      // Initialize the builder
      var builder = queryBuilder ?? new QueryBuilder();

      // Call the base
      var commandText = base.CreateInsertAll(builder,
          tableName,
          fields,
          batchSize,
          primaryField,
          identityField,
          hints);

      // Variables needed
      var databaseType = (string)null;

      // Check for the identity
      if (identityField != null) {
        var dbType = new ClientTypeToDbTypeResolver().Resolve(identityField.Type);
        if (dbType != null) {
          databaseType = new DbTypeToDB2iSeriesStringNameResolver().Resolve(dbType.Value);
        }
      }

      if (identityField != null) {
        // Variables needed
        var commandTexts = new List<string>();
        var splitted = commandText.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        // Iterate the indexes
        for (var index = 0; index < splitted.Count(); index++) {
          var line = splitted[index].Trim();
          var returnValue = string.IsNullOrEmpty(databaseType) ?
                  "SELECT SCOPE_IDENTITY()" :
                  $"SELECT CONVERT({databaseType}, SCOPE_IDENTITY())";
          commandTexts.Add(string.Concat(line, " ; ", returnValue, " ;"));
        }

        // Set the command text
        commandText = commandTexts.Join(" ");
      }

      // Return the query
      return commandText;
    }

    #endregion

    #region CreateMerge

    /// <summary>
    /// Creates a SQL Statement for merge operation.
    /// </summary>
    /// <param name="queryBuilder">The query builder to be used.</param>
    /// <param name="tableName">The name of the target table.</param>
    /// <param name="fields">The list of fields to be merged.</param>
    /// <param name="qualifiers">The list of the qualifier <see cref="Field"/> objects.</param>
    /// <param name="primaryField">The primary field from the database.</param>
    /// <param name="identityField">The identity field from the database.</param>
    /// <param name="hints">The table hints to be used.</param>
    /// <returns>A sql statement for merge operation.</returns>
    public override string CreateMerge(QueryBuilder queryBuilder,
        string tableName,
        IEnumerable<Field> fields,
        IEnumerable<Field> qualifiers = null,
        DbField primaryField = null,
        DbField identityField = null,
        string hints = null) {
      // Ensure with guards
      GuardTableName(tableName);
      GuardHints(hints);
      GuardPrimary(primaryField);
      GuardIdentity(identityField);

      // Verify the fields
      if (fields?.Any() != true) {
        throw new NullReferenceException($"The list of fields cannot be null or empty.");
      }

      // Check the qualifiers
      if (qualifiers?.Any() == true) {
        // Check if the qualifiers are present in the given fields
        var unmatchesQualifiers = qualifiers.Where(field =>
            fields.FirstOrDefault(f =>
                string.Equals(field.Name, f.Name, StringComparison.OrdinalIgnoreCase)) == null);

        // Throw an error we found any unmatches
        if (unmatchesQualifiers?.Any() == true) {
          throw new InvalidQualifiersException($"The qualifiers '{unmatchesQualifiers.Select(field => field.Name).Join(", ")}' are not " +
              $"present at the given fields '{fields.Select(field => field.Name).Join(", ")}'.");
        }
      } else {
        if (primaryField != null) {
          // Make sure that primary is present in the list of fields before qualifying to become a qualifier
          var isPresent = fields?.FirstOrDefault(f => string.Equals(f.Name, primaryField.Name, StringComparison.OrdinalIgnoreCase)) != null;

          // Throw if not present
          if (isPresent == false) {
            throw new InvalidQualifiersException($"There are no qualifier field objects found for '{tableName}'. Ensure that the " +
                $"primary field is present at the given fields '{fields.Select(field => field.Name).Join(", ")}'.");
          }

          // The primary is present, use it as a default if there are no qualifiers given
          qualifiers = primaryField.AsField().AsEnumerable();
        } else {
          // Throw exception, qualifiers are not defined
          throw new NullReferenceException($"There are no qualifier field objects found for '{tableName}'.");
        }
      }

      // Get the insertable and updateable fields
      var insertableFields = fields
          .Where(field => !string.Equals(field.Name, identityField?.Name, StringComparison.OrdinalIgnoreCase));
      var updateableFields = fields
          .Where(field => !string.Equals(field.Name, primaryField?.Name, StringComparison.OrdinalIgnoreCase) &&
              !string.Equals(field.Name, identityField?.Name, StringComparison.OrdinalIgnoreCase));

      // Variables needed
      var databaseType = "BIGINT";

      // Check for the identity
      if (identityField != null) {
        var dbType = new ClientTypeToDbTypeResolver().Resolve(identityField.Type);
        if (dbType != null) {
          databaseType = new DbTypeToDB2iSeriesStringNameResolver().Resolve(dbType.Value);
        }
      }

      // Initialize the builder
      var builder = queryBuilder ?? new QueryBuilder();

      // Build the query
      builder.Clear()
          // MERGE T USING S
          .Merge()
          .TableNameFrom(tableName, DbSetting)
          .As("T")
          .HintsFrom(hints)
          .Using()
          .OpenParen()
          .Select()
          .ParametersAsFieldsFrom(fields, 0, DbSetting)
          .CloseParen()
          .As("S")
          // QUALIFIERS
          .On()
          .OpenParen()
          .WriteText(qualifiers?
              .Select(
                  field => field.AsJoinQualifier("S", "T", DbSetting))
                      .Join(" AND "))
          .CloseParen()
          // WHEN NOT MATCHED THEN INSERT VALUES
          .When()
          .Not()
          .Matched()
          .Then()
          .Insert()
          .OpenParen()
          .FieldsFrom(insertableFields, DbSetting)
          .CloseParen()
          .Values()
          .OpenParen()
          .AsAliasFieldsFrom(insertableFields, "S", DbSetting)
          .CloseParen()
          // WHEN MATCHED THEN UPDATE SET
          .When()
          .Matched()
          .Then()
          .Update()
          .Set()
          .FieldsAndAliasFieldsFrom(updateableFields, "T", "S", DbSetting);

      // Set the output
      var outputField = identityField ?? primaryField;
      if (outputField != null) {
        queryBuilder
            .WriteText(string.Concat("OUTPUT INSERTED.", outputField.Name.AsField(DbSetting)))
            .As("[Result]");
      }

      // End the builder
      queryBuilder.End();

      // Return the query
      return builder.GetString();
    }

    #endregion

    #region CreateMergeAll

    /// <summary>
    /// Creates a SQL Statement for merge-all operation.
    /// </summary>
    /// <param name="queryBuilder">The query builder to be used.</param>
    /// <param name="tableName">The name of the target table.</param>
    /// <param name="fields">The list of fields to be merged.</param>
    /// <param name="qualifiers">The list of the qualifier <see cref="Field"/> objects.</param>
    /// <param name="batchSize">The batch size of the operation.</param>
    /// <param name="primaryField">The primary field from the database.</param>
    /// <param name="identityField">The identity field from the database.</param>
    /// <param name="hints">The table hints to be used.</param>
    /// <returns>A sql statement for merge operation.</returns>
    public override string CreateMergeAll(QueryBuilder queryBuilder,
        string tableName,
        IEnumerable<Field> fields,
        IEnumerable<Field> qualifiers = null,
        int batchSize = Constant.DefaultBatchOperationSize,
        DbField primaryField = null,
        DbField identityField = null,
        string hints = null) {
      // Ensure with guards
      GuardTableName(tableName);
      GuardHints(hints);
      GuardPrimary(primaryField);
      GuardIdentity(identityField);

      // Verify the fields
      if (fields?.Any() != true) {
        throw new NullReferenceException($"The list of fields cannot be null or empty.");
      }

      // Check the qualifiers
      if (qualifiers?.Any() == true) {
        // Check if the qualifiers are present in the given fields
        var unmatchesQualifiers = qualifiers.Where(field =>
            fields.FirstOrDefault(f =>
                string.Equals(field.Name, f.Name, StringComparison.OrdinalIgnoreCase)) == null);

        // Throw an error we found any unmatches
        if (unmatchesQualifiers?.Any() == true) {
          throw new InvalidQualifiersException($"The qualifiers '{unmatchesQualifiers.Select(field => field.Name).Join(", ")}' are not " +
              $"present at the given fields '{fields.Select(field => field.Name).Join(", ")}'.");
        }
      } else {
        if (primaryField != null) {
          // Make sure that primary is present in the list of fields before qualifying to become a qualifier
          var isPresent = fields?.FirstOrDefault(f => string.Equals(f.Name, primaryField.Name, StringComparison.OrdinalIgnoreCase)) != null;

          // Throw if not present
          if (isPresent == false) {
            throw new InvalidQualifiersException($"There are no qualifier field objects found for '{tableName}'. Ensure that the " +
                $"primary field is present at the given fields '{fields.Select(field => field.Name).Join(", ")}'.");
          }

          // The primary is present, use it as a default if there are no qualifiers given
          qualifiers = primaryField.AsField().AsEnumerable();
        } else {
          // Throw exception, qualifiers are not defined
          throw new NullReferenceException($"There are no qualifier field objects found for '{tableName}'.");
        }
      }

      // Get the insertable and updateable fields
      var insertableFields = fields
          .Where(field => !string.Equals(field.Name, identityField?.Name, StringComparison.OrdinalIgnoreCase));
      var updateableFields = fields
          .Where(field => !string.Equals(field.Name, primaryField?.Name, StringComparison.OrdinalIgnoreCase) &&
              !string.Equals(field.Name, identityField?.Name, StringComparison.OrdinalIgnoreCase));

      // Variables needed
      var databaseType = (string)null;

      // Check for the identity
      if (identityField != null) {
        var dbType = new ClientTypeToDbTypeResolver().Resolve(identityField.Type);
        if (dbType != null) {
          databaseType = new DbTypeToDB2iSeriesStringNameResolver().Resolve(dbType.Value);
        }
      } else if (primaryField != null) {
        var dbType = new ClientTypeToDbTypeResolver().Resolve(primaryField.Type);
        if (dbType != null) {
          databaseType = new DbTypeToDB2iSeriesStringNameResolver().Resolve(dbType.Value);
        }
      }
      // Initialize the builder
      var builder = queryBuilder ?? new QueryBuilder();

      // Build the query
      builder.Clear();

      // Iterate the indexes
      for (var index = 0; index < batchSize; index++) {
        // MERGE T USING S
        queryBuilder.Merge()
            .TableNameFrom(tableName, DbSetting)
            .As("T")
            .HintsFrom(hints)
            .Using()
            .OpenParen()
            .Select()
            .ParametersAsFieldsFrom(fields, index, DbSetting)
            .CloseParen()
            .As("S")
            // QUALIFIERS
            .On()
            .OpenParen()
            .WriteText(qualifiers?
                .Select(
                    field => field.AsJoinQualifier("S", "T", DbSetting))
                        .Join(" AND "))
            .CloseParen()
            // WHEN NOT MATCHED THEN INSERT VALUES
            .When()
            .Not()
            .Matched()
            .Then()
            .Insert()
            .OpenParen()
            .FieldsFrom(insertableFields, DbSetting)
            .CloseParen()
            .Values()
            .OpenParen()
            .AsAliasFieldsFrom(insertableFields, "S", DbSetting)
            .CloseParen()
            // WHEN MATCHED THEN UPDATE SET
            .When()
            .Matched()
            .Then()
            .Update()
            .Set()
            .FieldsAndAliasFieldsFrom(updateableFields, "T", "S", DbSetting);

        // Set the output
        var outputField = identityField ?? primaryField;
        if (outputField != null) {
          queryBuilder
              .WriteText(string.Concat("OUTPUT INSERTED.", outputField.Name.AsField(DbSetting)))
              .As("[Result]");
        }

        // End the builder
        queryBuilder.End();
      }

      // Return the query
      return builder.GetString();
    }

    #endregion

  }
}