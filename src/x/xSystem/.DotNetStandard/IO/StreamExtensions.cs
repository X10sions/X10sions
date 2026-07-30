using System.Text;

namespace System.IO;
public static class StreamExtensions {

  public static Task WriteAsync(this Stream stream, string value, CancellationToken token = default) {
    var bytes = Encoding.UTF8.GetBytes(value);
    return stream.WriteAsync(bytes, 0, bytes.Length, token);
  }

}