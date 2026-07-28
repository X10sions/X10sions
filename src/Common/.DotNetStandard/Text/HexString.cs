using Common.Attributes;
using Common.ValueObjects;
using System.Text;

namespace Common.Text;

public readonly record struct HexString : IValueObject<string> {
  [PreDotNet6Compatibility("Use System.Convert.ToHexString")]
  public HexString(string str) {

    var sb = new StringBuilder();
    var bytes = Encoding.UTF8.GetBytes(str); // Or another encoding like Encoding.Unicode
    foreach (var b in bytes) {
      sb.Append(b.ToString("X2")); // "X2" formats as two uppercase hexadecimal digits
    }
    Value = sb.ToString();
  }

  public HexString(byte[] ba) {
    Value = BitConverter.ToString(ba).Replace("-", "");
  }

  public string Value { get; }

  public static HexString Convert_ToHexString(string hexString) => new(hexString);

  [PreDotNet6Compatibility("Use System.Convert.FromHexString")]
  public static byte[] Convert_FromHexString(string hexString) {
    if (hexString == null) throw new ArgumentNullException(nameof(hexString));
    if (hexString.Length % 2 != 0) throw new ArgumentException("Hex string must have an even number of characters.", nameof(hexString));
    return Enumerable.Range(0, hexString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hexString.Substring(x, 2), 16)).ToArray();
  }

  public byte[] ToByteArray() {
    int NumberChars = Value.Length;
    byte[] bytes = new byte[NumberChars / 2];
    for (int i = 0; i < NumberChars; i += 2)
      bytes[i / 2] = Convert.ToByte(Value.Substring(i, 2), 16);
    return bytes;
  }

}

