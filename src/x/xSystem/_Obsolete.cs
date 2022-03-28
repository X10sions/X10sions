using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;

namespace System.Reflection ;
  [Obsolete("To be deleted")]
  public static class _ObsoleteExtensions {

    [Obsolete("Try remove this")]
    public static bool Obsolete_EqualsTo(this MemberInfo member1, MemberInfo member2, Type declaringType = null) {
      if (ReferenceEquals(member1, member2))
        return true;
      if (member1 == null || member2 == null)
        return false;
      if (member1.Name == member2.Name) {
        if (member1.DeclaringType == member2.DeclaringType)
          return true;
        if (member1 is PropertyInfo) {
          var isSubclass = member1.DeclaringType.IsSameOrParentOf(member2.DeclaringType) || member2.DeclaringType.IsSameOrParentOf(member1.DeclaringType);
          if (isSubclass)
            return true;
          if (declaringType != null && member2.DeclaringType.IsInterface) {
            var getter1 = ((PropertyInfo)member1).GetGetMethod();
            var getter2 = ((PropertyInfo)member2).GetGetMethod();
            var map = declaringType.GetInterfaceMap(member2.DeclaringType);
            for (var i = 0; i < map.InterfaceMethods.Length; i++)
              if (getter2.Name == map.InterfaceMethods[i].Name && getter2.DeclaringType == map.InterfaceMethods[i].DeclaringType &&
                getter1.Name == map.TargetMethods[i].Name && getter1.DeclaringType == map.TargetMethods[i].DeclaringType)
                return true;
          }
        }
      }
      if (member2.DeclaringType.IsInterface && !member1.DeclaringType.IsInterface && member1.Name.EndsWith(member2.Name, StringComparison.Ordinal) && member1 is PropertyInfo) {
        var isSubclass = member2.DeclaringType.IsAssignableFrom(member1.DeclaringType);
        if (isSubclass) {
          var getter1 = ((PropertyInfo)member1).GetGetMethod();
          var getter2 = ((PropertyInfo)member2).GetGetMethod();
          var map = member1.DeclaringType.GetInterfaceMap(member2.DeclaringType);
          for (var i = 0; i < map.InterfaceMethods.Length; i++) {
            var imi = map.InterfaceMethods[i];
            var tmi = map.TargetMethods[i];
            if ((getter2 == null || (getter2.Name == imi.Name && getter2.DeclaringType == imi.DeclaringType)) &&
                (getter1 == null || (getter1.Name == tmi.Name && getter1.DeclaringType == tmi.DeclaringType))) {
              return true;
            }
          }
        }
      }
      return false;
    }

    [Obsolete("Try remove this")]
    public static T[] Obsolete_xGetAttributes<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute {
      var attrs = memberInfo.GetCustomAttributes(typeof(T), inherit);
      var arr = new T[attrs.Length];
      for (var i = 0; i < attrs.Length; i++) {
        arr[i] = (T)attrs[i];
      }
      return arr;
    }

    [Obsolete("Try remove this")]
    public static Type Obsolete_GetMemberType(this MemberInfo memberInfo) {
      switch (memberInfo.MemberType) {
        case MemberTypes.Property:
          return ((PropertyInfo)memberInfo).PropertyType;
        case MemberTypes.Field:
          return ((FieldInfo)memberInfo).FieldType;
        case MemberTypes.Method:
          return ((MethodInfo)memberInfo).ReturnType;
        case MemberTypes.Constructor:
          return memberInfo.DeclaringType;
      }
      throw new InvalidOperationException();
    }

    [Obsolete("Try remove this")] public static bool Obsolete_HasAttribute<TAttribute>(this MemberInfo memberInfo, bool inherit) where TAttribute : Attribute => memberInfo.Obsolete_HasAttribute(typeof(TAttribute), inherit);

    [Obsolete("Try remove this")] public static bool Obsolete_HasAttribute(this MemberInfo memberInfo, Type attributeType, bool inherit) => Attribute.IsDefined(memberInfo, attributeType, inherit);

    [Obsolete("Try remove these")] public static Type Obsolete_GetMemberType01(this MemberInfo memberInfo) => (memberInfo as PropertyInfo)?.PropertyType ?? ((FieldInfo)memberInfo)?.FieldType;

    [Obsolete("Use default(type) instead.")] public static object Obsolete_GetDefaultValue(this Type type) => Obsolete_DefaultValueDictionary.Instance.TryGetValue(type, out var result) ? result : null;

    [Obsolete("Use MapToObject")] public static void Obsolete_MapPropertiesTo(this DataRow row, object obj) => row.MapToObject(obj);

    [Obsolete]
    public static IEnumerable<DataRow> Obsolete_AsEnumerableX(this DataTable dataTable) {
      foreach (DataRow row in dataTable.Rows) {
        yield return row;
      }
    }

    [Obsolete("Does not work properly, needs more testing")]
    public static IDbCommand Obsolete_RewriteNamedParametersToPositionalParameters(this IDbCommand command) {
      var newCommand = command.Obsolete_GetRewriteNamedParametersToPositionalParameters();
      return command.ReplaceCommndTextAndParameters(newCommand.CommandText, newCommand.Parameters);
    }

    [Obsolete("Does not work properly, needs more testing")]
    public static (string CommandText, List<IDbDataParameter> Parameters) Obsolete_GetRewriteNamedParametersToPositionalParameters(this IDbCommand command) {
      var newCommandText = command.CommandText;
      var newParameters = new List<IDbDataParameter>();
      var parameterMatches = command.Parameters.Cast<IDbDataParameter>().Select(x => Regex.Matches(newCommandText, "@" + x.ParameterName)).ToList();
      // Check to see if any of the parameters are listed multiple times in the command text.
      if (parameterMatches.Any(x => x.Count > 1)) {
        // order by descending to make the parameter name replacing easy
        var matches = parameterMatches.SelectMany(x => x.Cast<Match>()).OrderByDescending(x => x.Index);
        foreach (var match in matches) {
          // Substring removed the @ prefix.
          var parameterName = match.Value.Substring(1);
          // Add index to the name to make the parameter name unique.
          var newParameterName = parameterName + "_" + match.Index;
          var newParameter = (IDbDataParameter)((ICloneable)command.Parameters[parameterName]).Clone();
          newParameter.ParameterName = newParameterName;
          newParameters.Add(newParameter);
          // Replace the old parameter name with the new parameter name.
          newCommandText = newCommandText.Substring(0, match.Index) + "@" + newParameterName + newCommandText.Substring(match.Index + match.Length);
        }
        // The parameters were added to the list in the reverse order to make parameter name replacing easy.
        newParameters.Reverse();
        //ReplaceParameterNamesWithQuestionMark
        for (var index = command.Parameters.Count - 1; index >= 0; index--) {
          var p = (IDbDataParameter)command.Parameters[index];
          newCommandText = newCommandText.Replace("@" + p.ParameterName, "?");
        }
      }
      return (newCommandText, newParameters);
    }

    [Obsolete("Does not work properly, needs more testing")]
    public static (string CommandText, List<IDbDataParameter> Parameters) Obsolete_GetConvertNamedParametersToPositionalParameters2(this IDbCommand command) {
      //1. Find all occurrences parameters references in the SQL statement (such as @MyParameter).
      //2. Find the corresponding parameter in the command's parameters list.
      //3. Add the found parameter to the newParameters list and replace the parameter reference in the SQL with a question mark (?).
      //4. Replace the command's parameters list with the newParameters list.
      var oldParameters = command.Parameters;
      var oldCommandText = command.CommandText;
      var newParameters = new List<IDbDataParameter>();
      var newCommandText = oldCommandText;
      var paramNames = oldCommandText.Replace("@@", "??").Split('@').Select(x => x.Split(new[] { ' ', ')', ';', '\r', '\n' }).FirstOrDefault().Trim()).ToList().Skip(1);
      foreach (var p in paramNames) {
        newCommandText = newCommandText.Replace("@" + p, "?");
        var parameter = oldParameters.OfType<IDbDataParameter>().FirstOrDefault(a => a.ParameterName == p);
        if (parameter != null) {
          parameter.ParameterName = $"{parameter.ParameterName}_{newParameters.Count}";
          newParameters.Add(parameter);
        }
      }
      return (newCommandText, newParameters);
    }

    [Obsolete("Does not work properly, needs more testing")]
    public static IDbCommand Obsolete_ConvertNamedParametersToPositionalParameters(this IDbCommand command, char parameterPrefix = '@') {
      var positional = command.Obsolete_GetPositionalCommandTextAndParameters(parameterPrefix);
      return command.ReplaceCommndTextAndParameters(positional.CommandText, positional.Parameters);
    }

    [Obsolete("Does not work for Select @@Identity")]
    public static class Obsolete_NamedParameterPrefixPattern {
      public static readonly Regex AtSign = new Regex("(@\\w*)", RegexOptions.IgnoreCase);
      public static readonly Regex DollarSign = new Regex("($\\w*)", RegexOptions.IgnoreCase);
      public static readonly Regex Colon = new Regex("(:\\w*)", RegexOptions.IgnoreCase);
    }

    [Obsolete("Does not work properly, needs more testing")]
    public static Regex Obsolete_GetNamedParameterPrefixPattern(char parameterPrefix) {
      switch (parameterPrefix) {
        case '@':
          return Obsolete_NamedParameterPrefixPattern.AtSign;
        case ':':
          return Obsolete_NamedParameterPrefixPattern.Colon;
        case '$':
          return Obsolete_NamedParameterPrefixPattern.DollarSign;
        default:
          return new Regex($@"({parameterPrefix}\w*)", RegexOptions.IgnoreCase);
      }
    }

    [Obsolete("Does not work properly, needs more testing")]
    public static (string CommandText, List<IDbDataParameter> Parameters) Obsolete_GetPositionalCommandTextAndParameters(this IDbCommand command, char parameterPrefix = '@') {
      var namedParameterPrefixPattern = Obsolete_GetNamedParameterPrefixPattern(parameterPrefix);
      var newParameters = new List<IDbDataParameter>();
      var newCommandText = namedParameterPrefixPattern.Replace(command.CommandText, evaluator => {
        var match = evaluator.Groups[1].Value;
        var parameter = (from p in command.Parameters.OfType<IDbDataParameter>()
                         where p.ParameterName == match.TrimStart(parameterPrefix)
                         select command.CloneParameter(p)).FirstOrDefault();
        if (parameter != null) {
          parameter.ParameterName = $"{parameter.ParameterName}_{newParameters.Count}";
          newParameters.Add(parameter);
          return "?";
        }
        return match;
      });
      return (newCommandText, newParameters);
    }

    [Obsolete("Use SqLiteral()")] public static string Obsolete_ToSqlExpression(this decimal? @this) => @this.SqlLiteral();

    [Obsolete("Use GetAttribute")] public static string Obsolete_TGetDescription(this Enum enumValue) => enumValue.GetAttribute<DescriptionAttribute>()?.Description ?? enumValue.ToString();
    //[Obsolete("Use GetAttribute")] public static string Obsolete_TGetDisplay(this Enum enumValue) => enumValue.GetAttribute<DisplayAttribute>()?.Name ?? enumValue.ToString();
    [Obsolete("Use GetAttribute")] public static string Obsolete_TGetDisplayName(this Enum enumValue) => enumValue.GetAttribute<DisplayNameAttribute>()?.DisplayName ?? enumValue.ToString();

    //public static IEnumerable<SelectListItem> Obsolete_TToSelectList<TEnum>(this TEnum enumObj) where TEnum : Enum => EnumToSelectList(new[] { enumObj });
    //public static IEnumerable<SelectListItem> Obsolete_TEnumToSelectList<T>(this T[] selectedValues) => Enum.GetValues(typeof(T)).Cast<T>().Select(x => new SelectListItem {
    //  Text = x.ToString() + ": " + x.GetAttribute<T, DescriptionAttribute>()?.Description,
    //  Value = x.ToString(),
    //  Selected = selectedValues.Contains(x)
    //});

    [Obsolete("Use AsExpr")] public static Expression<Func<T, TOut>> Obsolete_ConvertToExpr<T, TOut>(this Func<T, TOut> func) => func.AsExpr();

    [Obsolete("Use SqLiteral()")] public static string Obsolete_ToSqlExpression(this int? @this) => @this.HasValue ? @this.Value.ToString() : "Null";
    [Obsolete("Use SqLiteral()")] public static string Obsolete_ToSqlValue(this int? @this) => @this.HasValue ? @this.Value.ToString() : "Null";

    [Obsolete] public static string Obsolete_IdString(this long? value) => value.HasValue ? value.ToString() : string.Empty;

    [Obsolete("Use System.Net.Dns.GetHostEntry")] public static IPHostEntry Obsolete_LocalHostEntry() => Dns.GetHostEntry(string.Empty);

    [Obsolete]
    public static void Obsolete_SendToPickupDirectory(this MailMessage mailMessage, string pickupDirectoryLocation = null) {
      using (var client = new SmtpClient { DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory }) {
        if (!string.IsNullOrWhiteSpace(pickupDirectoryLocation)) {
          client.PickupDirectoryLocation = pickupDirectoryLocation;
        }
        client.Send(mailMessage);
      }
    }

    //[Obsolete("Use SplitOfType<T>")]
    //public static IList<T> Obsolete_SplitToListOf<T>(this string expr, string delimeter, AllowStringType allowStringType = AllowStringType.NotNullOrWhitespace) {
    //  var list = new List<T>();
    //  if (expr != null) {
    //    foreach (string s in expr.Split(delimeter)) {
    //      if (allowStringType == AllowStringType.Any ||
    //        allowStringType == AllowStringType.NotNullOrWhitespace && !string.IsNullOrWhiteSpace(s) ||
    //        allowStringType == AllowStringType.NotNullOrEmpty && !string.IsNullOrEmpty(s) ||
    //        allowStringType == AllowStringType.NotNull && s != null) {
    //        TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
    //        var value = typeConverter.ConvertFromString(s);
    //        //var value = Convert.ChangeType(s, typeof(T));
    //        var value1 = s.OfType<T>();
    //        if (allowStringType == AllowStringType.Any || value != null) {
    //          list.Add((T)value);
    //        }
    //      }
    //    }
    //  }
    //  return list;
    //}

    [Obsolete("Use s.SplitOfType<int>(separator).ToList()")] public static IList<int> Obsolete_SplitToListOfInt(this string s, string separator) => s.SplitOfType<int>(separator).ToList();
    [Obsolete("Use s.SplitNotNullOrWhiteSpace(separator).ToList()")] public static IList<string> Obsolete_SplitToListOfString(this string s, string separator) => s.SplitNotNullOrWhiteSpace(separator).ToList();
    [Obsolete("Use s.SplitOfType<int>(separator)")] public static IEnumerable<int> Obsolete_SplitAsInt(this string s, string separator) => s.SplitOfType<int>(separator);
    [Obsolete("Use s.SplitOfType<int>(separator)")] public static IEnumerable<int> Obsolete_ToIntegerList(this string s, string separator) => s.SplitOfType<int>(separator);

    //[Obsolete()]
    //public static IList<T> Obsolete_SplitOfType<T>(this string expr, string delimeter = ",", AllowStringType allowStringType = AllowStringType.NotNullOrWhitespace) {
    //  Func<string, bool> predicate = x => true;
    //  if (allowStringType == AllowStringType.NotNullOrWhitespace) {
    //    predicate = s => !string.IsNullOrWhiteSpace(s);
    //  } else if (allowStringType == AllowStringType.NotNullOrEmpty) {
    //    predicate = s => !string.IsNullOrEmpty(s);
    //  }
    //  var strings = expr.Split(delimeter).Where(predicate);
    //  IEnumerable<T> types;
    //  if (allowStringType == AllowStringType.Any) {
    //    types = from s in strings select (T)(TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(s));
    //  } else {
    //    types = strings.OfType<T>();
    //  }
    //  return types.ToList();
    //}

    [Obsolete("Use s.SplitNotNullOrWhiteSpace(separator)")] public static IEnumerable<string> Obsolete_ToStringList(this string s, string separator) => s.SplitNotNullOrWhiteSpace(separator);

    [Obsolete("Use value.SqlLiteral()")] public static string Obsolete_ToSqlExpression(this string value) => value.SqlLiteral();

    //[Obsolete("Use Path.Combine(IHostingEnvironment.WebRootPath, 'SomePath')")] public string Obsolete_ServerMapPath => IsPhysicalPath ? path : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.TrimStart('/', '\\', '~')).Replace('/', '\\');

  }

  public static class Obsolete_DateTimeConstants {
    [Obsolete] public const string Obsolete_SqlQualifierDateDefault = SqlDateOptions.DefaultLiteralPrefix;
    [Obsolete] public const string Obsolete_SqlQualifierTimeDefault = SqlTimeOptions.DefaultLiteralPrefix;
    [Obsolete] public const string Obsolete_SqlQualifierTimestampDefault = SqlTimestampOptions.DefaultLiteralPrefix;
    [Obsolete] public const string Obsolete_SqlQualifierDateMsAccess = "#";

  }

  [Obsolete("Use default(type) instead.")]
  public class Obsolete_DefaultValueDictionary : Dictionary<Type, object> {

    public static Obsolete_DefaultValueDictionary Instance => new Obsolete_DefaultValueDictionary();

    public Obsolete_DefaultValueDictionary() {
      this[typeof(decimal)] = default(decimal);
      this[typeof(int)] = default(int);
      this[typeof(Guid)] = default(Guid);
      this[typeof(DateTime)] = default(DateTime);
      this[typeof(DateTimeOffset)] = default(DateTimeOffset);
      this[typeof(long)] = default(long);
      this[typeof(bool)] = default(bool);
      this[typeof(double)] = default(double);
      this[typeof(short)] = default(short);
      this[typeof(float)] = default(float);
      this[typeof(byte)] = default(byte);
      this[typeof(char)] = default(char);
      this[typeof(uint)] = default(uint);
      this[typeof(ushort)] = default(ushort);
      this[typeof(ulong)] = default(ulong);
      this[typeof(sbyte)] = default(sbyte);
    }

  }

}

namespace System.Data.Linq.Mapping {

  [Obsolete]
  public enum Obsolete_AutoSync {
    Default,
    Always,
    Never,
    OnInsert,
    OnUpdate
  }

  [Obsolete]
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public sealed class Obsolete_ColumnAttribute : Obsolete_DataAttribute {
    public Obsolete_AutoSync AutoSync { get; set; }
    public bool CanBeNull { get; set; } = true;
    public string DbType { get; set; }
    public string Expression { get; set; }
    public bool IsDbGenerated { get; set; }
    public bool IsDiscriminator { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsVersion { get; set; }
    public Obsolete_UpdateCheck UpdateCheck { get; set; }
  }

  [Obsolete]
  public abstract class Obsolete_DataAttribute : Attribute {
    public string Name { get; set; }
    public string Storage { get; set; }
  }

  [Obsolete]
  public enum Obsolete_UpdateCheck {
    Always,
    Never,
    WhenChanged
  }
}
namespace System.Reflection {

  [Obsolete("Try get rid of these: used by CommonORM")]
  public static class Obsolete_MemberInfo_MyExtensions {
    //makes expression for specific prop
    public static Expression<Func<TSource, object>> MyGetExpression<TSource>(string propertyName) {
      var pe = Expression.Parameter(typeof(TSource), "x");
      return Expression.Lambda<Func<TSource, object>>(Expression.Convert(Expression.PropertyOrField(pe, propertyName), typeof(object)), pe);
    }

    public static Expression<Func<TSource, object>> MyGetExpression<TSource>(this MemberInfo prop) => MyGetExpression<TSource>(prop.Name);

    public static Func<TSource, object> MyGetFunc<TSource>(string propertyName) => MyGetExpression<TSource>(propertyName).Compile();  //only need compiled expression

    public static Func<TSource, object> MyGetFunc<TSource>(this MemberInfo prop) => MyGetFunc<TSource>(prop.Name);

    public static IOrderedEnumerable<TSource> MyOrderBy<TSource>(this IEnumerable<TSource> source, string propertyName) => source.OrderBy(MyGetFunc<TSource>(propertyName));

    //OrderBy overload
    public static IOrderedQueryable<TSource> MyOrderBy<TSource>(this IQueryable<TSource> source, string propertyName) => source.OrderBy(MyGetExpression<TSource>(propertyName));

  }
}

namespace System.Windows.Media.Imaging {

  [Obsolete("Use: System.Drawing.Bitmap or xSystem.Windows.Media.Imaging.BitmapSource", false)]
  public class Obsolete_BitmapSource {

    public Obsolete_BitmapSource() {
      throw new NotImplementedException();
    }

  }
}
