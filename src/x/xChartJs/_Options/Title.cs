using ChartJs.Enums;
using Common.Html.Css;

namespace ChartJs.Options {
  public class Title {
    public bool? Display { get; set; }
    public Font? Font { get; set; } //= "defaults.font";
    public decimal? LineHeight { get; set; } //= 10;
    public int? Padding { get; set; } //= 10;
    public Position? Position { get; set; }
    public string Text { get; set; } = string.Empty;
  }
}