using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace xSystem.NetStandard;

/// <summary> https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.ViewFeatures/src/ExpressionHelper.cs </summary>
internal static class ExpressionHelper {
  public static string GetUncachedExpressionText(this LambdaExpression expression) => GetExpressionText(expression, expressionTextCache: null);

  public static string GetExpressionText(this LambdaExpression expression, ConcurrentDictionary<LambdaExpression, string>? expressionTextCache) {
    if (expression is null) throw new ArgumentNullException(nameof(expression));
    if (expressionTextCache != null && expressionTextCache.TryGetValue(expression, out var expressionText)) {
      return expressionText;
    }
    // Determine size of string needed (length) and number of segments it contains (segmentCount). Put another
    // way, segmentCount tracks the number of times the loop below should iterate. This avoids adding ".model"
    // and / or an extra leading "." and then removing them after the loop. Other information collected in this
    // first loop helps with length and segmentCount adjustments. doNotCache is somewhat separate: If
    // true, expression strings are not cached for the expression.
    //
    // After the corrections below the first loop, length is usually exactly the size of the returned string.
    // However when containsIndexers is true, the calculation is approximate because either evaluating indexer
    // expressions multiple times or saving indexer strings can get expensive. Optimizing for the common case
    // of a collection (not a dictionary) with less than 100 elements. If that assumption proves to be
    // incorrect, the StringBuilder will be enlarged but hopefully just once.
    var doNotCache = false;
    var lastIsModel = false;
    var length = 0;
    var segmentCount = 0;
    var trailingMemberExpressions = 0;

    var part = expression.Body;
    while (part != null) {
      switch (part.NodeType) {
        case ExpressionType.Call:
          // Will exit loop if at Method().Property or [i,j].Property. In that case (like [i].Property),
          // don't cache and don't remove ".Model" (if that's .Property).
          doNotCache = true;
          lastIsModel = false;
          var methodExpression = (MethodCallExpression)part;
          if (IsSingleArgumentIndexer(methodExpression)) {
            length += "[99]".Length;
            part = methodExpression.Object;
            segmentCount++;
            trailingMemberExpressions = 0;
          } else {
            // Unsupported.
            part = null;
          }
          break;
        case ExpressionType.ArrayIndex:
          var binaryExpression = (BinaryExpression)part;

          doNotCache = true;
          lastIsModel = false;
          length += "[99]".Length;
          part = binaryExpression.Left;
          segmentCount++;
          trailingMemberExpressions = 0;
          break;
        case ExpressionType.MemberAccess:
          var memberExpressionPart = (MemberExpression)part;
          var name = memberExpressionPart.Member.Name;
          // If identifier contains "__", it is "reserved for use by the implementation" and likely
          // compiler- or Razor-generated e.g. the name of a field in a delegate's generated class.
          if (name.Contains("__")) {
            // Exit loop.
            part = null;
          } else {
            lastIsModel = string.Equals("model", name, StringComparison.OrdinalIgnoreCase);
            length += name.Length + 1;
            part = memberExpressionPart.Expression;
            segmentCount++;
            trailingMemberExpressions++;
          }
          break;
        case ExpressionType.Parameter:
          // Unsupported but indicates previous member access was not the view's Model.
          lastIsModel = false;
          part = null;
          break;

        default:
          // Unsupported.
          part = null;
          break;
      }
    }
    // If name would start with ".model", then strip that part away.
    if (lastIsModel) {
      length -= ".model".Length;
      segmentCount--;
      trailingMemberExpressions--;
    }
    // Trim the leading "." if present. The loop below special-cases the last property to avoid this addition.
    if (trailingMemberExpressions > 0) {
      length--;
    }
    Debug.Assert(segmentCount >= 0);
    if (segmentCount == 0) {
      Debug.Assert(!doNotCache);
      expressionTextCache?.TryAdd(expression, string.Empty);

      return string.Empty;
    }
    var builder = new StringBuilder(length);
    part = expression.Body;
    while (part != null && segmentCount > 0) {
      segmentCount--;
      switch (part.NodeType) {
        case ExpressionType.Call:
          Debug.Assert(doNotCache);
          var methodExpression = (MethodCallExpression)part;
          InsertIndexerInvocationText(builder, methodExpression.Arguments.Single(), expression);
          part = methodExpression.Object;
          break;
        case ExpressionType.ArrayIndex:
          Debug.Assert(doNotCache);
          var binaryExpression = (BinaryExpression)part;
          InsertIndexerInvocationText(builder, binaryExpression.Right, expression);
          part = binaryExpression.Left;
          break;
        case ExpressionType.MemberAccess:
          var memberExpression = (MemberExpression)part;
          var name = memberExpression.Member.Name;
          Debug.Assert(!name.Contains("__"));

          builder.Insert(0, name);
          if (segmentCount > 0) {
            // One or more parts to the left of this part are coming.
            builder.Insert(0, '.');
          }
          part = memberExpression.Expression;
          break;

        default:
          // Should be unreachable due to handling in above loop.
          Debug.Assert(false);
          break;
      }
    }
    Debug.Assert(segmentCount == 0);
    expressionText = builder.ToString();
    if (expressionTextCache != null && !doNotCache) {
      expressionTextCache.TryAdd(expression, expressionText);
    }
    return expressionText;
  }

  private static void InsertIndexerInvocationText(StringBuilder builder, Expression indexExpression, LambdaExpression parentExpression) {
    if (builder is null) throw new ArgumentNullException(nameof(builder));
    if (indexExpression is null) throw new ArgumentNullException(nameof(indexExpression));
    if (parentExpression is null) throw new ArgumentNullException(nameof(parentExpression));
    if (parentExpression.Parameters == null) {
      throw new ArgumentException(Resources.FormatPropertyOfTypeCannotBeNull(nameof(parentExpression.Parameters), nameof(parentExpression)));
    }
    var converted = Expression.Convert(indexExpression, typeof(object));
    var fakeParameter = Expression.Parameter(typeof(object), null);
    var lambda = Expression.Lambda<Func<object, object>>(converted, fakeParameter);
    Func<object, object> func;
    try {
      func = CachedExpressionCompiler.Process(lambda) ?? lambda.Compile();
    } catch (InvalidOperationException ex) {
      var parameters = parentExpression.Parameters.ToArray();
      throw new InvalidOperationException(Resources.FormatExpressionHelper_InvalidIndexerExpression(indexExpression, parameters[0].Name), ex);
    }
    builder.Insert(0, ']');
    builder.Insert(0, Convert.ToString(func(null), CultureInfo.InvariantCulture));
    builder.Insert(0, '[');
  }

  public static bool IsSingleArgumentIndexer(Expression expression) {
    if (!(expression is MethodCallExpression methodExpression) || methodExpression.Arguments.Count != 1) {
      return false;
    }
    // Check whether GetDefaultMembers() (if present in CoreCLR) would return a member of this type. Compiler
    // names the indexer property, if any, in a generated [DefaultMember] attribute for the containing type.
    var declaringType = methodExpression.Method.DeclaringType;
    var defaultMember = declaringType.GetCustomAttribute<DefaultMemberAttribute>(inherit: true);
    if (defaultMember == null) {
      return false;
    }
    // Find default property (the indexer) and confirm its getter is the method in this expression.
    var runtimeProperties = declaringType.GetRuntimeProperties();
    foreach (var property in runtimeProperties) {
      if ((string.Equals(defaultMember.MemberName, property.Name, StringComparison.Ordinal) &&
          property.GetMethod == methodExpression.Method)) {
        return true;
      }
    }
    return false;
  }

  #region https://code-maze.com/dynamic-queries-expression-trees-csharp/

  public static Expression<Func<T, bool>> CreateEqualExpression<T>(string propertyName, object value, bool doNullCheck, bool doTypeCheck) {
    var type = typeof(T);
    var param = Expression.Parameter(type, type.Name);
    var member = Expression.Property(param, propertyName);
    if (doTypeCheck) {
      var typeCheck = Expression.TypeEqual(Expression.Constant(value), typeof(string));
      var typedConstant = Expression.Constant(value, typeof(object));
      var condition = Expression.Condition(
          typeCheck,
          Expression.Equal(member, typedConstant),
          Expression.Constant(false)
      );
      return Expression.Lambda<Func<T, bool>>(condition, param);
    }
    var constant = Expression.Constant(value);
    var body = Expression.Equal(member, constant);
    if (doNullCheck) {
      var nullCheck = Expression.Equal(member, Expression.Constant(null));
      var condition = Expression.Condition(nullCheck, Expression.Constant(false), body);
      return Expression.Lambda<Func<T, bool>>(condition, param);
    }
    return Expression.Lambda<Func<T, bool>>(body, param);
  }

  public static Expression<Func<T, bool>> CreateEqualExpression<T>(this IDictionary<string, object> filters) {
    var type = typeof(T);
    var param = Expression.Parameter(type, type.Name);
    Expression? body = null;
    foreach (var pair in filters) {
      var member = Expression.Property(param, pair.Key);
      var constant = Expression.Constant(pair.Value);
      var expression = Expression.Equal(member, constant);
      body = body == null ? expression : Expression.AndAlso(body, expression);
    }
    return Expression.Lambda<Func<T, bool>>(body, param);
  }

  public static Expression<Func<T, bool>> CreateContainsExpression<T>(string propertyName, string value) {
    var type = typeof(T);
    var param = Expression.Parameter(type, type.Name);
    var member = Expression.Property(param, propertyName);
    var constant = Expression.Constant(value);
    var body = Expression.Call(member, nameof(string.Contains), Type.EmptyTypes, constant);
    return Expression.Lambda<Func<T, bool>>(body, param);
  }

  public static Expression<Func<T, bool>> CreateInExpression<T>(string propertyName, object value) {
    var type = typeof(T);
    var param = Expression.Parameter(type, type.Name);
    var member = Expression.Property(param, propertyName);
    var propertyType = ((PropertyInfo)member.Member).PropertyType;
    var constant = Expression.Constant(value);
    var body = Expression.Call(typeof(Enumerable), nameof(string.Contains), [propertyType], constant, member);
    return Expression.Lambda<Func<T, bool>>(body, param);
  }

  public static Expression<Func<T, bool>> CreateNestedExpression<T>(string propertyName, object value) {
    var type = typeof(T);
    var param = Expression.Parameter(type, type.Name);
    Expression member = param;
    foreach (var namePart in propertyName.Split('.')) {
      member = Expression.Property(member, namePart);
    }
    var constant = Expression.Constant(value);
    var body = Expression.Equal(member, constant);
    return Expression.Lambda<Func<T, bool>>(body, param);
  }

  public static Expression<Func<T, bool>> CreateBetweenExpression<T>(string propertyName, object lowerValue, object upperValue) {
    var type = typeof(T);
    var param = Expression.Parameter(type, type.Name);
    var property = Expression.Property(param, propertyName);
    var body = Expression.AndAlso(Expression.GreaterThanOrEqual(property, Expression.Constant(lowerValue)), Expression.LessThanOrEqual(property, Expression.Constant(upperValue))
    );
    return Expression.Lambda<Func<T, bool>>(body, param);
  }

  /// <summary>
  /// var expression = CreateEqualExpression("FirstName", "Manoel");
  /// var predicate = ExpressionCacheHelper.GetPredicate(expression);
  /// var enumerable = persons.Where(predicate);
  /// </summary>
  public static class ExpressionCacheHelper {
    private static readonly Dictionary<int, Delegate> Cache = new Dictionary<int, Delegate>();
    public static Func<T, bool> GetPredicate<T>(Expression<Func<T, bool>> expression) {
      var key = expression.GetHashCode();
      if (Cache.TryGetValue(key, out var cachedDelegate)) {
        return (Func<T, bool>)cachedDelegate;
      }
      var compiledDelegate = expression.Compile();
      Cache[key] = compiledDelegate;
      return compiledDelegate;
    }
  }

  #endregion

}