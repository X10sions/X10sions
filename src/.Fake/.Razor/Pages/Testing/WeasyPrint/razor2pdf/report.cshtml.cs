using Microsoft.AspNetCore.Mvc.RazorPages;
using NanoByte.Razor2Pdf;

namespace X10sions.Fake.Pages.Testing.WeasyPrint.Razor2Pdf {

  public class ReportModel : PageModel, IPdfModel {
    public string ViewPath => "/Pages/WeasyPrint/Razor2Pdf/Report.cshtml";
    public string Name { get; set; }
  }
}
