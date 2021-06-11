using Microsoft.WindowsAPICodePack.Internal;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace Microsoft.WindowsAPICodePack.Shell {
  /// <summary>
  /// Represents a thumbnail or an icon for a ShellObject.
  /// </summary>
  public class ShellThumbnail {
    IShellItem shellItemNative;
    Size currentSize = new Size(256, 256);

    internal ShellThumbnail(ShellObject shellObject) {
      if (shellObject == null || shellObject.NativeShellItem == null) {
        throw new ArgumentNullException(nameof(shellObject));
      }
      shellItemNative = shellObject.NativeShellItem;
    }

    public Size CurrentSize {
      get { return currentSize; }
      set {
        if (value.Height == 0 || value.Width == 0) {
          throw new ArgumentOutOfRangeException(nameof(value), LocalizedMessages.ShellThumbnailSizeCannotBe0);
        }
        var size = (FormatOption == ShellThumbnailFormatOption.IconOnly) ? DefaultIconSize.Maximum : DefaultThumbnailSize.Maximum;
        if (value.Height > size.Height || value.Width > size.Width) {
          throw new ArgumentOutOfRangeException(nameof(value),
              string.Format(System.Globalization.CultureInfo.InvariantCulture,
              LocalizedMessages.ShellThumbnailCurrentSizeRange, size.ToString()));
        }
        currentSize = value;
      }
    }

    public Bitmap Bitmap => GetBitmap(CurrentSize);
    public BitmapSource BitmapSource => GetBitmapSource(CurrentSize);
    public Icon Icon => Icon.FromHandle(Bitmap.GetHicon());
    public Bitmap SmallBitmap => GetBitmap(DefaultIconSize.Small, DefaultThumbnailSize.Small);
    public BitmapSource SmallBitmapSource => GetBitmapSource(DefaultIconSize.Small, DefaultThumbnailSize.Small);
    public Icon SmallIcon => Icon.FromHandle(SmallBitmap.GetHicon());
    public Bitmap MediumBitmap => GetBitmap(DefaultIconSize.Medium, DefaultThumbnailSize.Medium);
    public BitmapSource MediumBitmapSource => GetBitmapSource(DefaultIconSize.Medium, DefaultThumbnailSize.Medium);
    public Icon MediumIcon { get { return Icon.FromHandle(MediumBitmap.GetHicon()); } }
    public Bitmap LargeBitmap => GetBitmap(DefaultIconSize.Large, DefaultThumbnailSize.Large);
    public BitmapSource LargeBitmapSource => GetBitmapSource(DefaultIconSize.Large, DefaultThumbnailSize.Large);
    public Icon LargeIcon => Icon.FromHandle(LargeBitmap.GetHicon());
    public Bitmap ExtraLargeBitmap => GetBitmap(DefaultIconSize.ExtraLarge, DefaultThumbnailSize.ExtraLarge);
    public BitmapSource ExtraLargeBitmapSource => GetBitmapSource(DefaultIconSize.ExtraLarge, DefaultThumbnailSize.ExtraLarge);
    public Icon ExtraLargeIcon => Icon.FromHandle(ExtraLargeBitmap.GetHicon());
    public ShellThumbnailRetrievalOption RetrievalOption { get; set; }

    ShellThumbnailFormatOption formatOption = ShellThumbnailFormatOption.Default;

    public ShellThumbnailFormatOption FormatOption {
      get => formatOption;
      set {
        formatOption = value;
        // Do a similar check as we did in CurrentSize property setter,
        // If our mode is IconOnly, then our max is defined by DefaultIconSize.Maximum. We should make sure 
        // our CurrentSize is within this max range
        if (FormatOption == ShellThumbnailFormatOption.IconOnly
            && (CurrentSize.Height > DefaultIconSize.Maximum.Height || CurrentSize.Width > DefaultIconSize.Maximum.Width)) {
          CurrentSize = DefaultIconSize.Maximum;
        }
      }

    }

    public bool AllowBiggerSize { get; set; }

    ShellNativeMethods.SIIGBF CalculateFlags() {
      ShellNativeMethods.SIIGBF flags = 0x0000;

      if (AllowBiggerSize) {
        flags |= ShellNativeMethods.SIIGBF.BiggerSizeOk;
      }
      if (RetrievalOption == ShellThumbnailRetrievalOption.CacheOnly) {
        flags |= ShellNativeMethods.SIIGBF.InCacheOnly;
      } else if (RetrievalOption == ShellThumbnailRetrievalOption.MemoryOnly) {
        flags |= ShellNativeMethods.SIIGBF.MemoryOnly;
      }
      if (FormatOption == ShellThumbnailFormatOption.IconOnly) {
        flags |= ShellNativeMethods.SIIGBF.IconOnly;
      } else if (FormatOption == ShellThumbnailFormatOption.ThumbnailOnly) {
        flags |= ShellNativeMethods.SIIGBF.ThumbnailOnly;
      }
      return flags;
    }

    IntPtr GetHBitmap(Size size) {
      var hbitmap = IntPtr.Zero;
      var nativeSIZE = new CoreNativeMethods.Size { Width = Convert.ToInt32(size.Width), Height = Convert.ToInt32(size.Height) };
      // Use IShellItemImageFactory to get an icon
      // Options passed in: Resize to fit
      var hr = ((IShellItemImageFactory)shellItemNative).GetImage(nativeSIZE, CalculateFlags(), out hbitmap);
      if (hr == HResult.Ok) { return hbitmap; } else if ((uint)hr == 0x8004B200 && FormatOption == ShellThumbnailFormatOption.ThumbnailOnly) {
        // Thumbnail was requested, but this ShellItem doesn't have a thumbnail.
        throw new InvalidOperationException(LocalizedMessages.ShellThumbnailDoesNotHaveThumbnail, Marshal.GetExceptionForHR((int)hr));
      } else if ((uint)hr == 0x80040154) // REGDB_E_CLASSNOTREG
        {
        throw new NotSupportedException(LocalizedMessages.ShellThumbnailNoHandler, Marshal.GetExceptionForHR((int)hr));
      }
      throw new ShellException(hr);
    }

    Bitmap GetBitmap(Size iconOnlySize, Size thumbnailSize) {
      return GetBitmap(FormatOption == ShellThumbnailFormatOption.IconOnly ? iconOnlySize : thumbnailSize);
    }

    Bitmap GetBitmap(Size size) {
      var hBitmap = GetHBitmap(size);
      // return a System.Drawing.Bitmap from the hBitmap
      var returnValue = Bitmap.FromHbitmap(hBitmap);
      // delete HBitmap to avoid memory leaks
      ShellNativeMethods.DeleteObject(hBitmap);
      return returnValue;
    }

    BitmapSource GetBitmapSource(Size iconOnlySize, Size thumbnailSize) => GetBitmapSource(FormatOption == ShellThumbnailFormatOption.IconOnly ? iconOnlySize : thumbnailSize);

    [Obsolete(".NET Framework only", false)]
    BitmapSource GetBitmapSource(Size size) {
      throw new NotImplementedException();
      //TODO  var hBitmap = GetHBitmap(size);
      //TODO  BitmapSource returnValue = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
      //TODO  ShellNativeMethods.DeleteObject(hBitmap);
      //TODO  return returnValue;
    }

  }
}