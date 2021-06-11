using System.Drawing;

namespace Microsoft.WindowsAPICodePack.Shell {
  /// <summary>
  /// Defines the read-only properties for default shell icon sizes.
  /// </summary>
  public static class DefaultIconSize {
    /// <summary>
    /// The small size property for a 16x16 pixel Shell Icon.
    /// </summary>
    public static readonly Size Small = new Size(16, 16);

    /// <summary>
    /// The medium size property for a 32x32 pixel Shell Icon.
    /// </summary>
    public static readonly Size Medium = new Size(32, 32);

    /// <summary>
    /// The large size property for a 48x48 pixel Shell Icon.
    /// </summary>
    public static readonly Size Large = new Size(48, 48);

    /// <summary>
    /// The extra-large size property for a 256x256 pixel Shell Icon.
    /// </summary>
    public static readonly Size ExtraLarge = new Size(256, 256);

    /// <summary>
    /// The maximum size for a Shell Icon, 256x256 pixels.
    /// </summary>
    public static readonly Size Maximum = new Size(256, 256);

  }
}
