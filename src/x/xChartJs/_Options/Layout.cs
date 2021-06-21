namespace ChartJs.Options {
  public class Layout {
    public Padding? Padding { get; set; }
  }

  public class Padding {
    public Padding(int padding) : this(padding, padding) { }
    public Padding(int vertical, int horizontal) : this(vertical, horizontal, vertical, horizontal) { }
    public Padding(int top, int horizontal, int bottom) : this(top, horizontal, bottom, horizontal) { }
    public Padding(int top, int right, int bottom, int left) {
      Bottom = bottom;
      Left = left;
      Right = right;
      Top = top;
    }
    public int? Bottom { get; set; }
    public int? Left { get; set; }
    public int? Right { get; set; }
    public int? Top { get; set; }

  }

}