using System.Text;

namespace Common.Helpers {
  public class UserAgentHelper {
    public UserAgentHelper(string userAgentString) {
      UserAgentString = userAgentString;
      ua = new StringBuilder(userAgentString + " ");
      Parse();
    }
    StringBuilder ua;

    void Parse() {
      DetectMozilla();
      DetectMobile();
    }

    void DetectMozilla() => MozillaVersion = ParseSectionBetweenVersion("Mozilla", "/", " ", true, () => {
      DetectAppleWebKit();
      DetectGecko();
      DetectTrident();
    });

    void DetectAppleWebKit() => AppleWebKitVersion = ParseSectionBetweenVersion("AppleWebKit", "/", " ", true, () => {
      DetectCriOS();
      DetectSafari();
    });

    void DetectGecko() => GeckoVersion = ParseSectionBetweenVersion("Gecko", "/", " ", true, () => {
      DetectFirefoxVersion();
      DetectSeaMonkey();
      DetectThunderbird();
    });

    void DetectChrome() => ChromeVersion = ParseSectionBetweenVersion("Chrome", "/", " ", true, () => {
      DetectOPR();
      DetectVivaldi();
    });

    void DetectFirefoxVersion() => FirefoxVersion = ParseSectionBetweenVersion("Firefox", "/", " ");
    void DetectCriOS() => CriOSVersion = ParseSectionBetweenVersion("CriOS", "/", " ");
    void DetectEdge() => EdgeVersion = ParseSectionBetweenVersion("Edge", "/", " ");
    void DetectVivaldi() => VivaldiVersion = ParseSectionBetweenVersion("Vivaldi", "/", " ");
    void DetectSafari() => SafariVersion = ParseSectionBetweenVersion("Safari", "/", " ", true, () => {
      DetectChrome();
      DetectEdge();
    });
    void DetectOPR() => OPRVersion = ParseSectionBetweenVersion("OPR", "/", " ", true);
    void DetectThunderbird() => ThunderbirdVersion = ParseSectionBetweenVersion("Thunderbird", "/", " ");
    void DetectSeaMonkey() => SeaMonkeyVersion = ParseSectionBetweenVersion("SeaMonkey", "/", " ");
    void DetectTrident() => TridentVersion = ParseSectionBetweenVersion("Trident", "/", "; ");
    void DetectMobile() => MobileVersion = ParseSectionBetween("Mobile", "/", " ").Value;

    KeyValuePair<string, string> ParseSectionBetween(string key, string separator, string end, bool doReplace = true) {
      var uaString = ua.ToString();
      var iStart = uaString.IndexOf(key + separator, StringComparison.Ordinal);
      if (iStart < 0) {
        return new KeyValuePair<string, string>(key, string.Empty);
      }
      var iSeparator = uaString.IndexOf(separator, iStart, StringComparison.Ordinal);
      var iEnd = uaString.IndexOf(end, iSeparator + 1, StringComparison.Ordinal);
      var value = uaString.Substring(iSeparator + 1, iEnd - iSeparator - 1);
      //throw new Exception("{" + key + separator + value + "}");
      if (doReplace) {
        ua.Replace(key + separator + value, string.Empty);
      }
      return new KeyValuePair<string, string>(key, value);
    }

    Version ParseSectionBetweenVersion(string key, string separator, string end, bool doReplace = true, Action? trueAction = null) {
      var kvp = ParseSectionBetween(key, separator, end, doReplace);
      if (string.IsNullOrWhiteSpace(kvp.Value)) {
        return new Version();
      }
      var version = Version.Parse(kvp.Value + (kvp.Value.Contains(".") ? string.Empty : ".0"));
      UserAgentName = key;
      UserAgentVersion = version;
      trueAction?.Invoke();
      return version;
    }

    public string UserAgentStringNotParsed => ua.ToString();
    public string UserAgentString { get; }
    public string BrowserName => GetUserAgentLoopup?.BrowserName ?? UserAgentName;
    public Version? BrowserVersion => GetUserAgentVersionLoopup?.BrowserVersion ?? UserAgentVersion;
    public string UserAgentName { get; protected set; } = "Unknown";
    public Version? UserAgentVersion { get; protected set; }

    public Version? AppleWebKitVersion { get; protected set; }
    public Version? ChromeVersion { get; protected set; }
    public Version? CriOSVersion { get; protected set; }
    public Version? EdgeVersion { get; protected set; }
    public Version? MozillaVersion { get; protected set; }
    public Version? OPRVersion { get; protected set; }
    public Version? SafariVersion { get; protected set; }
    public Version? VivaldiVersion { get; protected set; }
    public Version? GeckoVersion { get; protected set; }
    public Version? FirefoxVersion { get; protected set; }
    public Version? ThunderbirdVersion { get; protected set; }
    public Version? SeaMonkeyVersion { get; protected set; }
    public Version? TridentVersion { get; protected set; }
    public string? MobileVersion { get; protected set; }

    public class UserAgentLoopup {
      public UserAgentLoopup(string uaName, string bName, List<UserAgentVersionLoopup>? versions = null) {
        UserAgentName = uaName;
        BrowserName = bName;
        Versions = versions;
      }
      public string UserAgentName { get; set; }
      public string BrowserName { get; set; }

      public List<UserAgentVersionLoopup>? Versions { get; set; }
    }

    public class UserAgentVersionLoopup {
      public UserAgentVersionLoopup(string uaVersion, string bVersion) {
        UserAgentVersion = Version.Parse(uaVersion);
        BrowserVersion = Version.Parse(bVersion);
      }
      public Version UserAgentVersion { get; set; }
      public Version BrowserVersion { get; set; }
    }

    public static List<UserAgentVersionLoopup> EdgeHtmlEdgeVersions { get; } = new List<UserAgentVersionLoopup> {
      new UserAgentVersionLoopup("12.10049", "0.10.10049"),
      new UserAgentVersionLoopup("12.10051", "0.11.10051"),
      new UserAgentVersionLoopup("12.10052", "0.11.10052"),
      new UserAgentVersionLoopup("12.10061", "0.11.10061"),
      new UserAgentVersionLoopup("12.10074", "0.11.10074"),
      new UserAgentVersionLoopup("12.1008", "0.11.10080"),
      new UserAgentVersionLoopup("12.10122", "13.10122"),
      new UserAgentVersionLoopup("12.1013", "15.10130"),
      new UserAgentVersionLoopup("12.10136", "16.10136"),
      new UserAgentVersionLoopup("12.10149", "19.10149"),
      new UserAgentVersionLoopup("12.10158", "20.10158"),
      new UserAgentVersionLoopup("12.10159", "20.10159"),
      new UserAgentVersionLoopup("12.10162", "20.10162"),
      new UserAgentVersionLoopup("12.10166", "20.10166"),
      new UserAgentVersionLoopup("12.1024", "20.10240"),
      new UserAgentVersionLoopup("12.10525", "20.10525"),
      new UserAgentVersionLoopup("12.10532", "20.10532"),
      new UserAgentVersionLoopup("12.10536", "20.10536"),
      new UserAgentVersionLoopup("13.10547", "21.10547"),
      new UserAgentVersionLoopup("13.10549", "21.10549"),
      new UserAgentVersionLoopup("13.10565", "23.10565"),
      new UserAgentVersionLoopup("13.10572", "25.10572"),
      new UserAgentVersionLoopup("13.10576", "25.10576"),
      new UserAgentVersionLoopup("13.10581", "25.10581"),
      new UserAgentVersionLoopup("13.10586", "25.10586"),
      new UserAgentVersionLoopup("13.11082", "25.11082"),
      new UserAgentVersionLoopup("13.11099", "27.11099"),
      new UserAgentVersionLoopup("13.11102", "28.11102"),
      new UserAgentVersionLoopup("13.14251", "28.14251"),
      new UserAgentVersionLoopup("13.14257", "28.14257"),
      new UserAgentVersionLoopup("14.14267", "31.14267"),
      new UserAgentVersionLoopup("14.14271", "31.14271"),
      new UserAgentVersionLoopup("14.14279", "31.14279"),
      new UserAgentVersionLoopup("14.14283", "31.14283"),
      new UserAgentVersionLoopup("14.14291", "34.14291"),
      new UserAgentVersionLoopup("14.14295", "34.14295"),
      new UserAgentVersionLoopup("14.143", "34.143"),
      new UserAgentVersionLoopup("14.14316", "37.14316"),
      new UserAgentVersionLoopup("14.14322", "37.14322"),
      new UserAgentVersionLoopup("14.14327", "37.14327"),
      new UserAgentVersionLoopup("14.14328", "37.14328"),
      new UserAgentVersionLoopup("14.14332", "37.14332"),
      new UserAgentVersionLoopup("14.14342", "38.14342"),
      new UserAgentVersionLoopup("14.14352", "38.14352"),
      new UserAgentVersionLoopup("14.14393", "38.14393"),
      new UserAgentVersionLoopup("14.14901", "39.14901"),
      new UserAgentVersionLoopup("14.14905", "39.14905"),
      new UserAgentVersionLoopup("14.14915", "39.14915"),
      new UserAgentVersionLoopup("14.14926", "39.14926"),
      new UserAgentVersionLoopup("14.14931", "39.14931"),
      new UserAgentVersionLoopup("14.14936", "39.14936"),
      new UserAgentVersionLoopup("15.14942", "39.14942"),
      new UserAgentVersionLoopup("15.14946", "39.14946"),
      new UserAgentVersionLoopup("15.14951", "39.14951"),
      new UserAgentVersionLoopup("15.14955", "39.14955"),
      new UserAgentVersionLoopup("15.14959", "39.14959"),
      new UserAgentVersionLoopup("15.14965", "39.14965"),
      new UserAgentVersionLoopup("15.14971", "39.14971"),
      new UserAgentVersionLoopup("15.14977", "39.14977"),
      new UserAgentVersionLoopup("15.14986", "39.14986"),
      new UserAgentVersionLoopup("15.15063", "40.15063"),
      new UserAgentVersionLoopup("16.16299", "41.16299.15"),
      new UserAgentVersionLoopup("17.17134", "42.17134"),
      new UserAgentVersionLoopup("18.17763", "44.17763")
    };

    public static List<UserAgentVersionLoopup> TridentInternetExplorerVersions { get; } = new List<UserAgentVersionLoopup> {
      new UserAgentVersionLoopup("4.0", "8.0"),
      new UserAgentVersionLoopup("5.0", "9.0"),
      new UserAgentVersionLoopup("6.0", "10.0"),
      new UserAgentVersionLoopup("7.0", "11.0"),
      new UserAgentVersionLoopup("8.0", "11.0")
    };

    public static List<UserAgentVersionLoopup> TridentInternetExplorerMobileVersions { get; } = new List<UserAgentVersionLoopup> {
      new UserAgentVersionLoopup("3.1", "7.0"),
      new UserAgentVersionLoopup("5.0", "9.0"),
      new UserAgentVersionLoopup("6.0", "10.0"),
      new UserAgentVersionLoopup("7.0", "11.0"),
      new UserAgentVersionLoopup("8.0", "11.0")
    };

    public static List<UserAgentLoopup> UserAgentLoopups { get; } = new List<UserAgentLoopup> {
      new UserAgentLoopup("OPR", "Opera"),
      new UserAgentLoopup("Edge", "Edge", EdgeHtmlEdgeVersions),
      new UserAgentLoopup("Trident", "InternetExplorer", TridentInternetExplorerVersions)
    };

    UserAgentLoopup GetUserAgentLoopup => UserAgentLoopups.FirstOrDefault(x => x.UserAgentName == UserAgentName);
    UserAgentVersionLoopup? GetUserAgentVersionLoopup => GetUserAgentLoopup?.Versions?.FirstOrDefault(x => x.UserAgentVersion == UserAgentVersion);
  }
}
