using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Text;
using System.Data.Linq;
//using System.Data.Linq.Mapping;
using System.Data.Linq.Provider;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Data.Linq.SqlClient.Common;
using System.Data.Linq.Mapping;

namespace System.Data.Linq.SqlClient.Query;
internal class Translator {
  private class RelationComposer : ExpressionVisitor {
    private ParameterExpression parameter;
    private MetaAssociation association;
    private Expression otherSouce;
    private Expression parameterReplacement;

    internal RelationComposer(ParameterExpression parameter, MetaAssociation association, Expression otherSouce, Expression parameterReplacement) {
      this.parameter = parameter ?? throw Error.ArgumentNull(nameof(parameter));
      this.association = association ?? throw Error.ArgumentNull(nameof(association));
      this.otherSouce = otherSouce ?? throw Error.ArgumentNull(nameof(otherSouce));
      this.parameterReplacement = parameterReplacement ?? throw Error.ArgumentNull(nameof(parameterReplacement));
    }

    internal override Expression VisitParameter(ParameterExpression p) {
      if (p == parameter) {
        return parameterReplacement;
      }
      return base.VisitParameter(p);
    }

    private static Expression[] GetKeyValues(Expression expr, ReadOnlyCollection<MetaDataMember> keys) {
      var list = new List<Expression>();
      foreach (var key in keys) {
        list.Add(Expression.PropertyOrField(expr, key.Name));
      }
      return list.ToArray();
    }

    internal override Expression VisitMemberAccess(MemberExpression m) {
      if (MetaPosition.AreSameMember(m.Member, association.ThisMember.Member)) {
        var keyValues = GetKeyValues(Visit(m.Expression), association.ThisKey);
        return WhereClauseFromSourceAndKeys(otherSouce, association.OtherKey.ToArray(), keyValues);
      }
      var expression = Visit(m.Expression);
      if (expression != m.Expression) {
        if (expression.Type != m.Expression.Type && m.Member.Name == "Count" && TypeSystem.IsSequenceType(expression.Type)) {
          return Expression.Call(typeof(Enumerable), "Count", new Type[1]
          {
            TypeSystem.GetElementType(expression.Type)
          }, expression);
        }
        return Expression.MakeMemberAccess(expression, m.Member);
      }
      return m;
    }
  }

  private IDataServices services;

  private SqlFactory sql;

  private TypeSystemProvider typeProvider;

  internal Translator(IDataServices services, SqlFactory sqlFactory, TypeSystemProvider typeProvider) {
    this.services = services;
    sql = sqlFactory;
    this.typeProvider = typeProvider;
  }

  internal SqlSelect BuildDefaultQuery(MetaType rowType, bool allowDeferred, SqlLink link, Expression source) {
    if (rowType.HasInheritance && rowType.InheritanceRoot != rowType) {
      throw Error.ArgumentWrongValue("rowType");
    }
    var sqlTable = sql.Table(rowType.Table, rowType, source);
    var sqlAlias = new SqlAlias(sqlTable);
    var item = new SqlAliasRef(sqlAlias);
    var selection = BuildProjection(item, sqlTable.RowType, allowDeferred, link, source);
    return new SqlSelect(selection, sqlAlias, source);
  }

  internal SqlExpression BuildProjection(SqlExpression item, MetaType rowType, bool allowDeferred, SqlLink link, Expression source) {
    if (!rowType.HasInheritance) {
      return BuildProjectionInternal(item, rowType, (rowType.Table != null) ? rowType.PersistentDataMembers : rowType.DataMembers, allowDeferred, link, source);
    }
    var list = new List<MetaType>(rowType.InheritanceTypes);
    var list2 = new List<SqlTypeCaseWhen>();
    SqlTypeCaseWhen sqlTypeCaseWhen = null;
    var inheritanceRoot = rowType.InheritanceRoot;
    var discriminator = inheritanceRoot.Discriminator;
    var type = discriminator.Type;
    var sqlMember = sql.Member(item, discriminator.Member);
    foreach (var item2 in list) {
      if (item2.HasInheritanceCode) {
        var typeBinding = BuildProjectionInternal(item, item2, item2.PersistentDataMembers, allowDeferred, link, source);
        if (item2.IsInheritanceDefault) {
          sqlTypeCaseWhen = new SqlTypeCaseWhen(null, typeBinding);
        }
        object value = InheritanceRules.InheritanceCodeForClientCompare(item2.InheritanceCode, sqlMember.SqlType);
        var match = sql.Value(type, sql.Default(discriminator), value, true, source);
        list2.Add(new SqlTypeCaseWhen(match, typeBinding));
      }
    }
    if (sqlTypeCaseWhen == null) {
      throw Error.EmptyCaseNotSupported();
    }
    list2.Add(sqlTypeCaseWhen);
    return sql.TypeCase(inheritanceRoot.Type, inheritanceRoot, sqlMember, list2.ToArray(), source);
  }

  private bool IsPreloaded(MemberInfo member) {
    if (services.Context.LoadOptions == null) {
      return false;
    }
    return services.Context.LoadOptions.IsPreloaded(member);
  }

  private SqlNew BuildProjectionInternal(SqlExpression item, MetaType rowType, IEnumerable<MetaDataMember> members, bool allowDeferred, SqlLink link, Expression source) {
    var list = new List<SqlMemberAssign>();
    foreach (var member in members) {
      if (allowDeferred && (member.IsAssociation || member.IsDeferred)) {
        if (link != null && member != link.Member && member.IsAssociation && member.MappedName == link.Member.MappedName && !member.Association.IsMany && !IsPreloaded(link.Member.Member)) {
          var sqlLink = BuildLink(item, member, source);
          sqlLink.Expansion = link.Expression;
          list.Add(new SqlMemberAssign(member.Member, sqlLink));
        } else {
          list.Add(new SqlMemberAssign(member.Member, BuildLink(item, member, source)));
        }
      } else if (!member.IsAssociation) {
        list.Add(new SqlMemberAssign(member.Member, sql.Member(item, member)));
      }
    }
    var constructor = rowType.Type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
    if (constructor == null) {
      throw Error.MappedTypeMustHaveDefaultConstructor(rowType.Type);
    }
    return sql.New(rowType, constructor, null, null, list, source);
  }

  private SqlLink BuildLink(SqlExpression item, MetaDataMember member, Expression source) {
    if (member.IsAssociation) {
      var array = new SqlExpression[member.Association.ThisKey.Count];
      var i = 0;
      for (var num = array.Length; i < num; i++) {
        var metaDataMember = member.Association.ThisKey[i];
        array[i] = sql.Member(item, metaDataMember.Member);
      }
      var otherType = member.Association.OtherType;
      return new SqlLink(new object(), otherType, member.Type, typeProvider.From(member.Type), item, member, array, null, source);
    }
    var declaringType = member.DeclaringType;
    var list = new List<SqlExpression>();
    foreach (var identityMember in declaringType.IdentityMembers) {
      list.Add(sql.Member(item, identityMember.Member));
    }
    SqlExpression expansion = sql.Member(item, member.Member);
    return new SqlLink(new object(), declaringType, member.Type, typeProvider.From(member.Type), item, member, list, expansion, source);
  }

  internal SqlNode TranslateLink(SqlLink link, bool asExpression) => TranslateLink(link, link.KeyExpressions, asExpression);

  internal static Expression TranslateAssociation(DataContext context, MetaAssociation association, Expression otherSource, Expression[] keyValues, Expression thisInstance) {
    if (association == null) {
      throw Error.ArgumentNull("association");
    }
    if (keyValues == null) {
      throw Error.ArgumentNull("keyValues");
    }
    if (context.LoadOptions != null) {
      var associationSubquery = context.LoadOptions.GetAssociationSubquery(association.ThisMember.Member);
      if (associationSubquery != null) {
        var relationComposer = new RelationComposer(associationSubquery.Parameters[0], association, otherSource, thisInstance);
        return relationComposer.Visit(associationSubquery.Body);
      }
    }
    return WhereClauseFromSourceAndKeys(otherSource, association.OtherKey.ToArray(), keyValues);
  }

  internal static Expression WhereClauseFromSourceAndKeys(Expression source, MetaDataMember[] keyMembers, Expression[] keyValues) {
    var elementType = TypeSystem.GetElementType(source.Type);
    var parameterExpression = Expression.Parameter(elementType, "p");
    Expression expression = null;
    for (var i = 0; i < keyMembers.Length; i++) {
      var metaDataMember = keyMembers[i];
      var expression2 = (elementType == metaDataMember.Member.DeclaringType) ? parameterExpression : ((Expression)Expression.Convert(parameterExpression, metaDataMember.Member.DeclaringType));
      Expression expression3 = (metaDataMember.Member is FieldInfo) ? Expression.Field(expression2, (FieldInfo)metaDataMember.Member) : Expression.Property(expression2, (PropertyInfo)metaDataMember.Member);
      var expression4 = keyValues[i];
      if (expression4.Type != expression3.Type) {
        expression4 = Expression.Convert(expression4, expression3.Type);
      }
      Expression expression5 = Expression.Equal(expression3, expression4);
      expression = ((expression != null) ? Expression.And(expression, expression5) : expression5);
    }
    return Expression.Call(typeof(Enumerable), "Where", new Type[1]
    {
      parameterExpression.Type
    }, source, Expression.Lambda(expression, parameterExpression));
  }

  internal SqlNode TranslateLink(SqlLink link, List<SqlExpression> keyExpressions, bool asExpression) {
    var member = link.Member;
    if (member.IsAssociation) {
      var otherType = member.Association.OtherType;
      var type = otherType.InheritanceRoot.Type;
      var table = services.Context.GetTable(type);
      Expression otherSource = new LinkedTableExpression(link, table, typeof(IQueryable<>).MakeGenericType(otherType.Type));
      var array = new Expression[keyExpressions.Count];
      for (var i = 0; i < keyExpressions.Count; i++) {
        var metaDataMember = member.Association.OtherKey[i];
        var memberType = TypeSystem.GetMemberType(metaDataMember.Member);
        array[i] = InternalExpression.Known(keyExpressions[i], memberType);
      }
      var thisInstance = (link.Expression != null) ? ((Expression)InternalExpression.Known(link.Expression)) : Expression.Constant(null, link.Member.Member.DeclaringType);
      var node = TranslateAssociation(services.Context, member.Association, otherSource, array, thisInstance);
      var queryConverter = new QueryConverter(services, typeProvider, this, sql);
      var sqlSelect = (SqlSelect)queryConverter.ConvertInner(node, link.SourceExpression);
      SqlNode result = sqlSelect;
      if (asExpression) {
        result = ((!member.Association.IsMany) ? new SqlSubSelect(SqlNodeType.Element, link.ClrType, link.SqlType, sqlSelect) : new SqlSubSelect(SqlNodeType.Multiset, link.ClrType, link.SqlType, sqlSelect));
      }
      return result;
    }
    return link.Expansion;
  }

  internal SqlExpression TranslateEquals(SqlBinary expr) {
    var sqlExpression = expr.Left;
    var sqlExpression2 = expr.Right;
    if (sqlExpression2.NodeType == SqlNodeType.Element) {
      var sqlSubSelect = (SqlSubSelect)sqlExpression2;
      var sqlAlias = new SqlAlias(sqlSubSelect.Select);
      var sqlAliasRef = new SqlAliasRef(sqlAlias);
      var sqlSelect = new SqlSelect(sqlAliasRef, sqlAlias, expr.SourceExpression) {
        Where = sql.Binary(expr.NodeType, sql.DoNotVisitExpression(sqlExpression), sqlAliasRef)
      };
      return sql.SubSelect(SqlNodeType.Exists, sqlSelect);
    }
    if (sqlExpression.NodeType == SqlNodeType.Element) {
      var sqlSubSelect2 = (SqlSubSelect)sqlExpression;
      var sqlAlias2 = new SqlAlias(sqlSubSelect2.Select);
      var sqlAliasRef2 = new SqlAliasRef(sqlAlias2);
      var sqlSelect2 = new SqlSelect(sqlAliasRef2, sqlAlias2, expr.SourceExpression) {
        Where = sql.Binary(expr.NodeType, sql.DoNotVisitExpression(sqlExpression2), sqlAliasRef2)
      };
      return sql.SubSelect(SqlNodeType.Exists, sqlSelect2);
    }
    MetaType sourceMetaType = TypeSource.GetSourceMetaType(sqlExpression, services.Model);
    MetaType sourceMetaType2 = TypeSource.GetSourceMetaType(sqlExpression2, services.Model);
    if (sqlExpression.NodeType == SqlNodeType.TypeCase) {
      sqlExpression = BestIdentityNode((SqlTypeCase)sqlExpression);
    }
    if (sqlExpression2.NodeType == SqlNodeType.TypeCase) {
      sqlExpression2 = BestIdentityNode((SqlTypeCase)sqlExpression2);
    }
    if (sourceMetaType.IsEntity && sourceMetaType2.IsEntity && sourceMetaType.Table != sourceMetaType2.Table) {
      throw Error.CannotCompareItemsAssociatedWithDifferentTable();
    }
    if (!sourceMetaType.IsEntity && !sourceMetaType2.IsEntity && (sqlExpression.NodeType != SqlNodeType.New || sqlExpression.SqlType.CanBeColumn) && (sqlExpression2.NodeType != SqlNodeType.New || sqlExpression2.SqlType.CanBeColumn)) {
      if (expr.NodeType == SqlNodeType.EQ2V || expr.NodeType == SqlNodeType.NE2V) {
        return TranslateEqualsOp(expr.NodeType, sql.DoNotVisitExpression(expr.Left), sql.DoNotVisitExpression(expr.Right), false);
      }
      return expr;
    }
    if (sourceMetaType != sourceMetaType2 && sourceMetaType.InheritanceRoot != sourceMetaType2.InheritanceRoot) {
      return sql.Binary(SqlNodeType.EQ, sql.ValueFromObject(0, expr.SourceExpression), sql.ValueFromObject(1, expr.SourceExpression));
    }
    var sqlLink = sqlExpression as SqlLink;
    var list = (sqlLink == null || !sqlLink.Member.IsAssociation || !sqlLink.Member.Association.IsForeignKey) ? GetIdentityExpressions(sourceMetaType, sql.DoNotVisitExpression(sqlExpression)) : sqlLink.KeyExpressions;
    var sqlLink2 = sqlExpression2 as SqlLink;
    var list2 = (sqlLink2 == null || !sqlLink2.Member.IsAssociation || !sqlLink2.Member.Association.IsForeignKey) ? GetIdentityExpressions(sourceMetaType2, sql.DoNotVisitExpression(sqlExpression2)) : sqlLink2.KeyExpressions;
    SqlExpression sqlExpression3 = null;
    var op = (expr.NodeType == SqlNodeType.EQ2V || expr.NodeType == SqlNodeType.NE2V) ? SqlNodeType.EQ2V : SqlNodeType.EQ;
    var i = 0;
    for (var count = list.Count; i < count; i++) {
      var sqlExpression4 = TranslateEqualsOp(op, list[i], list2[i], !sourceMetaType.IsEntity);
      sqlExpression3 = ((sqlExpression3 != null) ? sql.Binary(SqlNodeType.And, sqlExpression3, sqlExpression4) : sqlExpression4);
    }
    if (expr.NodeType == SqlNodeType.NE || expr.NodeType == SqlNodeType.NE2V) {
      sqlExpression3 = sql.Unary(SqlNodeType.Not, sqlExpression3, sqlExpression3.SourceExpression);
    }
    return sqlExpression3;
  }

  private SqlExpression TranslateEqualsOp(SqlNodeType op, SqlExpression left, SqlExpression right, bool allowExpand) {
    switch (op) {
      case SqlNodeType.EQ:
      case SqlNodeType.NE:
        return sql.Binary(op, left, right);
      case SqlNodeType.EQ2V: {
          if (SqlExpressionNullability.CanBeNull(left) != false && SqlExpressionNullability.CanBeNull(right) != false) {
            var nodeType3 = allowExpand ? SqlNodeType.EQ2V : SqlNodeType.EQ;
            return sql.Binary(SqlNodeType.Or, sql.Binary(SqlNodeType.And, sql.Unary(SqlNodeType.IsNull, (SqlExpression)SqlDuplicator.Copy(left)), sql.Unary(SqlNodeType.IsNull, (SqlExpression)SqlDuplicator.Copy(right))), sql.Binary(SqlNodeType.And, sql.Binary(SqlNodeType.And, sql.Unary(SqlNodeType.IsNotNull, (SqlExpression)SqlDuplicator.Copy(left)), sql.Unary(SqlNodeType.IsNotNull, (SqlExpression)SqlDuplicator.Copy(right))), sql.Binary(nodeType3, left, right)));
          }
          var nodeType4 = allowExpand ? SqlNodeType.EQ2V : SqlNodeType.EQ;
          return sql.Binary(nodeType4, left, right);
        }
      case SqlNodeType.NE2V: {
          if (SqlExpressionNullability.CanBeNull(left) != false && SqlExpressionNullability.CanBeNull(right) != false) {
            var nodeType = allowExpand ? SqlNodeType.EQ2V : SqlNodeType.EQ;
            return sql.Unary(SqlNodeType.Not, sql.Binary(SqlNodeType.Or, sql.Binary(SqlNodeType.And, sql.Unary(SqlNodeType.IsNull, (SqlExpression)SqlDuplicator.Copy(left)), sql.Unary(SqlNodeType.IsNull, (SqlExpression)SqlDuplicator.Copy(right))), sql.Binary(SqlNodeType.And, sql.Binary(SqlNodeType.And, sql.Unary(SqlNodeType.IsNotNull, (SqlExpression)SqlDuplicator.Copy(left)), sql.Unary(SqlNodeType.IsNotNull, (SqlExpression)SqlDuplicator.Copy(right))), sql.Binary(nodeType, left, right))));
          }
          var nodeType2 = allowExpand ? SqlNodeType.NE2V : SqlNodeType.NE;
          return sql.Binary(nodeType2, left, right);
        }
      default:
        throw Error.UnexpectedNode(op);
    }
  }

  internal SqlExpression TranslateLinkEquals(SqlBinary bo) {
    var sqlLink = bo.Left as SqlLink;
    var sqlLink2 = bo.Right as SqlLink;
    if ((sqlLink != null && sqlLink.Member.IsAssociation && sqlLink.Member.Association.IsForeignKey) || (sqlLink2 != null && sqlLink2.Member.IsAssociation && sqlLink2.Member.Association.IsForeignKey)) {
      return TranslateEquals(bo);
    }
    return bo;
  }

  internal SqlExpression TranslateLinkIsNull(SqlUnary expr) {
    var sqlLink = expr.Operand as SqlLink;
    if (sqlLink == null || !sqlLink.Member.IsAssociation || !sqlLink.Member.Association.IsForeignKey) {
      return expr;
    }
    var keyExpressions = sqlLink.KeyExpressions;
    SqlExpression sqlExpression = null;
    var nodeType = (expr.NodeType == SqlNodeType.IsNull) ? SqlNodeType.Or : SqlNodeType.And;
    var i = 0;
    for (var count = keyExpressions.Count; i < count; i++) {
      SqlExpression sqlExpression2 = sql.Unary(expr.NodeType, sql.DoNotVisitExpression(keyExpressions[i]), expr.SourceExpression);
      sqlExpression = ((sqlExpression != null) ? sql.Binary(nodeType, sqlExpression, sqlExpression2) : sqlExpression2);
    }
    return sqlExpression;
  }

  private static SqlExpression BestIdentityNode(SqlTypeCase tc) {
    foreach (var when in tc.Whens) {
      if (when.TypeBinding.NodeType == SqlNodeType.New) {
        return when.TypeBinding;
      }
    }
    return tc.Whens[0].TypeBinding;
  }

  private static bool IsPublic(MemberInfo mi) {
    var fieldInfo = mi as FieldInfo;
    if (fieldInfo != null) {
      return fieldInfo.IsPublic;
    }
    var propertyInfo = mi as PropertyInfo;
    if (propertyInfo != null && propertyInfo.CanRead) {
      var getMethod = propertyInfo.GetGetMethod();
      if (getMethod != null) {
        return getMethod.IsPublic;
      }
    }
    return false;
  }

  private IEnumerable<MetaDataMember> GetIdentityMembers(MetaType type) {
    if (type.IsEntity) {
      return type.IdentityMembers;
    }
    return from m in type.DataMembers
           where IsPublic(m.Member)
           select m;
  }

  private List<SqlExpression> GetIdentityExpressions(MetaType type, SqlExpression expr) {
    var list = GetIdentityMembers(type).ToList();
    var list2 = new List<SqlExpression>(list.Count);
    foreach (var item in list) {
      list2.Add(sql.Member((SqlExpression)SqlDuplicator.Copy(expr), item));
    }
    return list2;
  }
}

internal class SqlDuplicator {
  internal class DuplicatingVisitor : SqlVisitor {
    private Dictionary<SqlNode, SqlNode> nodeMap;

    private bool ingoreExternalRefs;

    internal DuplicatingVisitor(bool ignoreExternalRefs) {
      ingoreExternalRefs = ignoreExternalRefs;
      nodeMap = new Dictionary<SqlNode, SqlNode>();
    }

    internal override SqlNode Visit(SqlNode node) {
      if (node == null) {
        return null;
      }
      SqlNode value = null;
      if (nodeMap.TryGetValue(node, out value)) {
        return value;
      }
      value = base.Visit(node);
      nodeMap[node] = value;
      return value;
    }

    internal override SqlExpression VisitDoNotVisit(SqlDoNotVisitExpression expr) => new SqlDoNotVisitExpression(VisitExpression(expr.Expression));

    internal override SqlAlias VisitAlias(SqlAlias a) {
      var sqlAlias = new SqlAlias(a.Node);
      nodeMap[a] = sqlAlias;
      sqlAlias.Node = Visit(a.Node);
      sqlAlias.Name = a.Name;
      return sqlAlias;
    }

    internal override SqlExpression VisitAliasRef(SqlAliasRef aref) {
      if (ingoreExternalRefs && !nodeMap.ContainsKey(aref.Alias)) {
        return aref;
      }
      return new SqlAliasRef((SqlAlias)Visit(aref.Alias));
    }

    internal override SqlRowNumber VisitRowNumber(SqlRowNumber rowNumber) {
      var list = new List<SqlOrderExpression>();
      foreach (var item in rowNumber.OrderBy) {
        list.Add(new SqlOrderExpression(item.OrderType, (SqlExpression)Visit(item.Expression)));
      }
      return new SqlRowNumber(rowNumber.ClrType, rowNumber.SqlType, list, rowNumber.SourceExpression);
    }

    internal override SqlExpression VisitBinaryOperator(SqlBinary bo) {
      var left = (SqlExpression)Visit(bo.Left);
      var right = (SqlExpression)Visit(bo.Right);
      return new SqlBinary(bo.NodeType, bo.ClrType, bo.SqlType, left, right, bo.Method);
    }

    internal override SqlExpression VisitClientQuery(SqlClientQuery cq) {
      var subquery = (SqlSubSelect)VisitExpression(cq.Query);
      var sqlClientQuery = new SqlClientQuery(subquery);
      var i = 0;
      for (var count = cq.Arguments.Count; i < count; i++) {
        sqlClientQuery.Arguments.Add(VisitExpression(cq.Arguments[i]));
      }
      var j = 0;
      for (var count2 = cq.Parameters.Count; j < count2; j++) {
        sqlClientQuery.Parameters.Add((SqlParameter)VisitExpression(cq.Parameters[j]));
      }
      return sqlClientQuery;
    }

    internal override SqlExpression VisitJoinedCollection(SqlJoinedCollection jc) => new SqlJoinedCollection(jc.ClrType, jc.SqlType, VisitExpression(jc.Expression), VisitExpression(jc.Count), jc.SourceExpression);

    internal override SqlExpression VisitClientArray(SqlClientArray scar) {
      var array = new SqlExpression[scar.Expressions.Count];
      var i = 0;
      for (var num = array.Length; i < num; i++) {
        array[i] = VisitExpression(scar.Expressions[i]);
      }
      return new SqlClientArray(scar.ClrType, scar.SqlType, array, scar.SourceExpression);
    }

    internal override SqlExpression VisitTypeCase(SqlTypeCase tc) {
      SqlExpression discriminator = VisitExpression(tc.Discriminator);
      var list = new List<SqlTypeCaseWhen>();
      foreach (var when in tc.Whens) {
        list.Add(new SqlTypeCaseWhen(VisitExpression(when.Match), VisitExpression(when.TypeBinding)));
      }
      return new SqlTypeCase(tc.ClrType, tc.SqlType, tc.RowType, discriminator, list, tc.SourceExpression);
    }

    internal override SqlExpression VisitNew(SqlNew sox) {
      var array = new SqlExpression[sox.Args.Count];
      var array2 = new SqlMemberAssign[sox.Members.Count];
      var i = 0;
      for (var num = array.Length; i < num; i++) {
        array[i] = VisitExpression(sox.Args[i]);
      }
      var j = 0;
      for (var num2 = array2.Length; j < num2; j++) {
        array2[j] = VisitMemberAssign(sox.Members[j]);
      }
      return new SqlNew(sox.MetaType, sox.SqlType, sox.Constructor, array, sox.ArgMembers, array2, sox.SourceExpression);
    }

    internal override SqlNode VisitLink(SqlLink link) {
      var array = new SqlExpression[link.KeyExpressions.Count];
      var i = 0;
      for (var num = array.Length; i < num; i++) {
        array[i] = VisitExpression(link.KeyExpressions[i]);
      }
      var sqlLink = new SqlLink(new object(), link.RowType, link.ClrType, link.SqlType, null, link.Member, array, null, link.SourceExpression);
      nodeMap[link] = sqlLink;
      sqlLink.Expression = VisitExpression(link.Expression);
      sqlLink.Expansion = VisitExpression(link.Expansion);
      return sqlLink;
    }

    internal override SqlExpression VisitColumn(SqlColumn col) {
      var sqlColumn = new SqlColumn(col.ClrType, col.SqlType, col.Name, col.MetaMember, null, col.SourceExpression);
      nodeMap[col] = sqlColumn;
      sqlColumn.Expression = VisitExpression(col.Expression);
      sqlColumn.Alias = (SqlAlias)Visit(col.Alias);
      return sqlColumn;
    }

    internal override SqlExpression VisitColumnRef(SqlColumnRef cref) {
      if (ingoreExternalRefs && !nodeMap.ContainsKey(cref.Column)) {
        return cref;
      }
      return new SqlColumnRef((SqlColumn)Visit(cref.Column));
    }

    internal override SqlStatement VisitDelete(SqlDelete sd) => new SqlDelete((SqlSelect)Visit(sd.Select), sd.SourceExpression);

    internal override SqlExpression VisitElement(SqlSubSelect elem) => VisitMultiset(elem);

    internal override SqlExpression VisitExists(SqlSubSelect sqlExpr) => new SqlSubSelect(sqlExpr.NodeType, sqlExpr.ClrType, sqlExpr.SqlType, (SqlSelect)Visit(sqlExpr.Select));

    internal override SqlStatement VisitInsert(SqlInsert si) {
      var sqlInsert = new SqlInsert(si.Table, VisitExpression(si.Expression), si.SourceExpression) {
        OutputKey = si.OutputKey,
        OutputToLocal = si.OutputToLocal,
        Row = VisitRow(si.Row)
      };
      return sqlInsert;
    }

    internal override SqlSource VisitJoin(SqlJoin join) {
      SqlSource left = VisitSource(join.Left);
      SqlSource right = VisitSource(join.Right);
      var cond = (SqlExpression)Visit(join.Condition);
      return new SqlJoin(join.JoinType, left, right, cond, join.SourceExpression);
    }

    internal override SqlExpression VisitValue(SqlValue value) => value;

    internal override SqlNode VisitMember(SqlMember m) => new SqlMember(m.ClrType, m.SqlType, (SqlExpression)Visit(m.Expression), m.Member);

    internal override SqlMemberAssign VisitMemberAssign(SqlMemberAssign ma) => new SqlMemberAssign(ma.Member, (SqlExpression)Visit(ma.Expression));

    internal override SqlExpression VisitMultiset(SqlSubSelect sms) => new SqlSubSelect(sms.NodeType, sms.ClrType, sms.SqlType, (SqlSelect)Visit(sms.Select));

    internal override SqlExpression VisitParameter(SqlParameter p) {
      var sqlParameter = new SqlParameter(p.ClrType, p.SqlType, p.Name, p.SourceExpression) {
        Direction = p.Direction
      };
      return sqlParameter;
    }

    internal override SqlRow VisitRow(SqlRow row) {
      var sqlRow = new SqlRow(row.SourceExpression);
      foreach (var column in row.Columns) {
        sqlRow.Columns.Add((SqlColumn)Visit(column));
      }
      return sqlRow;
    }

    internal override SqlExpression VisitScalarSubSelect(SqlSubSelect ss) => new SqlSubSelect(SqlNodeType.ScalarSubSelect, ss.ClrType, ss.SqlType, VisitSequence(ss.Select));

    internal override SqlSelect VisitSelect(SqlSelect select) {
      SqlSource from = VisitSource(select.From);
      List<SqlExpression> list = null;
      if (select.GroupBy.Count > 0) {
        list = new List<SqlExpression>(select.GroupBy.Count);
        foreach (var item2 in select.GroupBy) {
          list.Add((SqlExpression)Visit(item2));
        }
      }
      var having = (SqlExpression)Visit(select.Having);
      List<SqlOrderExpression> list2 = null;
      if (select.OrderBy.Count > 0) {
        list2 = new List<SqlOrderExpression>(select.OrderBy.Count);
        foreach (var item3 in select.OrderBy) {
          var item = new SqlOrderExpression(item3.OrderType, (SqlExpression)Visit(item3.Expression));
          list2.Add(item);
        }
      }
      var top = (SqlExpression)Visit(select.Top);
      var where = (SqlExpression)Visit(select.Where);
      var row = (SqlRow)Visit(select.Row);
      SqlExpression selection = VisitExpression(select.Selection);
      var sqlSelect = new SqlSelect(selection, from, select.SourceExpression);
      if (list != null) {
        sqlSelect.GroupBy.AddRange(list);
      }
      sqlSelect.Having = having;
      if (list2 != null) {
        sqlSelect.OrderBy.AddRange(list2);
      }
      sqlSelect.OrderingType = select.OrderingType;
      sqlSelect.Row = row;
      sqlSelect.Top = top;
      sqlSelect.IsDistinct = select.IsDistinct;
      sqlSelect.IsPercent = select.IsPercent;
      sqlSelect.Where = where;
      sqlSelect.DoNotOutput = select.DoNotOutput;
      return sqlSelect;
    }

    internal override SqlTable VisitTable(SqlTable tab) {
      var sqlTable = new SqlTable(tab.MetaTable, tab.RowType, tab.SqlRowType, tab.SourceExpression);
      nodeMap[tab] = sqlTable;
      foreach (var column in tab.Columns) {
        sqlTable.Columns.Add((SqlColumn)Visit(column));
      }
      return sqlTable;
    }

    internal override SqlUserQuery VisitUserQuery(SqlUserQuery suq) {
      var list = new List<SqlExpression>(suq.Arguments.Count);
      foreach (var argument in suq.Arguments) {
        list.Add(VisitExpression(argument));
      }
      SqlExpression projection = VisitExpression(suq.Projection);
      var sqlUserQuery = new SqlUserQuery(suq.QueryText, projection, list, suq.SourceExpression);
      nodeMap[suq] = sqlUserQuery;
      foreach (var column in suq.Columns) {
        var sqlUserColumn = new SqlUserColumn(column.ClrType, column.SqlType, column.Query, column.Name, column.IsRequired, column.SourceExpression);
        nodeMap[column] = sqlUserColumn;
        sqlUserQuery.Columns.Add(sqlUserColumn);
      }
      return sqlUserQuery;
    }

    internal override SqlStoredProcedureCall VisitStoredProcedureCall(SqlStoredProcedureCall spc) {
      var list = new List<SqlExpression>(spc.Arguments.Count);
      foreach (var argument in spc.Arguments) {
        list.Add(VisitExpression(argument));
      }
      SqlExpression projection = VisitExpression(spc.Projection);
      var sqlStoredProcedureCall = new SqlStoredProcedureCall(spc.Function, projection, list, spc.SourceExpression);
      nodeMap[spc] = sqlStoredProcedureCall;
      foreach (var column in spc.Columns) {
        sqlStoredProcedureCall.Columns.Add((SqlUserColumn)Visit(column));
      }
      return sqlStoredProcedureCall;
    }

    internal override SqlExpression VisitUserColumn(SqlUserColumn suc) {
      if (ingoreExternalRefs && !nodeMap.ContainsKey(suc)) {
        return suc;
      }
      return new SqlUserColumn(suc.ClrType, suc.SqlType, suc.Query, suc.Name, suc.IsRequired, suc.SourceExpression);
    }

    internal override SqlExpression VisitUserRow(SqlUserRow row) => new SqlUserRow(row.RowType, row.SqlType, (SqlUserQuery)Visit(row.Query), row.SourceExpression);

    internal override SqlExpression VisitTreat(SqlUnary t) => new SqlUnary(SqlNodeType.Treat, t.ClrType, t.SqlType, (SqlExpression)Visit(t.Operand), t.SourceExpression);

    internal override SqlExpression VisitUnaryOperator(SqlUnary uo) => new SqlUnary(uo.NodeType, uo.ClrType, uo.SqlType, (SqlExpression)Visit(uo.Operand), uo.Method, uo.SourceExpression);

    internal override SqlStatement VisitUpdate(SqlUpdate su) {
      var select = (SqlSelect)Visit(su.Select);
      var list = new List<SqlAssign>(su.Assignments.Count);
      foreach (var assignment in su.Assignments) {
        list.Add((SqlAssign)Visit(assignment));
      }
      return new SqlUpdate(select, list, su.SourceExpression);
    }

    internal override SqlStatement VisitAssign(SqlAssign sa) => new SqlAssign(VisitExpression(sa.LValue), VisitExpression(sa.RValue), sa.SourceExpression);

    internal override SqlExpression VisitSearchedCase(SqlSearchedCase c) {
      SqlExpression @else = VisitExpression(c.Else);
      var array = new SqlWhen[c.Whens.Count];
      var i = 0;
      for (var num = array.Length; i < num; i++) {
        var sqlWhen = c.Whens[i];
        array[i] = new SqlWhen(VisitExpression(sqlWhen.Match), VisitExpression(sqlWhen.Value));
      }
      return new SqlSearchedCase(c.ClrType, array, @else, c.SourceExpression);
    }

    internal override SqlExpression VisitClientCase(SqlClientCase c) {
      SqlExpression expr = VisitExpression(c.Expression);
      var array = new SqlClientWhen[c.Whens.Count];
      var i = 0;
      for (var num = array.Length; i < num; i++) {
        var sqlClientWhen = c.Whens[i];
        array[i] = new SqlClientWhen(VisitExpression(sqlClientWhen.Match), VisitExpression(sqlClientWhen.Value));
      }
      return new SqlClientCase(c.ClrType, expr, array, c.SourceExpression);
    }

    internal override SqlExpression VisitSimpleCase(SqlSimpleCase c) {
      SqlExpression expr = VisitExpression(c.Expression);
      var array = new SqlWhen[c.Whens.Count];
      var i = 0;
      for (var num = array.Length; i < num; i++) {
        var sqlWhen = c.Whens[i];
        array[i] = new SqlWhen(VisitExpression(sqlWhen.Match), VisitExpression(sqlWhen.Value));
      }
      return new SqlSimpleCase(c.ClrType, expr, array, c.SourceExpression);
    }

    internal override SqlNode VisitUnion(SqlUnion su) => new SqlUnion(Visit(su.Left), Visit(su.Right), su.All);

    internal override SqlExpression VisitExprSet(SqlExprSet xs) {
      var array = new SqlExpression[xs.Expressions.Count];
      var i = 0;
      for (var num = array.Length; i < num; i++) {
        array[i] = VisitExpression(xs.Expressions[i]);
      }
      return new SqlExprSet(xs.ClrType, array, xs.SourceExpression);
    }

    internal override SqlBlock VisitBlock(SqlBlock block) {
      var sqlBlock = new SqlBlock(block.SourceExpression);
      foreach (var statement in block.Statements) {
        sqlBlock.Statements.Add((SqlStatement)Visit(statement));
      }
      return sqlBlock;
    }

    internal override SqlExpression VisitVariable(SqlVariable v) => v;

    internal override SqlExpression VisitOptionalValue(SqlOptionalValue sov) {
      SqlExpression hasValue = VisitExpression(sov.HasValue);
      SqlExpression value = VisitExpression(sov.Value);
      return new SqlOptionalValue(hasValue, value);
    }

    internal override SqlExpression VisitBetween(SqlBetween between) => new SqlBetween(between.ClrType, between.SqlType, VisitExpression(between.Expression), VisitExpression(between.Start), VisitExpression(between.End), between.SourceExpression);

    internal override SqlExpression VisitIn(SqlIn sin) {
      var sqlIn = new SqlIn(sin.ClrType, sin.SqlType, VisitExpression(sin.Expression), sin.Values, sin.SourceExpression);
      var i = 0;
      for (var count = sqlIn.Values.Count; i < count; i++) {
        sqlIn.Values[i] = VisitExpression(sqlIn.Values[i]);
      }
      return sqlIn;
    }

    internal override SqlExpression VisitLike(SqlLike like) => new SqlLike(like.ClrType, like.SqlType, VisitExpression(like.Expression), VisitExpression(like.Pattern), VisitExpression(like.Escape), like.SourceExpression);

    internal override SqlExpression VisitFunctionCall(SqlFunctionCall fc) {
      var array = new SqlExpression[fc.Arguments.Count];
      var i = 0;
      for (var count = fc.Arguments.Count; i < count; i++) {
        array[i] = VisitExpression(fc.Arguments[i]);
      }
      return new SqlFunctionCall(fc.ClrType, fc.SqlType, fc.Name, array, fc.SourceExpression);
    }

    internal override SqlExpression VisitTableValuedFunctionCall(SqlTableValuedFunctionCall fc) {
      var array = new SqlExpression[fc.Arguments.Count];
      var i = 0;
      for (var count = fc.Arguments.Count; i < count; i++) {
        array[i] = VisitExpression(fc.Arguments[i]);
      }
      var sqlTableValuedFunctionCall = new SqlTableValuedFunctionCall(fc.RowType, fc.ClrType, fc.SqlType, fc.Name, array, fc.SourceExpression);
      nodeMap[fc] = sqlTableValuedFunctionCall;
      foreach (var column in fc.Columns) {
        sqlTableValuedFunctionCall.Columns.Add((SqlColumn)Visit(column));
      }
      return sqlTableValuedFunctionCall;
    }

    internal override SqlExpression VisitMethodCall(SqlMethodCall mc) {
      var array = new SqlExpression[mc.Arguments.Count];
      var i = 0;
      for (var count = mc.Arguments.Count; i < count; i++) {
        array[i] = VisitExpression(mc.Arguments[i]);
      }
      return new SqlMethodCall(mc.ClrType, mc.SqlType, mc.Method, VisitExpression(mc.Object), array, mc.SourceExpression);
    }

    internal override SqlExpression VisitSharedExpression(SqlSharedExpression sub) {
      var sqlSharedExpression = new SqlSharedExpression(sub.Expression);
      nodeMap[sub] = sqlSharedExpression;
      sqlSharedExpression.Expression = VisitExpression(sub.Expression);
      return sqlSharedExpression;
    }

    internal override SqlExpression VisitSharedExpressionRef(SqlSharedExpressionRef sref) {
      if (ingoreExternalRefs && !nodeMap.ContainsKey(sref.SharedExpression)) {
        return sref;
      }
      return new SqlSharedExpressionRef((SqlSharedExpression)Visit(sref.SharedExpression));
    }

    internal override SqlExpression VisitSimpleExpression(SqlSimpleExpression simple) => new SqlSimpleExpression(VisitExpression(simple.Expression));

    internal override SqlExpression VisitGrouping(SqlGrouping g) => new SqlGrouping(g.ClrType, g.SqlType, VisitExpression(g.Key), VisitExpression(g.Group), g.SourceExpression);

    internal override SqlExpression VisitDiscriminatedType(SqlDiscriminatedType dt) => new SqlDiscriminatedType(dt.SqlType, VisitExpression(dt.Discriminator), dt.TargetType, dt.SourceExpression);

    internal override SqlExpression VisitLift(SqlLift lift) => new SqlLift(lift.ClrType, VisitExpression(lift.Expression), lift.SourceExpression);

    internal override SqlExpression VisitDiscriminatorOf(SqlDiscriminatorOf dof) => new SqlDiscriminatorOf(VisitExpression(dof.Object), dof.ClrType, dof.SqlType, dof.SourceExpression);

    internal override SqlNode VisitIncludeScope(SqlIncludeScope scope) => new SqlIncludeScope(Visit(scope.Child), scope.SourceExpression);
  }

  private DuplicatingVisitor superDuper;

  internal SqlDuplicator()
    : this(true) {
  }

  internal SqlDuplicator(bool ignoreExternalRefs) {
    superDuper = new DuplicatingVisitor(ignoreExternalRefs);
  }

  internal static SqlNode Copy(SqlNode node) {
    if (node == null) {
      return null;
    }
    var nodeType = node.NodeType;
    if (nodeType == SqlNodeType.ColumnRef || nodeType == SqlNodeType.Parameter || (uint)(nodeType - 92) <= 1u) {
      return node;
    }
    return new SqlDuplicator().Duplicate(node);
  }

  internal SqlNode Duplicate(SqlNode node) => superDuper.Visit(node);
}
