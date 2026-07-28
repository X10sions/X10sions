using System.Text;

namespace Common.Text;

public static class HexConverter {

  public static HexString ToHexString(this byte[] ba) {
    StringBuilder hex = new StringBuilder(ba.Length * 2);
    foreach (byte b in ba)
      hex.AppendFormat("{0:x2}", b);
    return new(hex.ToString());
  }

}

