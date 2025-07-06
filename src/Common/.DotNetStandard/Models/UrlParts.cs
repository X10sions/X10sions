namespace Common.Models;

public class UrlParts {

  public string Scheme { get; set; }
  public string SchemePart => Scheme + "://";

  public UrlUserInfo UserInfo { get; set; } = new UrlUserInfo();
  public string? UserName { get => UserInfo.UserName; set => UserInfo.UserName = value; }
  public string? Password { get => UserInfo.Password; set => UserInfo.UserName = value; }

  public UrlHostInfo HostInfo { get; set; } = new UrlHostInfo();
  public string? HostSubDomain { get => HostInfo.SubDomain; set => HostInfo.SubDomain = value; }
  public string? HostDomain { get => HostInfo.Domain; set => HostInfo.Domain = value; }
  public int? Port { get => HostInfo.Port; set => HostInfo.Port = value; }

  public string PathBase { get; set; }
  public string Path { get; set; }

  public string QueryString {
    get => QueryStringItems.HasAnyItems() ? string.Join("&", QueryStringItems.Select(x => $"{x.Key}={x.Value}")) : string.Empty;
    set => QueryStringItems = value.TrimStart('?').Split('&').Select(x => new QueryStringItem(x)).ToList();
  }
  public string QueryStringPart { get => QueryString.WrapIfNotNullOrWhiteSpace("?"); set => QueryString = value.TrimStart('?'); }

  public List<QueryStringItem> QueryStringItems { get; set; } = new List<QueryStringItem>();
  public class QueryStringItem {
    public QueryStringItem(string keyValue) {
      var i = keyValue.IndexOf('=');
      if (i > 0) {
        Key = keyValue.Substring(0, i);
        Value = keyValue.Substring(i + 1);
      } else {
        Key = keyValue;
      }
    }
    public QueryStringItem(string key, string value) {
      Key = key;
      Value = value;
    }

    public string Key { get; set; }
    public object Value { get; set; }
    public override string ToString() => Key + Value.ToString().WrapIfNotNullOrWhiteSpace("=");
  }

  public string? Fragment { get; set; }
  public string FragmentPart { get => Fragment.WrapIfNotNullOrWhiteSpace("#"); set => Fragment = value.TrimStart('#'); }

  public override string ToString() => SchemePart + UserInfo.ToString() + HostInfo.ToString() + PathBase + Path + QueryStringPart + FragmentPart;

  public class UrlHostInfo {
    public string? SubDomain { get; set; }
    public string SubDomainPart { get => SubDomain.WrapIfNotNullOrWhiteSpace(string.Empty, "."); set => value.TrimEnd('.'); }
    public string? Domain { get; set; }
    public int? Port { get; set; }
    public string PortPart { get => Port.WrapIfNotNull(":"); set => Port = int.Parse(value.TrimStart(':')); }

    public override string ToString() => SubDomainPart + Domain + PortPart;
  }

  public class UrlUserInfo {
    public string? UserName { get; set; }
    public string? UserNamePart { get => UserName.WrapIfNotNullOrWhiteSpace("@"); set => UserName = value.TrimStart('@'); }
    public string? Password { get; set; }
    public string PasswordPart { get => Password.WrapIfNotNullOrWhiteSpace(":"); set => UserName = value.TrimStart(':'); }

    public override string ToString() => UserNamePart + PasswordPart;
  }

}