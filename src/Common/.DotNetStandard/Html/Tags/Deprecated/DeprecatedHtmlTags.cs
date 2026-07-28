using System;

namespace Common.Html.Tags;

public static class ObsoleteReason {
  public const string Not_supported_in_HTML_5 = "Not supported in HTML 5.";
  public const string Not_supported_in_browsers = "Not supported in browsers.";
}

[Obsolete("Deprecated: use '<abbr>'")] public class Acronym { }
[Obsolete("Deprecated: use '<object>'")] public class Applet { }
[Obsolete("Deprecated")] public class BaseFont { }
[Obsolete("Deprecated: use css styles")] public class Big { }
[Obsolete("Deprecated: use 'animation'")] public class Blink { }
[Obsolete("Deprecated: use 'text-align'")] public class Center { }
[Obsolete("Deprecated: use '<ul>; list-style'")] public class Dir { }
[Obsolete("Deprecated: use '<object>'")] public class Embed { }
[Obsolete("Deprecated: use css styles")] public class Font { }
[Obsolete("Deprecated: use '<iframe>'")] public class Frame { }
[Obsolete("Deprecated ")] public class FrameSet { }
[Obsolete("Deprecated: use '<form>'")] public class IsIndex { }
[Obsolete("Deprecated")] public class NoFrames { }
[Obsolete("Deprecated: use 'animation, transform'")] public class Marquee { }
[Obsolete("Deprecated: use '<ul>'")] public class Menu { }
[Obsolete("Deprecated")] public class MenuItem { }
[Obsolete("Deprecated: use '<pre>'")] public class PlainText { }
[Obsolete("Deprecated: use 'text-decoration'")] public class S { }
[Obsolete("Deprecated: use '<del>, <s>, CSS styles'")] public class Strike { }
[Obsolete("Deprecated: use 'font-family, <kbd>, <code>, <samp>'")] public class TT { }
[Obsolete("Deprecated: use 'text-decoration'")] public class U { }
