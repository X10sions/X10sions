using System.Text;
using Xunit.Abstractions;

namespace Common.Testing.XUnit;
public class TestOutputHelperTextWriter(ITestOutputHelper testOutputHelper) : TextWriter {

  public static TestOutputHelperTextWriter UseConsole(ITestOutputHelper testOutputHelper) {
    using (var converter = new TestOutputHelperTextWriter(testOutputHelper)) {
      Console.SetOut(converter);
      return converter;
    }
  }

  public override Encoding Encoding => Encoding.Default;
  public override void WriteLine(string value) => testOutputHelper.WriteLine(value);
  public override void WriteLine(string format, params object[] arg) => testOutputHelper.WriteLine(format, arg);
}