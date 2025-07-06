using Common.Constants;

namespace Common.Abstractions {
  [Obsolete("To be deleted")]
  public static class _ObsoleteExtensions {


    [Obsolete("VB replace with: ")] 
    public static readonly TypeCode[] Obsolete_TypeCodeConstants_NumericVB = TypeCodeConstants.Numeric.Union(new[] { TypeCode.Boolean }).ToArray();

    [Obsolete("Use application/javascript")] 
    public const string Obsolete_Constants_MediaTypeNames_Text_Javascript = "text/javascript";



  }
}