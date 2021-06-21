using ChartJs.Options;
namespace ChartJs {
  public class ChartJsOptions {
    public Animation? Animation { get; set; }
    //    public Elements elements { get; set; }
    //    public string[]? events { get; set; }
    //    public Hover hover { get; set; }
    public Layout? Layout { get; set; }
    public Legend? Legend { get; set; }
    //    public bool? maintainAspectRatio { get; set; }
    //    public string? onClick { get; set; }
    //    public string? onResize { get; set; }
    public Title Title { get; set; } = new Title();
    public Tooltip? Tooltips { get; set; }
    public bool? Responsive { get; set; }
    //    public int? responsiveAnimationDuration { get; set; }
    public Scales Scales { get; set; } = new Scales();

  }
}