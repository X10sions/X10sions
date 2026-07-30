using Common.Html.Css.Enums;

namespace Common.Html.Css {
  public class Font {
    // https://www.w3schools.com/cssref/pr_font_font.asp

    public string? Family{ get; set; } //= "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif";
    public decimal? LineHeight{ get; set; }// = 1.2m;
    public int? Size{ get; set; } //= 12;
    public FontStyle? Style { get; set; } 
    public FontWeight? Weight { get; set; } 
    public FontVariant? Variant { get; set; }

    public override string ToString()
      => ((Style == null ? string.Empty : " " + Style)
      + (Variant == null ? string.Empty : " " + Variant)
      + (Weight== null ? string.Empty : " " + Weight)
      + (Size == null ? string.Empty : " " + Size)
      + (LineHeight == null ? string.Empty : "/" + LineHeight)
      + (Family == null ? string.Empty : " " + Family)
      ).ToLower();

  }
}