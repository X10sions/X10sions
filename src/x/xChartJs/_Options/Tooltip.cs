using ChartJs.Enums;
using Common.Html.Css;
using Common.Html.Css.Enums;

namespace ChartJs.Options {
  public class Tooltip {
    public bool? Enabled { get; set; }
    //TODO  public function? Custom{ get; set; }
    public ToolTipMode? Mode { get; set; }
    public bool? Intersect { get; set; }
    public ToolTipPosition? Position { get; set; }

    //public object? callbacks { get; set; }
    //public function  itemSort Sort tooltip items.more...
    //public function  filter Filter tooltip items.more...
    public Color? BackgroundColor { get; set; }
    public Font? TitleFont { get; set; }
    public TextAlign? TitleAlign { get; set; }
    public int? TitleSpacing { get; set; }
    public int? TitleMarginBottom { get; set; }
    public Font? BodyFont { get; set; }
    public TextAlign? BodyAlign { get; set; }
    public int? BodySpacing { get; set; }
    public Font? FooterFont { get; set; }
    public TextAlign? FooterAlign { get; set; }
    public int? FooterSpacing { get; set; }
    public int? FooterMarginTop { get; set; }
    public int? XPadding { get; set; }
    public int? YPadding { get; set; }
    public int? CaretPadding { get; set; }
    public int? caretSize { get; set; }
    public int? cornerRadius { get; set; }
    public Color? multiKeyBackground { get; set; }
    public bool? DisplayColors { get; set; }
    public Color? BorderColor { get; set; }
    public int? BorderWidth { get; set; }
    public bool? Rtl { get; set; }
    public string? TextDirection { get; set; }
  }
}