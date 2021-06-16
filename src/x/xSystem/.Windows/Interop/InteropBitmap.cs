using Microsoft.Internal;
using Microsoft.Win32;
using System.Drawing.Imaging;
using System.Security;
using System.Windows.Media.Imaging;

namespace System.Windows.Interop {
  public sealed class InteropBitmap : BitmapSource {
    [SecurityCritical]
    private BitmapSourceSafeMILHandle _unmanagedSource;

    private Int32Rect _sourceRect = Int32Rect.Empty;

    private BitmapSizeOptions _sizeOptions;

    [SecurityTreatAsSafe]
    [SecurityCritical]
    private InteropBitmap()
      : base(true) {
      SecurityHelper.DemandUnmanagedCode();
    }

    [SecurityCritical]
    internal InteropBitmap(IntPtr hbitmap, IntPtr hpalette, Int32Rect sourceRect, BitmapSizeOptions sizeOptions, WICBitmapAlphaChannelOption alphaOptions)
      : base(true) {
      _bitmapInit.BeginInit();
      using (FactoryMaker factoryMaker = new FactoryMaker()) {
        HRESULT.Check(UnsafeNativeMethods.WICImagingFactory.CreateBitmapFromHBITMAP(factoryMaker.ImagingFactoryPtr, hbitmap, hpalette, alphaOptions, out _unmanagedSource));
      }
      _unmanagedSource.CalculateSize();
      _sizeOptions = sizeOptions;
      _sourceRect = sourceRect;
      _syncObject = _unmanagedSource;
      _bitmapInit.EndInit();
      FinalizeCreation();
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    internal InteropBitmap(IntPtr hicon, Int32Rect sourceRect, BitmapSizeOptions sizeOptions)
      : base(true) {
      SecurityHelper.DemandUnmanagedCode();
      _bitmapInit.BeginInit();
      using (FactoryMaker factoryMaker = new FactoryMaker()) {
        HRESULT.Check(UnsafeNativeMethods.WICImagingFactory.CreateBitmapFromHICON(factoryMaker.ImagingFactoryPtr, hicon, out _unmanagedSource));
      }
      _unmanagedSource.CalculateSize();
      _sourceRect = sourceRect;
      _sizeOptions = sizeOptions;
      _syncObject = _unmanagedSource;
      _bitmapInit.EndInit();
      FinalizeCreation();
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    internal InteropBitmap(IntPtr section, int pixelWidth, int pixelHeight, PixelFormat format, int stride, int offset)
      : base(true) {
      SecurityHelper.DemandUnmanagedCode();
      _bitmapInit.BeginInit();
      if (pixelWidth <= 0) {
        throw new ArgumentOutOfRangeException("pixelWidth", SR.Get("ParameterMustBeGreaterThanZero"));
      }
      if (pixelHeight <= 0) {
        throw new ArgumentOutOfRangeException("pixelHeight", SR.Get("ParameterMustBeGreaterThanZero"));
      }
      Guid pixelFormatGuid = format.Guid;
      HRESULT.Check(UnsafeNativeMethods.WindowsCodecApi.CreateBitmapFromSection((uint)pixelWidth, (uint)pixelHeight, ref pixelFormatGuid, section, (uint)stride, (uint)offset, out _unmanagedSource));
      _unmanagedSource.CalculateSize();
      _sourceRect = Int32Rect.Empty;
      _sizeOptions = null;
      _syncObject = _unmanagedSource;
      _bitmapInit.EndInit();
      FinalizeCreation();
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    protected override Freezable CreateInstanceCore() {
      return new InteropBitmap();
    }

    [SecurityCritical]
    private void CopyCommon(InteropBitmap sourceBitmapSource) {
      base.Animatable_IsResourceInvalidationNecessary = false;
      _unmanagedSource = sourceBitmapSource._unmanagedSource;
      _sourceRect = sourceBitmapSource._sourceRect;
      _sizeOptions = sourceBitmapSource._sizeOptions;
      InitFromWICSource(sourceBitmapSource.WicSourceHandle);
      base.Animatable_IsResourceInvalidationNecessary = true;
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    protected override void CloneCore(Freezable sourceFreezable) {
      InteropBitmap sourceBitmapSource = (InteropBitmap)sourceFreezable;
      base.CloneCore(sourceFreezable);
      CopyCommon(sourceBitmapSource);
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    protected override void CloneCurrentValueCore(Freezable sourceFreezable) {
      InteropBitmap sourceBitmapSource = (InteropBitmap)sourceFreezable;
      base.CloneCurrentValueCore(sourceFreezable);
      CopyCommon(sourceBitmapSource);
    }

    [SecurityCritical]
    [SecurityTreatAsSafe]
    protected override void GetAsFrozenCore(Freezable sourceFreezable) {
      InteropBitmap sourceBitmapSource = (InteropBitmap)sourceFreezable;
      base.GetAsFrozenCore(sourceFreezable);
      CopyCommon(sourceBitmapSource);
    }

    [SecurityTreatAsSafe]
    [SecurityCritical]
    protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable) {
      InteropBitmap sourceBitmapSource = (InteropBitmap)sourceFreezable;
      base.GetCurrentValueAsFrozenCore(sourceFreezable);
      CopyCommon(sourceBitmapSource);
    }

    [SecurityCritical]
    private void InitFromWICSource(SafeMILHandle wicSource) {
      _bitmapInit.BeginInit();
      BitmapSourceSafeMILHandle ppIBitmap = null;
      lock (_syncObject) {
        using (FactoryMaker factoryMaker = new FactoryMaker()) {
          HRESULT.Check(UnsafeNativeMethods.WICImagingFactory.CreateBitmapFromSource(factoryMaker.ImagingFactoryPtr, wicSource, WICBitmapCreateCacheOptions.WICBitmapCacheOnLoad, out ppIBitmap));
        }
        ppIBitmap.CalculateSize();
      }
      base.WicSourceHandle = ppIBitmap;
      _isSourceCached = true;
      _bitmapInit.EndInit();
      UpdateCachedSettings();
    }

    /// <summary>Forces the hosted non-WPF UI to be rendered.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Interop.InteropBitmap" /> instance is frozen and cannot have its members written to.</exception>
    [SecurityCritical]
    public void Invalidate() {
      Invalidate(null);
    }

    [SecurityCritical]
    public unsafe void Invalidate(Int32Rect? dirtyRect) {
      SecurityHelper.DemandUnmanagedCode();
      Int32Rect value;
      if (dirtyRect.HasValue) {
        value = dirtyRect.Value;
        value.ValidateForDirtyRect("dirtyRect", _pixelWidth, _pixelHeight);
        value = dirtyRect.Value;
        if (!value.HasArea) {
          return;
        }
      }
      WritePreamble();
      if (_unmanagedSource != null) {
        if (base.UsableWithoutCache) {
          int i = 0;
          for (int channelCount = _duceResource.GetChannelCount(); i < channelCount; i++) {
            DUCE.Channel channel = _duceResource.GetChannel(i);
            DUCE.MILCMD_BITMAP_INVALIDATE mILCMD_BITMAP_INVALIDATE = default(DUCE.MILCMD_BITMAP_INVALIDATE);
            mILCMD_BITMAP_INVALIDATE.Type = MILCMD.MilCmdBitmapInvalidate;
            mILCMD_BITMAP_INVALIDATE.Handle = _duceResource.GetHandle(channel);
            bool hasValue = dirtyRect.HasValue;
            if (hasValue) {
              ref MS.Win32.NativeMethods.RECT dirtyRect2 = ref mILCMD_BITMAP_INVALIDATE.DirtyRect;
              value = dirtyRect.Value;
              dirtyRect2.left = value.X;
              ref MS.Win32.NativeMethods.RECT dirtyRect3 = ref mILCMD_BITMAP_INVALIDATE.DirtyRect;
              value = dirtyRect.Value;
              dirtyRect3.top = value.Y;
              ref MS.Win32.NativeMethods.RECT dirtyRect4 = ref mILCMD_BITMAP_INVALIDATE.DirtyRect;
              value = dirtyRect.Value;
              int x = value.X;
              value = dirtyRect.Value;
              dirtyRect4.right = x + value.Width;
              ref MS.Win32.NativeMethods.RECT dirtyRect5 = ref mILCMD_BITMAP_INVALIDATE.DirtyRect;
              value = dirtyRect.Value;
              int y = value.Y;
              value = dirtyRect.Value;
              dirtyRect5.bottom = y + value.Height;
            }
            mILCMD_BITMAP_INVALIDATE.UseDirtyRect = (uint)(hasValue ? 1 : 0);
            channel.SendCommand((byte*)(&mILCMD_BITMAP_INVALIDATE), sizeof(DUCE.MILCMD_BITMAP_INVALIDATE));
          }
        } else {
          _needsUpdate = true;
          RegisterForAsyncUpdateResource();
        }
      }
      WritePostscript();
    }

    [SecurityCritical]
    internal override void FinalizeCreation() {
      BitmapSourceSafeMILHandle ppBitmapClipper = null;
      BitmapSourceSafeMILHandle ppBitmapScaler = null;
      BitmapSourceSafeMILHandle bitmapSourceSafeMILHandle = _unmanagedSource;
      HRESULT.Check(UnsafeNativeMethods.WICBitmap.SetResolution(_unmanagedSource, 96.0, 96.0));
      using (FactoryMaker factoryMaker = new FactoryMaker()) {
        IntPtr imagingFactoryPtr = factoryMaker.ImagingFactoryPtr;
        if (!_sourceRect.IsEmpty) {
          HRESULT.Check(UnsafeNativeMethods.WICImagingFactory.CreateBitmapClipper(imagingFactoryPtr, out ppBitmapClipper));
          lock (_syncObject) {
            HRESULT.Check(UnsafeNativeMethods.WICBitmapClipper.Initialize(ppBitmapClipper, bitmapSourceSafeMILHandle, ref _sourceRect));
          }
          bitmapSourceSafeMILHandle = ppBitmapClipper;
        }
        if (_sizeOptions != null) {
          if (_sizeOptions.DoesScale) {
            _sizeOptions.GetScaledWidthAndHeight((uint)_sizeOptions.PixelWidth, (uint)_sizeOptions.PixelHeight, out uint newWidth, out uint newHeight);
            HRESULT.Check(UnsafeNativeMethods.WICImagingFactory.CreateBitmapScaler(imagingFactoryPtr, out ppBitmapScaler));
            lock (_syncObject) {
              HRESULT.Check(UnsafeNativeMethods.WICBitmapScaler.Initialize(ppBitmapScaler, bitmapSourceSafeMILHandle, newWidth, newHeight, WICInterpolationMode.Fant));
            }
          } else if (_sizeOptions.Rotation != 0) {
            HRESULT.Check(UnsafeNativeMethods.WICImagingFactory.CreateBitmapFlipRotator(imagingFactoryPtr, out ppBitmapScaler));
            lock (_syncObject) {
              HRESULT.Check(UnsafeNativeMethods.WICBitmapFlipRotator.Initialize(ppBitmapScaler, bitmapSourceSafeMILHandle, _sizeOptions.WICTransformOptions));
            }
          }
          if (ppBitmapScaler != null) {
            bitmapSourceSafeMILHandle = ppBitmapScaler;
          }
        }
        base.WicSourceHandle = bitmapSourceSafeMILHandle;
        _isSourceCached = true;
      }
      base.CreationCompleted = true;
      UpdateCachedSettings();
    }
  }
}
