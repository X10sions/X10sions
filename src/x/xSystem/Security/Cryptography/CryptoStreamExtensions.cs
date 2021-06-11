namespace System.Security.Cryptography {
  public static class CryptoStreamExtensions {
    public static void Write(this CryptoStream cryptoStream, byte[] buffer) => cryptoStream.Write(buffer, 0, buffer.Length);
  }
}
