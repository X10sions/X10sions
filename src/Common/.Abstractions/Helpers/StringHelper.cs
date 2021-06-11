using Common.Constants;
using System;
using System.Text;

namespace Common.Helpers {
  public static class StringHelper {

    public static string GenerateRandomString(int stringLength, string allowedCharaters = CharactersConstants.Base32) {
      var random = new Random();
      var chars = allowedCharaters.ToCharArray();
      var sb = new StringBuilder();
      for (var i = 1; i <= stringLength; i++) {
        sb.Append(chars[random.Next(0, chars.Length)]);
      }
      return sb.ToString();
    }

  }
}
