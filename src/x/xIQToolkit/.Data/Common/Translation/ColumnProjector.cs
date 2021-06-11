// Copyright (c) Microsoft Corporation.  All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (MS-PL)

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace IQToolkit.Data.Common {
  /// <summary>
  /// Result from calling ColumnProjector.ProjectColumns
  /// </summary>
  public sealed class ProjectedColumns {
    private readonly Expression projector;
    private readonly ReadOnlyCollection<ColumnDeclaration> columns;

    public ProjectedColumns(Expression projector, ReadOnlyCollection<ColumnDeclaration> columns) {
      this.projector = projector;
      this.columns = columns;
    }

    /// <summary>
    /// The expression to computed on the client.
    /// </summary>
    public Expression Projector => projector;

    /// <summary>
    /// The columns to be computed on the server.
    /// </summary>
    public ReadOnlyCollection<ColumnDeclaration> Columns => columns;
  }

  public enum ProjectionAffinity {
    /// <summary>
    /// Prefer expression computation on the client
    /// </summary>
    Client,

    /// <summary>
    /// Prefer expression computation on the server
    /// </summary>
    Server
  }

  /// <summary>
  /// Splits an expression into two parts
  ///   1) a list of column declarations for sub-expressions that must be evaluated on the server
  ///   2) a expression that describes how to combine/project the columns back together into the correct result
  /// </summary>
  public class ColumnProjector : DbExpressionVisitor {
    private readonly QueryLanguage language;
    private readonly Dictionary<ColumnExpression, ColumnExpression> map;
    private readonly List<ColumnDeclaration> columns;
    private readonly HashSet<string> columnNames;
    private readonly HashSet<Expression> candidates;
    private readonly HashSet<TableAlias> existingAliases;
    private readonly TableAlias newAlias;
    private int iColumn;

    private ColumnProjector(QueryLanguage language, ProjectionAffinity affinity, Expression expression, IEnumerable<ColumnDeclaration> existingColumns, TableAlias newAlias, IEnumerable<TableAlias> existingAliases) {
      this.language = language;
      this.newAlias = newAlias;
      this.existingAliases = new HashSet<TableAlias>(existingAliases);
      map = new Dictionary<ColumnExpression, ColumnExpression>();
      if (existingColumns != null) {
        columns = new List<ColumnDeclaration>(existingColumns);
        columnNames = new HashSet<string>(existingColumns.Select(c => c.Name));
      } else {
        columns = new List<ColumnDeclaration>();
        columnNames = new HashSet<string>();
      }
      candidates = Nominator.Nominate(language, affinity, expression);
    }

    public static ProjectedColumns ProjectColumns(QueryLanguage language, ProjectionAffinity affinity, Expression expression, IEnumerable<ColumnDeclaration> existingColumns, TableAlias newAlias, IEnumerable<TableAlias> existingAliases) {
      var projector = new ColumnProjector(language, affinity, expression, existingColumns, newAlias, existingAliases);
      var expr = projector.Visit(expression);
      return new ProjectedColumns(expr, projector.columns.AsReadOnly());
    }

    public static ProjectedColumns ProjectColumns(QueryLanguage language, Expression expression, IEnumerable<ColumnDeclaration> existingColumns, TableAlias newAlias, IEnumerable<TableAlias> existingAliases) => ProjectColumns(language, ProjectionAffinity.Client, expression, existingColumns, newAlias, existingAliases);

    public static ProjectedColumns ProjectColumns(QueryLanguage language, ProjectionAffinity affinity, Expression expression, IEnumerable<ColumnDeclaration> existingColumns, TableAlias newAlias, params TableAlias[] existingAliases) => ProjectColumns(language, affinity, expression, existingColumns, newAlias, (IEnumerable<TableAlias>)existingAliases);

    public static ProjectedColumns ProjectColumns(QueryLanguage language, Expression expression, IEnumerable<ColumnDeclaration> existingColumns, TableAlias newAlias, params TableAlias[] existingAliases) => ProjectColumns(language, expression, existingColumns, newAlias, (IEnumerable<TableAlias>)existingAliases);

    public override Expression Visit(Expression expression) {
      if (candidates.Contains(expression)) {
        if (expression.NodeType == (ExpressionType)DbExpressionType.Column) {
          var column = (ColumnExpression)expression;
          ColumnExpression mapped;
          if (map.TryGetValue(column, out mapped)) {
            return mapped;
          }

          // check for column that already refers to this column
          foreach (var existingColumn in columns) {
            var cex = existingColumn.Expression as ColumnExpression;
            if (cex != null && cex.Alias == column.Alias && cex.Name == column.Name) {
              // refer to the column already in the column list
              return new ColumnExpression(column.Type, column.QueryType, newAlias, existingColumn.Name);
            }
          }

          if (existingAliases.Contains(column.Alias)) {
            var ordinal = columns.Count;
            var columnName = GetUniqueColumnName(column.Name);
            columns.Add(new ColumnDeclaration(columnName, column, column.QueryType));
            mapped = new ColumnExpression(column.Type, column.QueryType, newAlias, columnName);
            map.Add(column, mapped);
            columnNames.Add(columnName);
            return mapped;
          }

          // must be referring to outer scope
          return column;
        } else {
          var columnName = GetNextColumnName();
          var colType = language.TypeSystem.GetColumnType(expression.Type);
          columns.Add(new ColumnDeclaration(columnName, expression, colType));
          return new ColumnExpression(expression.Type, colType, newAlias, columnName);
        }
      } else {
        return base.Visit(expression);
      }
    }

    private bool IsColumnNameInUse(string name) => columnNames.Contains(name);

    private string GetUniqueColumnName(string name) {
      var baseName = name;
      var suffix = 1;
      while (IsColumnNameInUse(name)) {
        name = baseName + (suffix++);
      }
      return name;
    }

    private string GetNextColumnName() => GetUniqueColumnName("c" + (iColumn++));

    /// <summary>
    /// Nominator is a class that walks an expression tree bottom up, determining the set of 
    /// candidate expressions that are possible columns of a select expression
    /// </summary>
    class Nominator : DbExpressionVisitor {
      private readonly QueryLanguage language;
      private readonly HashSet<Expression> candidates;
      private readonly ProjectionAffinity affinity;
      private bool isBlocked;

      private Nominator(QueryLanguage language, ProjectionAffinity affinity) {
        this.language = language;
        this.affinity = affinity;
        candidates = new HashSet<Expression>();
        isBlocked = false;
      }

      internal static HashSet<Expression> Nominate(QueryLanguage language, ProjectionAffinity affinity, Expression expression) {
        var nominator = new Nominator(language, affinity);
        nominator.Visit(expression);
        return nominator.candidates;
      }

      public override Expression Visit(Expression expression) {
        if (expression != null) {
          var saveIsBlocked = isBlocked;
          isBlocked = false;
          if (language.MustBeColumn(expression)) {
            candidates.Add(expression);
            // don't merge saveIsBlocked
          } else {
            base.Visit(expression);
            if (!isBlocked) {
              if (language.MustBeColumn(expression)
                  || (affinity == ProjectionAffinity.Server && language.CanBeColumn(expression))) {
                candidates.Add(expression);
              } else {
                isBlocked = true;
              }
            }
            isBlocked |= saveIsBlocked;
          }
        }
        return expression;
      }

      protected override Expression VisitProjection(ProjectionExpression proj) {
        Visit(proj.Projector);
        return proj;
      }
    }
  }
}
