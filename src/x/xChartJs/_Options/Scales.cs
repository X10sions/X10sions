namespace ChartJs.Options {
  public class Scales {
    public Axis[]? XAxes { get; set; }
    public Axis[]? YAxes { get; set; }

    public class Axis {
      //        public bool stacked { get; set; } = true;
      //        public bool display { get; set; }
      //        public string? id { get; set; }
      //        public Type? type { get; set; }
      public Tick? Ticks { get; set; }

      //        public enum Type { time }

      public class Tick {
        public bool? BeginAtZero { get; set; }
      }
    }

  }
}