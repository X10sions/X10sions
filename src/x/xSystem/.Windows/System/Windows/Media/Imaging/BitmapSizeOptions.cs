namespace System.Windows.Media.Imaging {
  [Obsolete("Use:")]
  public class BitmapSizeOptions {
    /// <summary>Gets a value that determines whether the aspect ratio of the original bitmap image is preserved.</summary>
    /// <returns>true if the original aspect ratio is maintained; otherwise, false.</returns>
    public bool PreservesAspectRatio { get; private set; }

    /// <summary>The width, in pixels, of the bitmap image.</summary>
    /// <returns>The width of the bitmap.</returns>
    public int PixelWidth { get; private set; }

    /// <summary>The height, in pixels, of the bitmap image.</summary>
    /// <returns>The height of the bitmap.</returns>
    public int PixelHeight { get; private set; }

    /// <summary>Gets a value that represents the rotation angle that is applied to a bitmap. </summary>
    /// <returns>The rotation angle that is applied to the image.</returns>
    public Rotation Rotation { get; private set; }

    internal bool DoesScale {
      get {
        if (PixelWidth == 0) {
          return PixelHeight != 0;
        }
        return true;
      }
    }

    internal WICBitmapTransformOptions WICTransformOptions {
      get {
        WICBitmapTransformOptions result = WICBitmapTransformOptions.WICBitmapTransformRotate0;
        switch (Rotation) {
          case Rotation.Rotate0:
            result = WICBitmapTransformOptions.WICBitmapTransformRotate0;
            break;
          case Rotation.Rotate90:
            result = WICBitmapTransformOptions.WICBitmapTransformRotate90;
            break;
          case Rotation.Rotate180:
            result = WICBitmapTransformOptions.WICBitmapTransformRotate180;
            break;
          case Rotation.Rotate270:
            result = WICBitmapTransformOptions.WICBitmapTransformRotate270;
            break;
        }
        return result;
      }
    }

    private BitmapSizeOptions() {
    }

    /// <summary>Initializes a new instance of <see cref="T:System.Windows.Media.Imaging.BitmapSizeOptions" /> with empty sizing properties.</summary>
    /// <returns>An instance of <see cref="T:System.Windows.Media.Imaging.BitmapSizeOptions" />.</returns>
    public static BitmapSizeOptions FromEmptyOptions() {
      BitmapSizeOptions bitmapSizeOptions = new BitmapSizeOptions {
        Rotation = Rotation.Rotate0,
        PreservesAspectRatio = true,
        PixelHeight = 0,
        PixelWidth = 0
      };
      return bitmapSizeOptions;
    }

    /// <summary>Initializes an instance of <see cref="T:System.Windows.Media.Imaging.BitmapSizeOptions" /> that preserves the aspect ratio of the source bitmap and specifies an initial <see cref="P:System.Windows.Media.Imaging.BitmapSizeOptions.PixelHeight" />.</summary>
    /// <returns>An instance of <see cref="T:System.Windows.Media.Imaging.BitmapSizeOptions" />.</returns>
    /// <param name="pixelHeight">The height, in pixels, of the resulting bitmap.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Occurs when <paramref name="pixelHeight" /> is less than zero.</exception>
    public static BitmapSizeOptions FromHeight(int pixelHeight) {
      if (pixelHeight <= 0) {
        throw new ArgumentOutOfRangeException("pixelHeight", SR.Get("ParameterMustBeGreaterThanZero"));
      }
      BitmapSizeOptions bitmapSizeOptions = new BitmapSizeOptions {
        Rotation = Rotation.Rotate0,
        PreservesAspectRatio = true,
        PixelHeight = pixelHeight,
        PixelWidth = 0
      };
      return bitmapSizeOptions;
    }

    /// <summary>Initializes an instance of <see cref="T:System.Windows.Media.Imaging.BitmapSizeOptions" /> that preserves the aspect ratio of the source bitmap and specifies an initial <see cref="P:System.Windows.Media.Imaging.BitmapSizeOptions.PixelWidth" />.</summary>
    /// <returns>An instance of <see cref="T:System.Windows.Media.Imaging.BitmapSizeOptions" />.</returns>
    /// <param name="pixelWidth">The width, in pixels, of the resulting bitmap.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Occurs when <paramref name="pixelWidth" /> is less than zero.</exception>
    public static BitmapSizeOptions FromWidth(int pixelWidth) {
      if (pixelWidth <= 0) {
        throw new ArgumentOutOfRangeException("pixelWidth", SR.Get("ParameterMustBeGreaterThanZero"));
      }
      BitmapSizeOptions bitmapSizeOptions = new BitmapSizeOptions {
        Rotation = Rotation.Rotate0,
        PreservesAspectRatio = true,
        PixelWidth = pixelWidth,
        PixelHeight = 0
      };
      return bitmapSizeOptions;
    }

    /// <summary>Initializes an instance of <see cref="T:System.Windows.Media.Imaging.BitmapSizeOptions" /> that does not preserve the original bitmap aspect ratio.</summary>
    /// <returns>A new instance of <see cref="T:System.Windows.Media.Imaging.BitmapSizeOptions" />.</returns>
    /// <param name="pixelWidth">The width, in pixels, of the resulting bitmap.</param>
    /// <param name="pixelHeight">The height, in pixels, of the resulting bitmap.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Occurs when <paramref name="pixelWidth" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Occurs when <paramref name="pixelHeight" /> is less than zero.</exception>
    public static BitmapSizeOptions FromWidthAndHeight(int pixelWidth, int pixelHeight) {
      if (pixelWidth <= 0) {
        throw new ArgumentOutOfRangeException("pixelWidth", SR.Get("ParameterMustBeGreaterThanZero"));
      }
      if (pixelHeight <= 0) {
        throw new ArgumentOutOfRangeException("pixelHeight", SR.Get("ParameterMustBeGreaterThanZero"));
      }
      BitmapSizeOptions bitmapSizeOptions = new BitmapSizeOptions {
        Rotation = Rotation.Rotate0,
        PreservesAspectRatio = false,
        PixelWidth = pixelWidth,
        PixelHeight = pixelHeight
      };
      return bitmapSizeOptions;
    }

    /// <summary>Initializes an instance of <see cref="T:System.Windows.Media.Imaging.BitmapSizeOptions" /> that preserves the aspect ratio of the source bitmap and specifies an initial <see cref="T:System.Windows.Media.Imaging.Rotation" /> to apply.</summary>
    /// <returns>A new instance of <see cref="T:System.Windows.Media.Imaging.BitmapSizeOptions" />.</returns>
    /// <param name="rotation">The initial rotation value to apply. Only 90 degree increments are supported.</param>
    public static BitmapSizeOptions FromRotation(Rotation rotation) {
      switch (rotation) {
        default:
          throw new ArgumentException(SR.Get("Image_SizeOptionsAngle"), "rotation");
        case Rotation.Rotate0:
        case Rotation.Rotate90:
        case Rotation.Rotate180:
        case Rotation.Rotate270: {
            BitmapSizeOptions bitmapSizeOptions = new BitmapSizeOptions {
              Rotation = rotation,
              PreservesAspectRatio = true,
              PixelWidth = 0,
              PixelHeight = 0
            };
            return bitmapSizeOptions;
          }
      }
    }

    internal void GetScaledWidthAndHeight(uint width, uint height, out uint newWidth, out uint newHeight) {
      if (PixelWidth == 0 && PixelHeight != 0) {
        newWidth = (uint)(PixelHeight * width / height);
        newHeight = (uint)PixelHeight;
      } else if (PixelWidth != 0 && PixelHeight == 0) {
        newWidth = (uint)PixelWidth;
        newHeight = (uint)(PixelWidth * height / width);
      } else if (PixelWidth != 0 && PixelHeight != 0) {
        newWidth = (uint)PixelWidth;
        newHeight = (uint)PixelHeight;
      } else {
        newWidth = width;
        newHeight = height;
      }
    }
  }


}
