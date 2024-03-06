namespace Common.Html.Tags;
public class Script : HtmlTag5Base<Script> {
  #region Attributes

  /// <summary>
  /// Specifies that the script is downloaded in parallel to parsing the page, and executed as soon as it is available (before parsing completes) (only for external scripts)
  /// </summary>
  public bool? Async { get => GetAttribute<bool?>(nameof(Async)); set => attributes[nameof(Async)] = value; }
  /// <summary>
  /// Sets the mode of the request to an HTTP CORS Request
  /// </summary>
  public Script_CrossOrigin? CrossOrigin { get => GetAttribute<Script_CrossOrigin?>(nameof(CrossOrigin)); set => attributes[nameof(CrossOrigin)] = value; }
  /// <summary>
  /// Specifies that the script is downloaded in parallel to parsing the page, and executed after the page has finished parsing (only for external scripts)
  /// </summary>
  public bool? Defer { get => GetAttribute<bool?>(nameof(Defer)); set => attributes[nameof(Defer)] = value; }
  /// <summary>
  /// Allows a browser to check the fetched script to ensure that the code is never loaded if the source has been manipulated
  /// </summary>
  public string? FileHash { get => GetAttribute<string?>(nameof(FileHash)); set => attributes[nameof(FileHash)] = value; }
  /// <summary>
  /// Specifies that the script should not be executed in browsers supporting ES2015 modules
  /// </summary>
  public bool? NoModule { get => GetAttribute<bool?>(nameof(NoModule)); set => attributes[nameof(NoModule)] = value; }

  /// <summary>
  /// Specifies which referrer information to send when fetching a script
  /// </summary>
  public Script_ReferrerPolicy? ReferrerPolicy { get => GetAttribute<Script_ReferrerPolicy?>(nameof(ReferrerPolicy)); set => attributes[nameof(ReferrerPolicy)] = value; }

  /// <summary>
  /// Specifies the URL of an external script file
  /// </summary>
  public string? Src { get => GetAttribute<string?>(nameof(Src)); set => attributes[nameof(Src)] = value; }

  /// <summary>
  /// Specifies the media type of the script 
  /// </summary>
  public string? Type { get => GetAttribute<string?>(nameof(Type)); set => attributes[nameof(Type)] = value; }

  #endregion

  public string Contents { get; set; } = string.Empty;

  public override string ToHtml() => this.ToHtml(Contents);

  public static string InlineScriptHtmlTag(string contents) => $"<script>{contents}</script>";
  public static string ScriptHtmlTagString(string src) => $"<script src=\"{src}\"></script>";

  public enum Script_CrossOrigin { anonymous, use_credentials }

  public enum Script_ReferrerPolicy {
    no_referrer,
    no_referrer_when_downgrade,
    origin,
    origin_when_cross_origin,
    same_origin,
    strict_origin,
    strict_origin_when_cross_origin,
    unsafe_url
  }

}
