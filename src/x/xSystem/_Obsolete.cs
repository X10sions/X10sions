using System.Data;
using System.Text.RegularExpressions;

namespace System.Reflection {
  [Obsolete("To be deleted")]
  public static class _ObsoleteExtensions {


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

