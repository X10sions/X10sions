using System;

namespace OfficeOpenXml {
  public static class ePaperSizeExtensions {

    public static int PaperWidthInMillimetres(this ePaperSize paperSize) {
      switch (paperSize) {
        case ePaperSize.Legal:
        case ePaperSize.Letter:
          return 216;
        case ePaperSize.Executive: return 184;
        case ePaperSize.A2: return 420;
        case ePaperSize.A3: return 297;
        case ePaperSize.A4: return 210;
        case ePaperSize.A5: return 148;
        default:
          throw new NotImplementedException($"Unknown for '{paperSize}'.");
      }

    }

    public static int PaperHeightInMillimetres(this ePaperSize paperSize) {
      switch (paperSize) {
        case ePaperSize.A2: return 594;
        case ePaperSize.A3: return 420;
        case ePaperSize.A4: return 297;
        case ePaperSize.A5: return 210;
        default:
          throw new NotImplementedException($"Unknown for '{paperSize}'.");
      }
    }

  }
}