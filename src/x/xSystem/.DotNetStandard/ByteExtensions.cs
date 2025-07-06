using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace System {
  public static class ByteExtensions {

    static readonly string EncoderResources_WebEncoders_InvalidCountOffsetOrLength = "Invalid {0}, {1} or {2} length.";

    public static string Base64UrlEncode(this byte[] input) {
      if (input == null) {
        throw new ArgumentNullException(nameof(input));
      }
      return input.Base64UrlEncode(0, input.Length);
    }

    public static string Base64UrlEncode(this byte[] input, int offset, int count) {
      if (input == null) {
        throw new ArgumentNullException(nameof(input));
      }
      ValidateParameters(input.Length, nameof(input), offset, count);
      // Special-case empty input
      if (count == 0) {
        return string.Empty;
      }
      var buffer = new char[count.GetArraySizeRequiredToEncode()];
      var numBase64Chars = input.Base64UrlEncode(offset, buffer, 0, count);
      return new string(buffer, 0, numBase64Chars);
    }

    public static int Base64UrlEncode(this byte[] input, int offset, char[] output, int outputOffset, int count) {
      if (input == null) {
        throw new ArgumentNullException(nameof(input));
      }
      if (output == null) {
        throw new ArgumentNullException(nameof(output));
      }

      ValidateParameters(input.Length, nameof(input), offset, count);
      if (outputOffset < 0) {
        throw new ArgumentOutOfRangeException(nameof(outputOffset));
      }

      var arraySizeRequired = count.GetArraySizeRequiredToEncode();
      if (output.Length - outputOffset < arraySizeRequired) {
        throw new ArgumentException(
            string.Format(
                CultureInfo.CurrentCulture,
                EncoderResources_WebEncoders_InvalidCountOffsetOrLength,
                nameof(count),
                nameof(outputOffset),
                nameof(output)),
            nameof(count));
      }

      // Special-case empty input.
      if (count == 0) {
        return 0;
      }

      // Use base64url encoding with no padding characters. See RFC 4648, Sec. 5.

      // Start with default Base64 encoding.
      var numBase64Chars = Convert.ToBase64CharArray(input, offset, count, output, outputOffset);

      // Fix up '+' -> '-' and '/' -> '_'. Drop padding characters.
      for (var i = outputOffset; i - outputOffset < numBase64Chars; i++) {
        var ch = output[i];
        if (ch == '+') {
          output[i] = '-';
        } else if (ch == '/') {
          output[i] = '_';
        } else if (ch == '=') {
          // We've reached a padding character; truncate the remainder.
          return i - outputOffset;
        }
      }

      return numBase64Chars;
    }


    public static T? Get<T>(this byte[] bytes) {
      if (bytes == null) {
        return default(T);
      }
      using (var memoryStream = new MemoryStream(bytes)) {
        return (T)new BinaryFormatter().Deserialize(memoryStream);
      }
    }

    public static BinaryReader? GetBinaryReader(this byte[] bytes) {
      if (bytes == null) {
        return null;
      }
      using (var memoryStream = new MemoryStream(bytes)) {
        return new BinaryReader(memoryStream);
      }
    }

    public static bool? GetBoolean(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadBoolean();
    public static byte? GetByte(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadByte();
    public static byte[]? GetBytes(this byte[] bytes, int count) => bytes?.GetBinaryReader()?.ReadBytes(count);
    public static char? GetChar(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadChar();
    public static char[]? GetChars(this byte[] bytes, int count) => bytes?.GetBinaryReader()?.ReadChars(count);
    public static decimal? GetDecimal(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadDecimal();
    public static double? GetDouble(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadDouble();
    public static short? GetInt16(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadInt16();
    public static int? GetInt32(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadInt32();
    public static long? GetInt64(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadInt64();
    public static sbyte? GeSByte(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadSByte();
    public static float? GetSingle(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadSingle();
    public static string? GetString(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadString();
    public static ushort? GetUInt16(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadUInt16();
    public static uint? GetUInt32(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadUInt32();
    public static ulong? GetUInt64(this byte[] bytes) => bytes?.GetBinaryReader()?.ReadUInt64();


    static void ValidateParameters(int bufferLength, string inputName, int offset, int count) {
      if (offset < 0) {
        throw new ArgumentOutOfRangeException(nameof(offset));
      }
      if (count < 0) {
        throw new ArgumentOutOfRangeException(nameof(count));
      }
      if (bufferLength - offset < count) {
        throw new ArgumentException(
            string.Format(
                CultureInfo.CurrentCulture,
                EncoderResources_WebEncoders_InvalidCountOffsetOrLength,
                nameof(count),
                nameof(offset),
                inputName),
            nameof(count));
      }
    }
  }
}