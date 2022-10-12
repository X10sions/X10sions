using System.Collections;

namespace ChartJs {
  public class ChartJsData {

    public IEnumerable<string>? Labels { get; set; }
    public List<Dataset> Datasets { get; set; } = new List<Dataset>();

    //    //public string[]? xLabels { get; set; }
    //    //public string[]? yLabels { get; set; }

    public class Dataset {
      public Dataset() { }
      public Dataset(IEnumerable data) { Data = data; }

      //      //public bool? active { get; set; }
      public string[]? BackgroundColor { get; set; }
      public string[]? BorderColor { get; set; }
      public int[]? BorderDash { get; set; }
      public int? BorderWidth { get; set; }
      //      public Clip? clip { get; set; }
      public IEnumerable Data { get; set; }
      public bool? Fill { get; set; }
      //      public bool? hidden { get; set; }
      public string? Label { get; set; }
      //      public Parsing? parsing { get; set; }
      //      public int? order { get; set; }
      //      public bool? showLine { get; set; }
      //      public string? stack { get; set; }

      //      public class Clip {
      //        public int? left { get; set; }
      //        public int? top { get; set; }
      //        public int? right { get; set; }
      //        public int? bottom { get; set; }
      //      }

      //      public class Parsing {
      //        public string? yAxisKey { get; set; }
      //        public string? xAxisKey { get; set; }
      //      }

    }

  }
}