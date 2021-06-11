using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Media.Imaging {
  /// <summary>Represents a single, constant set of pixels at a certain size and resolution.</summary>
  [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
  public abstract class BitmapSource : ImageSource, DUCE.IResource {
    [ComImport]
    [Guid("00000120-a8f2-4877-ba0a-fd2b6645fb94")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IWICBitmapSource {
      [PreserveSig] int GetSize(out int puiWidth, out int puiHeight);
      [PreserveSig] int GetPixelFormat(out Guid guidFormat);
      [PreserveSig] int GetResolution(out double pDpiX, out double pDpiY);
      [PreserveSig] int GetPalette(IntPtr pIPalette);
      [PreserveSig] int CopyPixels(IntPtr prc, int cbStride, int cbPixels, IntPtr pvPixels);
    }

    [ClassInterface(ClassInterfaceType.None)]
    internal class ManagedBitmapSource : IWICBitmapSource {
      private WeakReference<BitmapSource> _bitmapSource;

      public ManagedBitmapSource(BitmapSource bitmapSource) {
        if (bitmapSource == null) {
          throw new ArgumentNullException(nameof(bitmapSource));
        }
        _bitmapSource = new WeakReference<BitmapSource>(bitmapSource);
      }

      int IWICBitmapSource.GetSize(out int puiWidth, out int puiHeight) {
        if (_bitmapSource.TryGetTarget(out BitmapSource target)) {
          puiWidth = target.PixelWidth;
          puiHeight = target.PixelHeight;
          return 0;
        }
        puiWidth = 0;
        puiHeight = 0;
        return -2147467259;
      }

      int IWICBitmapSource.GetPixelFormat(out Guid guidFormat) {
        if (_bitmapSource.TryGetTarget(out BitmapSource target)) {
          guidFormat = target.Format.Guid;
          return 0;
        }
        guidFormat = Guid.Empty;
        return -2147467259;
      }

      int IWICBitmapSource.GetResolution(out double pDpiX, out double pDpiY) {
        if (_bitmapSource.TryGetTarget(out BitmapSource target)) {
          pDpiX = target.DpiX;
          pDpiY = target.DpiY;
          return 0;
        }
        pDpiX = 0.0;
        pDpiY = 0.0;
        return -2147467259;
      }

      [SecurityCritical]
      int IWICBitmapSource.GetPalette(IntPtr pIPalette) {
        if (_bitmapSource.TryGetTarget(out BitmapSource target)) {
          BitmapPalette palette = target.Palette;
          if (palette == null || palette.InternalPalette == null || palette.InternalPalette.IsInvalid) {
            return -2003292347;
          }
          HRESULT.Check(UnsafeNativeMethods.WICPalette.InitializeFromPalette(pIPalette, palette.InternalPalette));
          return 0;
        }
        return -2147467259;
      }

      [SecurityCritical]
      int IWICBitmapSource.CopyPixels(IntPtr prc, int cbStride, int cbPixels, IntPtr pvPixels) {
        if (cbStride < 0) {
          return -2147024809;
        }
        if (pvPixels == IntPtr.Zero) {
          return -2147024809;
        }
        if (_bitmapSource.TryGetTarget(out BitmapSource target)) {
          Int32Rect sourceRect = (!(prc == IntPtr.Zero)) ? ((Int32Rect)Marshal.PtrToStructure(prc, typeof(Int32Rect))) : new Int32Rect(0, 0, target.PixelWidth, target.PixelHeight);
          int height = sourceRect.Height;
          int width = sourceRect.Width;
          if (sourceRect.Width < 1 || sourceRect.Height < 1) {
            return -2147024809;
          }
          PixelFormat format = target.Format;
          if (format.Format == PixelFormatEnum.Default || format.Format == PixelFormatEnum.Default) {
            return -2003292288;
          }
          int num = checked(width * format.InternalBitsPerPixel + 7) / 8;
          byte[] array;
          long num3;
          checked {
            if (cbPixels < (height - 1) * cbStride + num) {
              return -2003292276;
            }
            int num2 = height * num;
            array = new byte[num2];
            target.CopyPixels(sourceRect, array, num, 0);
            num3 = pvPixels.ToInt64();
          }
          for (int i = 0; i < height; i++) {
            Marshal.Copy(array, i * num, new IntPtr(num3), num);
            num3 += cbStride;
          }
          return 0;
        }
        return -2147467259;
      }
    }

    private class WeakBitmapSourceEventSink : WeakReference {
      private BitmapSource _eventSource;
      public BitmapSource EventSource {
        get {
          return _eventSource;
        }
        set {
          if (_eventSource != null) {
            DetachSourceDownloadHandlers(_eventSource);
          }
          _eventSource = value;
          if (_eventSource != null) {
            AttachSourceDownloadHandlers();
          }
        }
      }

      public WeakBitmapSourceEventSink(BitmapSource bitmapSource)
        : base(bitmapSource) {
      }

      public void OnSourceDownloadCompleted(object sender, EventArgs e) {
        BitmapSource bitmapSource = Target as BitmapSource;
        if (bitmapSource != null) {
          bitmapSource.OnSourceDownloadCompleted(bitmapSource, e);
        } else {
          DetachSourceDownloadHandlers(EventSource);
        }
      }

      public void OnSourceDownloadFailed(object sender, ExceptionEventArgs e) {
        BitmapSource bitmapSource = Target as BitmapSource;
        if (bitmapSource != null) {
          bitmapSource.OnSourceDownloadFailed(bitmapSource, e);
        } else {
          DetachSourceDownloadHandlers(EventSource);
        }
      }

      public void OnSourceDownloadProgress(object sender, DownloadProgressEventArgs e) {
        BitmapSource bitmapSource = Target as BitmapSource;
        if (bitmapSource != null) {
          bitmapSource.OnSourceDownloadProgress(bitmapSource, e);
        } else {
          DetachSourceDownloadHandlers(EventSource);
        }
      }

      public void DetachSourceDownloadHandlers(BitmapSource source) {
        if (!source.IsFrozen) {
          source.DownloadCompleted -= OnSourceDownloadCompleted;
          source.DownloadFailed -= OnSourceDownloadFailed;
          source.DownloadProgress -= OnSourceDownloadProgress;
        }
      }

      public void AttachSourceDownloadHandlers() {
        if (!_eventSource.IsFrozen) {
          _eventSource.DownloadCompleted += OnSourceDownloadCompleted;
          _eventSource.DownloadFailed += OnSourceDownloadFailed;
          _eventSource.DownloadProgress += OnSourceDownloadProgress;
        }
      }
    }

    private bool _delayCreation;
    private bool _creationComplete;
    private bool _useVirtuals;
    internal BitmapInitialize _bitmapInit = new BitmapInitialize();
    [SecurityCritical] internal BitmapSourceSafeMILHandle _wicSource;
    [SecurityCritical] internal BitmapSourceSafeMILHandle _convertedDUCEPtr;
    internal object _syncObject;
    internal bool _isSourceCached;
    internal bool _needsUpdate;
    internal bool _isColorCorrected;
    internal UniqueEventHelper _downloadEvent = new UniqueEventHelper();
    internal UniqueEventHelper<DownloadProgressEventArgs> _progressEvent = new UniqueEventHelper<DownloadProgressEventArgs>();
    internal UniqueEventHelper<ExceptionEventArgs> _failedEvent = new UniqueEventHelper<ExceptionEventArgs>();
    internal UniqueEventHelper<ExceptionEventArgs> _decodeFailedEvent = new UniqueEventHelper<ExceptionEventArgs>();
    internal PixelFormat _format = PixelFormats.Default;
    internal int _pixelWidth;
    internal int _pixelHeight;
    internal double _dpiX = 96.0;
    internal double _dpiY = 96.0;
    internal BitmapPalette _palette;
    internal DUCE.MultiChannelResource _duceResource;

    private static readonly PixelFormat[] s_supportedDUCEFormats = new PixelFormat[13] {
    PixelFormats.Indexed1,
    PixelFormats.BlackWhite,
    PixelFormats.Indexed2,
    PixelFormats.Gray2,
    PixelFormats.Indexed4,
    PixelFormats.Gray4,
    PixelFormats.Indexed8,
    PixelFormats.Gray8,
    PixelFormats.Bgr555,
    PixelFormats.Bgr565,
    PixelFormats.Bgr32,
    PixelFormats.Bgra32,
    PixelFormats.Pbgra32
    };

    private WeakBitmapSourceEventSink _weakBitmapSourceEventSink;

    /// <summary>Gets the native <see cref="T:System.Windows.Media.PixelFormat" /> of the bitmap data. </summary>
    /// <returns>The pixel format of the bitmap data.</returns>
    public virtual PixelFormat Format {
      get {
        ReadPreamble();
        EnsureShouldUseVirtuals();
        _bitmapInit.EnsureInitializedComplete();
        CompleteDelayedCreation();
        return _format;
      }
    }

    /// <summary>Gets the width of the bitmap in pixels. </summary>
    /// <returns>The width of the bitmap in pixels.</returns>
    public virtual int PixelWidth {
      get {
        ReadPreamble();
        EnsureShouldUseVirtuals();
        _bitmapInit.EnsureInitializedComplete();
        CompleteDelayedCreation();
        return _pixelWidth;
      }
    }

    /// <summary>Gets the height of the bitmap in pixels. </summary>
    /// <returns>The height of the bitmap in pixels.</returns>
    public virtual int PixelHeight {
      get {
        ReadPreamble();
        EnsureShouldUseVirtuals();
        _bitmapInit.EnsureInitializedComplete();
        CompleteDelayedCreation();
        return _pixelHeight;
      }
    }

    /// <summary>Gets the horizontal dots per inch (dpi) of the image. </summary>
    /// <returns>The horizontal dots per inch (dpi) of the image; that is, the dots per inch (dpi) along the x-axis.</returns>
    public virtual double DpiX {
      get {
        ReadPreamble();
        EnsureShouldUseVirtuals();
        _bitmapInit.EnsureInitializedComplete();
        CompleteDelayedCreation();
        return _dpiX;
      }
    }

    /// <summary>Gets the vertical dots per inch (dpi) of the image. </summary>
    /// <returns>The vertical dots per inch (dpi) of the image; that is, the dots per inch (dpi) along the y-axis.</returns>
    public virtual double DpiY {
      get {
        ReadPreamble();
        EnsureShouldUseVirtuals();
        _bitmapInit.EnsureInitializedComplete();
        CompleteDelayedCreation();
        return _dpiY;
      }
    }

    /// <summary>Gets the color palette of the bitmap, if one is specified. </summary>
    /// <returns>The color palette of the bitmap.</returns>
    public virtual BitmapPalette Palette {
      get {
        ReadPreamble();
        EnsureShouldUseVirtuals();
        _bitmapInit.EnsureInitializedComplete();
        CompleteDelayedCreation();
        if (_palette == null && _format.Palettized) {
          _palette = BitmapPalette.CreateFromBitmapSource(this);
        }
        return _palette;
      }
    }

    /// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> content is currently downloading. </summary>
    /// <returns>true if the bitmap source is currently downloading; otherwise, false.</returns>
    public virtual bool IsDownloading {
      get {
        ReadPreamble();
        return false;
      }
    }

    /// <summary>Gets the width of the bitmap in device-independent units (1/96th inch per unit). </summary>
    /// <returns>The width of the bitmap in device-independent units (1/96th inch per unit).</returns>
    public override double Width {
      get {
        ReadPreamble();
        return GetWidthInternal();
      }
    }

    /// <summary>Gets the height of the source bitmap in device-independent units (1/96th inch per unit). </summary>
    /// <returns>The height of the bitmap in device-independent units (1/96th inch per unit).</returns>
    public override double Height {
      get {
        ReadPreamble();
        return GetHeightInternal();
      }
    }

    /// <summary>Gets the metadata that is associated with this bitmap image. </summary>
    /// <returns>The metadata that is associated with the bitmap image.</returns>
    public override ImageMetadata Metadata {
      get {
        ReadPreamble();
        return null;
      }
    }

    internal override Size Size {
      get {
        ReadPreamble();
        return new Size(Math.Max(0.0, GetWidthInternal()), Math.Max(0.0, GetHeightInternal()));
      }
    }

    internal bool DelayCreation {
      get {
        return _delayCreation;
      }
      set {
        _delayCreation = value;
        if (_delayCreation) {
          CreationCompleted = false;
        }
      }
    }

    internal bool CreationCompleted {
      get {
        return _creationComplete;
      }
      set {
        _creationComplete = value;
      }
    }

    internal object SyncObject => _syncObject;

    internal bool IsSourceCached {
      get {
        return _isSourceCached;
      }
      set {
        _isSourceCached = value;
      }
    }

    internal BitmapSourceSafeMILHandle WicSourceHandle {
      [SecurityCritical]
      get {
        CompleteDelayedCreation();
        if (_wicSource == null || _wicSource.IsInvalid) {
          ManagedBitmapSource o = new ManagedBitmapSource(this);
          _wicSource = new BitmapSourceSafeMILHandle(Marshal.GetComInterfaceForObject(o, typeof(IWICBitmapSource)));
        }
        return _wicSource;
      }
      [SecurityTreatAsSafe]
      [SecurityCritical]
      set {
        if (value != null) {
          IntPtr ppvObject = IntPtr.Zero;
          Guid guid = MILGuidData.IID_IWICBitmapSource;
          HRESULT.Check(UnsafeNativeMethods.MILUnknown.QueryInterface(value, ref guid, out ppvObject));
          _wicSource = new BitmapSourceSafeMILHandle(ppvObject, value);
          UpdateCachedSettings();
        } else {
          _wicSource = null;
        }
      }
    }

    internal virtual unsafe BitmapSourceSafeMILHandle DUCECompatiblePtr {
      [SecurityCritical]
      get {
        BitmapSourceSafeMILHandle ppIBitmap = WicSourceHandle;
        BitmapSourceSafeMILHandle pCWICWrapperBitmap = null;
        if (_convertedDUCEPtr == null || _convertedDUCEPtr.IsInvalid) {
          if (UsableWithoutCache) {
            Int32Rect prc = new Int32Rect(0, 0, 1, 1);
            int num = (Format.BitsPerPixel + 7) / 8;
            byte[] array = new byte[num];
            try {
              try {
                fixed (IntPtr* value = (IntPtr*)(&array[0])) {
                  HRESULT.Check(UnsafeNativeMethods.WICBitmapSource.CopyPixels(ppIBitmap, ref prc, (uint)num, (uint)num, (void*)value));
                }
              } finally {
              }
            } catch (Exception e) {
              RecoverFromDecodeFailure(e);
              ppIBitmap = WicSourceHandle;
            }
          } else {
            BitmapSourceSafeMILHandle ppFormatConverter = null;
            using (FactoryMaker factoryMaker = new FactoryMaker()) {
              try {
                if (!HasCompatibleFormat) {
                  Guid dstFormat = GetClosestDUCEFormat(Format, Palette).Guid;
                  HRESULT.Check(UnsafeNativeMethods.WICImagingFactory.CreateFormatConverter(factoryMaker.ImagingFactoryPtr, out ppFormatConverter));
                  HRESULT.Check(UnsafeNativeMethods.WICFormatConverter.Initialize(ppFormatConverter, ppIBitmap, ref dstFormat, DitherType.DitherTypeNone, new SafeMILHandle(IntPtr.Zero), 0.0, WICPaletteType.WICPaletteTypeCustom));
                  ppIBitmap = ppFormatConverter;
                }
                try {
                  HRESULT.Check(UnsafeNativeMethods.WICImagingFactory.CreateBitmapFromSource(factoryMaker.ImagingFactoryPtr, ppIBitmap, WICBitmapCreateCacheOptions.WICBitmapCacheOnLoad, out ppIBitmap));
                } catch (Exception e2) {
                  RecoverFromDecodeFailure(e2);
                  ppIBitmap = WicSourceHandle;
                }
                _isSourceCached = true;
              } finally {
                ppFormatConverter?.Close();
              }
              goto end_IL_009c;
            IL_0134:
            end_IL_009c:;
            }
          }
          HRESULT.Check(UnsafeNativeMethods.MilCoreApi.CreateCWICWrapperBitmap(ppIBitmap, out pCWICWrapperBitmap));
          UnsafeNativeMethods.MILUnknown.AddRef(pCWICWrapperBitmap);
          _convertedDUCEPtr = new BitmapSourceSafeMILHandle(pCWICWrapperBitmap.DangerousGetHandle(), ppIBitmap);
        }
        return _convertedDUCEPtr;
      }
    }

    internal virtual bool ShouldCloneEventDelegates => true;

    internal bool UsableWithoutCache {
      get {
        if (HasCompatibleFormat) {
          return _isSourceCached;
        }
        return false;
      }
    }

    internal bool HasCompatibleFormat => IsCompatibleFormat(Format);

    /// <summary>Occurs when the bitmap content has been completely downloaded.</summary>
    public virtual event EventHandler DownloadCompleted {
      add {
        WritePreamble();
        _downloadEvent.AddEvent(value);
      }
      remove {
        WritePreamble();
        _downloadEvent.RemoveEvent(value);
      }
    }

    /// <summary>Occurs when the download progress of the bitmap content has changed.</summary>
    public virtual event EventHandler<DownloadProgressEventArgs> DownloadProgress {
      add {
        WritePreamble();
        _progressEvent.AddEvent(value);
      }
      remove {
        WritePreamble();
        _progressEvent.RemoveEvent(value);
      }
    }

    /// <summary>Occurs when the bitmap content failed to download.</summary>
    public virtual event EventHandler<ExceptionEventArgs> DownloadFailed {
      add {
        WritePreamble();
        _failedEvent.AddEvent(value);
      }
      remove {
        WritePreamble();
        _failedEvent.RemoveEvent(value);
      }
    }

    /// <summary>Occurs when the image fails to load, due to a corrupt image header.</summary>
    public virtual event EventHandler<ExceptionEventArgs> DecodeFailed {
      add {
        WritePreamble();
        EnsureShouldUseVirtuals();
        _decodeFailedEvent.AddEvent(value);
      }
      remove {
        WritePreamble();
        EnsureShouldUseVirtuals();
        _decodeFailedEvent.RemoveEvent(value);
      }
    }

    /// <summary>Creates a new <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> from an array of pixels.</summary>
    /// <returns>The <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> that is created from the specified array of pixels.</returns>
    /// <param name="pixelWidth">The width of the bitmap.</param>
    /// <param name="pixelHeight">The height of the bitmap.</param>
    /// <param name="dpiX">The horizontal dots per inch (dpi) of the bitmap.</param>
    /// <param name="dpiY">The vertical dots per inch (dpi) of the bitmap.</param>
    /// <param name="pixelFormat">The pixel format of the bitmap.</param>
    /// <param name="palette">The palette of the bitmap.</param>
    /// <param name="pixels">An array of bytes that represents the content of a bitmap image.</param>
    /// <param name="stride">The stride of the bitmap.</param>
    public static BitmapSource Create(int pixelWidth, int pixelHeight, double dpiX, double dpiY, PixelFormat pixelFormat, BitmapPalette palette, Array pixels, int stride) {
      return new CachedBitmap(pixelWidth, pixelHeight, dpiX, dpiY, pixelFormat, palette, pixels, stride);
    }

    /// <summary>Creates a new <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> from an array of pixels that are stored in unmanaged memory.</summary>
    /// <returns>A <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> that is created from the array of pixels in unmanaged memory.</returns>
    /// <param name="pixelWidth">The width of the bitmap.</param>
    /// <param name="pixelHeight">The height of the bitmap.</param>
    /// <param name="dpiX">The horizontal dots per inch (dpi) of the bitmap.</param>
    /// <param name="dpiY">The vertical dots per inch (dpi) of the bitmap.</param>
    /// <param name="pixelFormat">The pixel format of the bitmap.</param>
    /// <param name="palette">The palette of the bitmap.</param>
    /// <param name="buffer">A pointer to the buffer that contains the bitmap data in memory.</param>
    /// <param name="bufferSize">The size of the buffer.</param>
    /// <param name="stride">The stride of the bitmap.</param>
    [SecurityCritical]
    public static BitmapSource Create(int pixelWidth, int pixelHeight, double dpiX, double dpiY, PixelFormat pixelFormat, BitmapPalette palette, IntPtr buffer, int bufferSize, int stride) {
      SecurityHelper.DemandUnmanagedCode();
      return new CachedBitmap(pixelWidth, pixelHeight, dpiX, dpiY, pixelFormat, palette, buffer, bufferSize, stride);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> class. </summary>
    [SecurityTreatAsSafe]
    [SecurityCritical]
    protected BitmapSource() {
      _syncObject = _bitmapInit;
      _isSourceCached = false;
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    internal BitmapSource(bool useVirtuals) {
      _useVirtuals = true;
      _isSourceCached = false;
      _syncObject = _bitmapInit;
    }

    /// <summary>Creates a modifiable clone of this <see cref="T:System.Windows.Media.Imaging.BitmapSource" />, making deep copies of this object's values. When copying dependency properties, this method copies resource references and data bindings (but they might no longer resolve) but not animations or their current values.</summary>
    /// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property will be false even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property was true.</returns>
    public new BitmapSource Clone() {
      return (BitmapSource)base.Clone();
    }

    /// <summary>Creates a modifiable clone of this <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> object, making deep copies of this object's current values. Resource references, data bindings, and animations are not copied, but their current values are. </summary>
    /// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property will be false even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property was true.</returns>
    public new BitmapSource CloneCurrentValue() {
      return (BitmapSource)base.CloneCurrentValue();
    }

    /// <summary>Copies the bitmap pixel data within the specified rectangle into an array of pixels that has the specified stride starting at the specified offset.</summary>
    /// <param name="sourceRect">The source rectangle to copy. An <see cref="P:System.Windows.Int32Rect.Empty" /> value specifies the entire bitmap.</param>
    /// <param name="pixels">The destination array.</param>
    /// <param name="stride">The stride of the bitmap.</param>
    /// <param name="offset">The pixel location where copying begins.</param>
    [SecurityCritical]
    [SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public virtual void CopyPixels(Int32Rect sourceRect, Array pixels, int stride, int offset) {
      EnsureShouldUseVirtuals();
      CheckIfSiteOfOrigin();
      CriticalCopyPixels(sourceRect, pixels, stride, offset);
    }

    /// <summary>Copies the bitmap pixel data into an array of pixels with the specified stride, starting at the specified offset.</summary>
    /// <param name="pixels">The destination array.</param>
    /// <param name="stride">The stride of the bitmap.</param>
    /// <param name="offset">The pixel location where copying starts.</param>
    [SecurityCritical]
    [SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public virtual void CopyPixels(Array pixels, int stride, int offset) {
      Int32Rect empty = Int32Rect.Empty;
      EnsureShouldUseVirtuals();
      CheckIfSiteOfOrigin();
      CopyPixels(empty, pixels, stride, offset);
    }

    /// <summary>Copies the bitmap pixel data within the specified rectangle </summary>
    /// <param name="sourceRect">The source rectangle to copy. An <see cref="P:System.Windows.Int32Rect.Empty" /> value specifies the entire bitmap.</param>
    /// <param name="buffer">A pointer to the buffer.</param>
    /// <param name="bufferSize">The size of the buffer.</param>
    /// <param name="stride">The stride of the bitmap.</param>
    [SecurityCritical]
    [SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public virtual void CopyPixels(Int32Rect sourceRect, IntPtr buffer, int bufferSize, int stride) {
      ReadPreamble();
      EnsureShouldUseVirtuals();
      _bitmapInit.EnsureInitializedComplete();
      CompleteDelayedCreation();
      CheckIfSiteOfOrigin();
      CriticalCopyPixels(sourceRect, buffer, bufferSize, stride);
    }

    private double GetWidthInternal() {
      return ImageSource.PixelsToDIPs(DpiX, PixelWidth);
    }

    private double GetHeightInternal() {
      return ImageSource.PixelsToDIPs(DpiY, PixelHeight);
    }

    internal void CompleteDelayedCreation() {
      if (DelayCreation) {
        lock (_syncObject) {
          if (DelayCreation) {
            EnsureShouldUseVirtuals();
            DelayCreation = false;
            try {
              FinalizeCreation();
            } catch {
              DelayCreation = true;
              throw;
            IL_0040:;
            }
            CreationCompleted = true;
          }
        }
      }
    }

    internal virtual void FinalizeCreation() {
      throw new NotImplementedException();
    }

    private void EnsureShouldUseVirtuals() {
      if (!_useVirtuals) {
        throw new NotImplementedException();
      }
    }

    [SecurityCritical]
    internal virtual void UpdateCachedSettings() {
      EnsureShouldUseVirtuals();
      uint puiWidth;
      uint puiHeight;
      lock (_syncObject) {
        _format = PixelFormat.GetPixelFormat(_wicSource);
        HRESULT.Check(UnsafeNativeMethods.WICBitmapSource.GetSize(_wicSource, out puiWidth, out puiHeight));
        HRESULT.Check(UnsafeNativeMethods.WICBitmapSource.GetResolution(_wicSource, out _dpiX, out _dpiY));
      }
      _pixelWidth = (int)puiWidth;
      _pixelHeight = (int)puiHeight;
    }

    [FriendAccessAllowed]
    [SecurityCritical]
    internal unsafe void CriticalCopyPixels(Int32Rect sourceRect, Array pixels, int stride, int offset) {
      ReadPreamble();
      _bitmapInit.EnsureInitializedComplete();
      CompleteDelayedCreation();
      if (pixels == null) {
        throw new ArgumentNullException("pixels");
      }
      if (pixels.Rank != 1) {
        throw new ArgumentException(SR.Get("Collection_BadRank"), "pixels");
      }
      if (offset < 0) {
        HRESULT.Check(-2147024362);
      }
      int num = -1;
      if (pixels is byte[]) {
        num = 1;
      } else if (pixels is short[] || pixels is ushort[]) {
        num = 2;
      } else if (pixels is int[] || pixels is uint[] || pixels is float[]) {
        num = 4;
      } else if (pixels is double[]) {
        num = 8;
      }
      if (num == -1) {
        throw new ArgumentException(SR.Get("Image_InvalidArrayForPixel"));
      }
      int bufferSize = checked(num * (pixels.Length - offset));
      if (pixels is byte[]) {
        fixed (IntPtr* value = (IntPtr*)(&((byte[])pixels)[offset])) {
          CriticalCopyPixels(sourceRect, (IntPtr)(void*)value, bufferSize, stride);
        }
      } else if (pixels is short[]) {
        fixed (IntPtr* value2 = (IntPtr*)(&((short[])pixels)[offset])) {
          CriticalCopyPixels(sourceRect, (IntPtr)(void*)value2, bufferSize, stride);
        }
      } else if (pixels is ushort[]) {
        fixed (IntPtr* value3 = (IntPtr*)(&((ushort[])pixels)[offset])) {
          CriticalCopyPixels(sourceRect, (IntPtr)(void*)value3, bufferSize, stride);
        }
      } else if (pixels is int[]) {
        fixed (IntPtr* value4 = (IntPtr*)(&((int[])pixels)[offset])) {
          CriticalCopyPixels(sourceRect, (IntPtr)(void*)value4, bufferSize, stride);
        }
      } else if (pixels is uint[]) {
        fixed (IntPtr* value5 = (IntPtr*)(&((uint[])pixels)[offset])) {
          CriticalCopyPixels(sourceRect, (IntPtr)(void*)value5, bufferSize, stride);
        }
      } else if (pixels is float[]) {
        fixed (IntPtr* value6 = (IntPtr*)(&((float[])pixels)[offset])) {
          CriticalCopyPixels(sourceRect, (IntPtr)(void*)value6, bufferSize, stride);
        }
      } else if (pixels is double[]) {
        fixed (IntPtr* value7 = (IntPtr*)(&((double[])pixels)[offset])) {
          CriticalCopyPixels(sourceRect, (IntPtr)(void*)value7, bufferSize, stride);
        }
      }
    }

    [SecurityCritical]
    internal void CriticalCopyPixels(Int32Rect sourceRect, IntPtr buffer, int bufferSize, int stride) {
      if (buffer == IntPtr.Zero) {
        throw new ArgumentNullException("buffer");
      }
      if (stride <= 0) {
        throw new ArgumentOutOfRangeException("stride", SR.Get("ParameterMustBeGreaterThanZero"));
      }
      if (sourceRect.Width <= 0) {
        sourceRect.Width = PixelWidth;
      }
      if (sourceRect.Height <= 0) {
        sourceRect.Height = PixelHeight;
      }
      if (sourceRect.Width > PixelWidth) {
        throw new ArgumentOutOfRangeException("sourceRect.Width", SR.Get("ParameterCannotBeGreaterThan", PixelWidth));
      }
      if (sourceRect.Height > PixelHeight) {
        throw new ArgumentOutOfRangeException("sourceRect.Height", SR.Get("ParameterCannotBeGreaterThan", PixelHeight));
      }
      int num = checked(sourceRect.Width * Format.BitsPerPixel + 7) / 8;
      if (stride < num) {
        throw new ArgumentOutOfRangeException("stride", SR.Get("ParameterCannotBeLessThan", num));
      }
      int num2 = checked(stride * (sourceRect.Height - 1) + num);
      if (bufferSize < num2) {
        throw new ArgumentOutOfRangeException("buffer", SR.Get("ParameterCannotBeLessThan", num2));
      }
      lock (_syncObject) {
        HRESULT.Check(UnsafeNativeMethods.WICBitmapSource.CopyPixels(WicSourceHandle, ref sourceRect, (uint)stride, (uint)bufferSize, buffer));
      }
    }

    /// <summary>Checks whether the bitmap source content is from a known site of origin. This method is used to make sure that pixel copying operations are safe. </summary>
    [SecurityTreatAsSafe]
    [SecurityCritical]
    protected void CheckIfSiteOfOrigin() {
      string uri = null;
      if (CanSerializeToString()) {
        uri = ConvertToString(null, null);
      }
      SecurityHelper.DemandMediaAccessPermission(uri);
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    internal override void UpdateResource(DUCE.Channel channel, bool skipOnChannelCheck) {
      base.UpdateResource(channel, skipOnChannelCheck);
      UpdateBitmapSourceResource(channel, skipOnChannelCheck);
    }

    internal override DUCE.ResourceHandle AddRefOnChannelCore(DUCE.Channel channel) {
      if (_duceResource.CreateOrAddRefOnChannel(this, channel, DUCE.ResourceType.TYPE_BITMAPSOURCE)) {
        UpdateResource(channel, true);
      }
      return _duceResource.GetHandle(channel);
    }

    DUCE.ResourceHandle DUCE.IResource.AddRefOnChannel(DUCE.Channel channel) {
      using (CompositionEngineLock.Acquire()) {
        return AddRefOnChannelCore(channel);
      }
    }

    internal override int GetChannelCountCore() {
      return _duceResource.GetChannelCount();
    }

    int DUCE.IResource.GetChannelCount() {
      return GetChannelCountCore();
    }

    internal override DUCE.Channel GetChannelCore(int index) {
      return _duceResource.GetChannel(index);
    }

    DUCE.Channel DUCE.IResource.GetChannel(int index) {
      return GetChannelCore(index);
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    internal virtual void UpdateBitmapSourceResource(DUCE.Channel channel, bool skipOnChannelCheck) {
      if (_needsUpdate) {
        _convertedDUCEPtr = null;
        _needsUpdate = false;
      }
      if (skipOnChannelCheck || _duceResource.IsOnChannel(channel)) {
        lock (_syncObject) {
          channel.SendCommandBitmapSource(_duceResource.GetHandle(channel), DUCECompatiblePtr);
        }
      }
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    internal void RecoverFromDecodeFailure(Exception e) {
      byte[] pixels = new byte[4];
      WicSourceHandle = Create(1, 1, 96.0, 96.0, PixelFormats.Pbgra32, null, pixels, 4).WicSourceHandle;
      IsSourceCached = true;
      OnDecodeFailed(this, new ExceptionEventArgs(e));
    }

    internal override void ReleaseOnChannelCore(DUCE.Channel channel) {
      _duceResource.ReleaseOnChannel(channel);
    }

    void DUCE.IResource.ReleaseOnChannel(DUCE.Channel channel) {
      using (CompositionEngineLock.Acquire()) {
        ReleaseOnChannelCore(channel);
      }
    }

    internal override DUCE.ResourceHandle GetHandleCore(DUCE.Channel channel) {
      return _duceResource.GetHandle(channel);
    }

    DUCE.ResourceHandle DUCE.IResource.GetHandle(DUCE.Channel channel) {
      using (CompositionEngineLock.Acquire()) {
        return GetHandleCore(channel);
      }
    }

    internal static PixelFormat GetClosestDUCEFormat(PixelFormat format, BitmapPalette palette) {
      int num = Array.IndexOf(s_supportedDUCEFormats, format);
      if (num != -1) {
        return s_supportedDUCEFormats[num];
      }
      int internalBitsPerPixel = format.InternalBitsPerPixel;
      if (internalBitsPerPixel == 1) {
        return PixelFormats.Indexed1;
      }
      if (internalBitsPerPixel == 2) {
        return PixelFormats.Indexed2;
      }
      if (internalBitsPerPixel <= 4) {
        return PixelFormats.Indexed4;
      }
      if (internalBitsPerPixel <= 8) {
        return PixelFormats.Indexed8;
      }
      if (internalBitsPerPixel <= 16 && format.Format != PixelFormatEnum.Gray16) {
        return PixelFormats.Bgr555;
      }
      if (format.HasAlpha || BitmapPalette.DoesPaletteHaveAlpha(palette)) {
        return PixelFormats.Pbgra32;
      }
      return PixelFormats.Bgr32;
    }

    [SecurityCritical]
    internal static BitmapSourceSafeMILHandle CreateCachedBitmap(BitmapFrame frame, BitmapSourceSafeMILHandle wicSource, BitmapCreateOptions createOptions, BitmapCacheOption cacheOption, BitmapPalette palette) {
      BitmapSourceSafeMILHandle ppIDst = null;
      BitmapSourceSafeMILHandle ppIBitmap = null;
      if (cacheOption == BitmapCacheOption.None) {
        return wicSource;
      }
      using (FactoryMaker factoryMaker = new FactoryMaker()) {
        IntPtr imagingFactoryPtr = factoryMaker.ImagingFactoryPtr;
        bool flag = false;
        PixelFormat pbgra = PixelFormats.Pbgra32;
        WICBitmapCreateCacheOptions options = WICBitmapCreateCacheOptions.WICBitmapCacheOnLoad;
        if (cacheOption == BitmapCacheOption.Default) {
          options = WICBitmapCreateCacheOptions.WICBitmapCacheOnDemand;
        }
        pbgra = PixelFormat.GetPixelFormat(wicSource);
        PixelFormat pixelFormat = pbgra;
        if ((createOptions & BitmapCreateOptions.PreservePixelFormat) == BitmapCreateOptions.None) {
          if (!IsCompatibleFormat(pbgra)) {
            flag = true;
          }
          pixelFormat = GetClosestDUCEFormat(pbgra, palette);
        }
        if (frame != null && (createOptions & BitmapCreateOptions.IgnoreColorProfile) == BitmapCreateOptions.None && frame.ColorContexts != null && frame.ColorContexts[0] != (ColorContext)null && frame.ColorContexts[0].IsValid && !frame._isColorCorrected && PixelFormat.GetPixelFormat(wicSource).Format != 0) {
          ColorContext colorContext;
          try {
            colorContext = new ColorContext(pixelFormat);
          } catch (NotSupportedException) {
            colorContext = null;
          }
          if (colorContext != (ColorContext)null) {
            bool flag2 = false;
            bool flag3 = false;
            try {
              ColorConvertedBitmap colorConvertedBitmap = new ColorConvertedBitmap(frame, frame.ColorContexts[0], colorContext, pixelFormat);
              wicSource = colorConvertedBitmap.WicSourceHandle;
              frame._isColorCorrected = true;
              flag2 = true;
              flag = false;
            } catch (NotSupportedException) {
            } catch (FileFormatException) {
              flag3 = true;
            }
            if ((!flag2 & flag) && !flag3) {
              flag = false;
              FormatConvertedBitmap source = new FormatConvertedBitmap(frame, pixelFormat, null, 0.0);
              ColorConvertedBitmap colorConvertedBitmap2 = new ColorConvertedBitmap(source, frame.ColorContexts[0], colorContext, pixelFormat);
              wicSource = colorConvertedBitmap2.WicSourceHandle;
              frame._isColorCorrected = true;
              flag = false;
            }
          }
        }
        if (flag) {
          Guid dstPixelFormatGuid = pixelFormat.Guid;
          HRESULT.Check(UnsafeNativeMethods.WICCodec.WICConvertBitmapSource(ref dstPixelFormatGuid, wicSource, out ppIDst));
          HRESULT.Check(UnsafeNativeMethods.WICImagingFactory.CreateBitmapFromSource(imagingFactoryPtr, ppIDst, options, out ppIBitmap));
        } else {
          HRESULT.Check(UnsafeNativeMethods.WICImagingFactory.CreateBitmapFromSource(imagingFactoryPtr, wicSource, options, out ppIBitmap));
        }
        ppIBitmap.CalculateSize();
        return ppIBitmap;
      }
    }

    private void OnDecodeFailed(object sender, ExceptionEventArgs e) {
      _decodeFailedEvent.InvokeEvents(this, e);
    }

    private void OnSourceDownloadCompleted(object sender, EventArgs e) {
      if (_weakBitmapSourceEventSink != null) {
        CleanUpWeakEventSink();
        if (_bitmapInit.IsInitAtLeastOnce && IsValidForFinalizeCreation(false)) {
          try {
            FinalizeCreation();
            _needsUpdate = true;
          } catch {
          }
          _downloadEvent.InvokeEvents(this, e);
        }
      }
    }

    private void OnSourceDownloadFailed(object sender, ExceptionEventArgs e) {
      if (_weakBitmapSourceEventSink != null) {
        CleanUpWeakEventSink();
        _failedEvent.InvokeEvents(this, e);
      }
    }

    private void OnSourceDownloadProgress(object sender, DownloadProgressEventArgs e) {
      _progressEvent.InvokeEvents(this, e);
    }

    private void CleanUpWeakEventSink() {
      _weakBitmapSourceEventSink.EventSource = null;
      _weakBitmapSourceEventSink = null;
    }

    internal void RegisterDownloadEventSource(BitmapSource eventSource) {
      if (_weakBitmapSourceEventSink == null) {
        _weakBitmapSourceEventSink = new WeakBitmapSourceEventSink(this);
      }
      _weakBitmapSourceEventSink.EventSource = eventSource;
    }

    internal void UnregisterDownloadEventSource() {
      if (_weakBitmapSourceEventSink != null) {
        CleanUpWeakEventSink();
      }
    }

    internal virtual bool IsValidForFinalizeCreation(bool throwIfInvalid) {
      return true;
    }

    /// <summary>Makes an instance of <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> or a derived class immutable.</summary>
    /// <returns>If <paramref name="isChecking" /> is true, this method returns true if this <see cref="T:System.Windows.Media.Animation.Animatable" /> can be made unmodifiable, or false if it cannot be made unmodifiable. If <paramref name="isChecking" /> is false, this method returns true if the if this <see cref="T:System.Windows.Media.Animation.Animatable" /> is now unmodifiable, or false if it cannot be made unmodifiable, with the side effect of having begun to change the frozen status of this object.</returns>
    /// <param name="isChecking">true if this instance should actually freeze itself when this method is called; otherwise, false.</param>
    protected override bool FreezeCore(bool isChecking) {
      if (!base.FreezeCore(isChecking)) {
        return false;
      }
      if (IsDownloading) {
        return false;
      }
      return true;
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    private void CopyCommon(BitmapSource sourceBitmap) {
      _useVirtuals = sourceBitmap._useVirtuals;
      _delayCreation = sourceBitmap.DelayCreation;
      _creationComplete = sourceBitmap.CreationCompleted;
      WicSourceHandle = sourceBitmap.WicSourceHandle;
      _syncObject = sourceBitmap.SyncObject;
      IsSourceCached = sourceBitmap.IsSourceCached;
      if (ShouldCloneEventDelegates) {
        if (sourceBitmap._downloadEvent != null) {
          _downloadEvent = sourceBitmap._downloadEvent.Clone();
        }
        if (sourceBitmap._progressEvent != null) {
          _progressEvent = sourceBitmap._progressEvent.Clone();
        }
        if (sourceBitmap._failedEvent != null) {
          _failedEvent = sourceBitmap._failedEvent.Clone();
        }
        if (sourceBitmap._decodeFailedEvent != null) {
          _decodeFailedEvent = sourceBitmap._decodeFailedEvent.Clone();
        }
      }
      _format = sourceBitmap.Format;
      _pixelWidth = sourceBitmap.PixelWidth;
      _pixelHeight = sourceBitmap.PixelHeight;
      _dpiX = sourceBitmap.DpiX;
      _dpiY = sourceBitmap.DpiY;
      _palette = sourceBitmap.Palette;
      if (_weakBitmapSourceEventSink != null && sourceBitmap._weakBitmapSourceEventSink != null) {
        sourceBitmap._weakBitmapSourceEventSink.DetachSourceDownloadHandlers(_weakBitmapSourceEventSink.EventSource);
      }
    }

    /// <summary>Makes this instance a deep copy of the specified <see cref="T:System.Windows.Media.Imaging.BitmapSource" />. When copying dependency properties, this method copies resource references and data bindings (but they might no longer resolve) but not animations or their current values.</summary>
    /// <param name="sourceFreezable">The <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> to clone..</param>
    protected override void CloneCore(Freezable sourceFreezable) {
      BitmapSource sourceBitmap = (BitmapSource)sourceFreezable;
      base.CloneCore(sourceFreezable);
      CopyCommon(sourceBitmap);
    }

    /// <summary>Makes this instance a modifiable deep copy of the specified <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> using current property values. Resource references, data bindings, and animations are not copied, but their current values are.</summary>
    /// <param name="sourceFreezable">The <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> to clone.</param>
    protected override void CloneCurrentValueCore(Freezable sourceFreezable) {
      BitmapSource sourceBitmap = (BitmapSource)sourceFreezable;
      base.CloneCurrentValueCore(sourceFreezable);
      CopyCommon(sourceBitmap);
    }

    /// <summary>Makes this instance a clone of the specified <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> object. </summary>
    /// <param name="sourceFreezable">The <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> object to clone and freeze.</param>
    protected override void GetAsFrozenCore(Freezable sourceFreezable) {
      BitmapSource sourceBitmap = (BitmapSource)sourceFreezable;
      base.GetAsFrozenCore(sourceFreezable);
      CopyCommon(sourceBitmap);
    }

    /// <summary>Makes this instance a frozen clone of the specified <see cref="T:System.Windows.Media.Imaging.BitmapSource" />. Resource references, data bindings, and animations are not copied, but their current values are.</summary>
    /// <param name="sourceFreezable">The <see cref="T:System.Windows.Media.Imaging.BitmapSource" /> to copy and freeze.</param>
    protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable) {
      BitmapSource sourceBitmap = (BitmapSource)sourceFreezable;
      base.GetCurrentValueAsFrozenCore(sourceFreezable);
      CopyCommon(sourceBitmap);
    }

    internal static bool IsCompatibleFormat(PixelFormat format) {
      return Array.IndexOf(s_supportedDUCEFormats, format) != -1;
    }
  }

}
