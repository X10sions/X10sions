using System;
using System.Security.Cryptography;

namespace Microsoft.Extensions.FileProviders {
  public static class IFileInfoExtensions {

    public static string GetHashForFile(this IFileInfo fileInfo) {
      using (var sha256 = SHA256.Create())
      using (var readStream = fileInfo.CreateReadStream()) {
        var hash = sha256.ComputeHash(readStream);
        return hash.Base64UrlEncode();
      }
    }

  }
}