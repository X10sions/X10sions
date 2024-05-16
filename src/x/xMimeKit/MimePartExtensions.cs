namespace MimeKit;
public static class MimePartExtensions {
  public static long MeasureAttachmentSize(this MimePart part) {
    using (var measure = new IO.MeasuringStream()) {
      part.Content.DecodeTo(measure);
      return measure.Length;
    }
  }

}
