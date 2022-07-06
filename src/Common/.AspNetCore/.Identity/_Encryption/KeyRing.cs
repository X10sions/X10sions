using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace Common.AspNetCore.Identity {
  public class KeyRing : ILookupProtectorKeyRing {
    // https://github.com/blowdart/AspNetCoreIdentityEncryption/blob/master/AspNetCoreIdentityEncryption/KeyRing.cs
    private readonly IDictionary<string, string> _keyDictionary = new Dictionary<string, string>();

    public KeyRing(IHostEnvironment hostingEnvironment) {
      // Create the keyring directory if one doesn't exist.
      var keyRingDirectory = Path.Combine(hostingEnvironment.ContentRootPath, "keyring");
      Directory.CreateDirectory(keyRingDirectory);

      var directoryInfo = new DirectoryInfo(keyRingDirectory);
      if (directoryInfo.GetFiles("*.key").Length == 0) {
        ProtectorAlgorithmHelper.GetAlgorithms(
            ProtectorAlgorithmHelper.DefaultAlgorithm,
            out var encryptionAlgorithm,
            out var signingAlgorithm,
            out var derivationCount);
        encryptionAlgorithm.GenerateKey();

        var keyAsString = Convert.ToBase64String(encryptionAlgorithm.Key);
        var keyId = Guid.NewGuid().ToString();
        var keyFileName = Path.Combine(keyRingDirectory, keyId + ".key");
        using (var file = File.CreateText(keyFileName)) {
          file.WriteLine(keyAsString);
        }

        _keyDictionary.Add(keyId, keyAsString);

        CurrentKeyId = keyId;

        encryptionAlgorithm.Clear();
        encryptionAlgorithm.Dispose();
        signingAlgorithm.Dispose();
      } else {
        var filesOrdered = directoryInfo.EnumerateFiles()
                            .OrderByDescending(d => d.CreationTime)
                            .Select(d => d.Name)
                            .ToList();

        foreach (var fileName in filesOrdered) {
          var keyFileName = Path.Combine(keyRingDirectory, fileName);
          var key = File.ReadAllText(keyFileName);
          var keyId = Path.GetFileNameWithoutExtension(fileName);
          _keyDictionary.Add(keyId, key);
          CurrentKeyId = keyId;
        }
      }
    }

    public string this[string keyId] => _keyDictionary[keyId];

    public string CurrentKeyId {
      get; private set;
    }

    public IEnumerable<string> GetAllKeyIds() => _keyDictionary.Keys;
  }
}