using ChartJs.Enums;
using Common.Html.Css;
using Common.Html.Css.Enums;

namespace ChartJs.Options {
  public class Legend {
    public TextAlignLast? Align { get; set; }
    public bool? Display { get; set; }
    public Position? Position { get; set; }
    public bool? FullWidth { get; set; }
    public string? OnClick { get; set; }
    public string? OnHover { get; set; }
    public string? OnLeave { get; set; }
    public bool? Reverse { get; set; }
    public LegendLabel[]? Labels { get; set; }
    public bool? Rtl { get; set; }
    public string? TextDirection { get; set; }
  }

  public class LegendLabel {
    public int? BoxWidth { get; set; }
    //TODO  public function Filter { get; set; }
    public Font? Font { get; set; }
    //public function GenerateLabels { get; set; }
    public int? Padding { get; set; }
    public bool? UsePointStyle { get; set; }
  }

  public class LegendItem {
    public bool? Hidden { get; set; }
    public Color? FillStyle { get; set; }
    public Color? strokeStyle { get; set; }
    public int? LineDashOffset { get; set; }
    public int? LineWidth { get; set; }
    public int? Rotation { get; set; }
    public int[]? LineDash { get; set; }
    public CanvasLineCap? lLineCap { get; set; }
    public CanvasLineJoin? LineJoin { get; set; }
    public string? PointStyle { get; set; }
    public string? Text { get; set; }
  }

}