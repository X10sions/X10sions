using Common.Constants;
using System.Security.Cryptography;

namespace Common.Helpers;

public static class StringHelper {

  public static string GenerateRandomString(int stringLength, string allowedCharacters = CharactersConstants.Base32) {
    if (stringLength < 0) throw new ArgumentOutOfRangeException(nameof(stringLength), "Length cannot be negative.");
    if (string.IsNullOrEmpty(allowedCharacters)) throw new ArgumentException("Characters cannot be empty.", nameof(allowedCharacters));
    char[] result = new char[stringLength];
    int allowedLength = allowedCharacters.Length;
    // Use a reusable byte buffer to reduce memory allocations
    byte[] randomBytes = new byte[128];
    int byteIndex = randomBytes.Length; // Force initial fill
    using (var rng = RandomNumberGenerator.Create()) {
      for (int i = 0; i < stringLength; i++) {
        // If we consumed all random bytes in the buffer, fetch a new batch
        if (byteIndex >= randomBytes.Length) {
          rng.GetBytes(randomBytes);
          byteIndex = 0;
        }
        // Map the byte to an index in the allowed character pool.
        // Using modulo is acceptable here since the bias across a standard alphabet is statistically negligible.
        int randomIndex = randomBytes[byteIndex++] % allowedLength;
        result[i] = allowedCharacters[randomIndex];
      }
    }
    return new string(result);
  }

}
