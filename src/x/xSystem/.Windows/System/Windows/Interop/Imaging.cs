using Microsoft.Internal;
using System.Drawing.Imaging;
using System.Security;
using System.Windows.Media.Imaging;

namespace System.Windows.Interop {

  public static class Imaging {
    /// <summary>Returns a managed <see cref="T:System.Windows.Media.Imaging.BitmapSource" />, based on the provided pointer to an unmanaged bitmap and palette information. </summary>
    /// <returns>The created <see cref="T:System.Windows.Media.Imaging.BitmapSource" />.</returns>
    /// <param name="bitmap">A pointer to the unmanaged bitmap.</param>
    /// <param name="palette">A pointer to the bitmap's palette map.</param>
    /// <param name="sourceRect">The size of the source image.</param>
    /// <param name="sizeOptions">A value of the enumeration that specifies how to handle conversions.</param>
    [SecurityCritical]
    public static BitmapSource CreateBitmapSourceFromHBitmap(IntPtr bitmap, IntPtr palette, Int32Rect sourceRect, BitmapSizeOptions sizeOptions) {
      SecurityHelper.DemandUnmanagedCode();
      return CriticalCreateBitmapSourceFromHBitmap(bitmap, palette, sourceRect, sizeOptions, WICBitmapAlphaChannelOption.WICBitmapUseAlpha);
    }

    [SecurityCritical]
    internal static BitmapSource CriticalCreateBitmapSourceFromHBitmap(IntPtr bitmap, IntPtr palette, Int32Rect sourceRect, BitmapSizeOptions sizeOptions, WICBitmapAlphaChannelOption alphaOptions) {
      if (bitmap == IntPtr.Zero) {
        throw new ArgumentNullException(nameof(bitmap));
      }
      return new InteropBitmap(bitmap, palette, sourceRect, sizeOptions, alphaOptions);
    }

    /// <summary>Returns a managed <see cref="T:System.Windows.Media.Imaging.BitmapSource" />, based on the provided pointer to an unmanaged icon image. </summary>
    /// <returns>The created <see cref="T:System.Windows.Media.Imaging.BitmapSource" />.</returns>
    /// <param name="icon">A pointer to the unmanaged icon source.</param>
    /// <param name="sourceRect">The size of the source image.</param>
    /// <param name="sizeOptions">A value of the enumeration that specifies how to handle conversions.</param>
    [SecurityCritical]
    public static BitmapSource CreateBitmapSourceFromHIcon(IntPtr icon, Int32Rect sourceRect, BitmapSizeOptions sizeOptions) {
      SecurityHelper.DemandUnmanagedCode();
      if (icon == IntPtr.Zero) {
        throw new ArgumentNullException(nameof(icon));
      }
      return new InteropBitmap(icon, sourceRect, sizeOptions);
    }

    /// <summary>Returns a managed <see cref="T:System.Windows.Media.Imaging.BitmapSource" />, based on the provided unmanaged memory location. </summary>
    /// <returns>The created <see cref="T:System.Windows.Media.Imaging.BitmapSource" />.</returns>
    /// <param name="section">A pointer to a memory section.</param>
    /// <param name="pixelWidth">An integer that specifies the width, in pixels, of the bitmap.</param>
    /// <param name="pixelHeight">An integer that specifies the height, in pixels, of the bitmap.</param>
    /// <param name="format">A value of the enumeration.</param>
    /// <param name="stride">The stride of the bitmap.</param>
    /// <param name="offset">The byte offset into the memory stream where the image starts.</param>
    [SecurityCritical]
    public static BitmapSource CreateBitmapSourceFromMemorySection(IntPtr section, int pixelWidth, int pixelHeight, PixelFormat format, int stride, int offset) {
      SecurityHelper.DemandUnmanagedCode();
      if (section == IntPtr.Zero) {
        throw new ArgumentNullException(nameof(section));
      }
      return new InteropBitmap(section, pixelWidth, pixelHeight, format, stride, offset);
    }
  }
}
