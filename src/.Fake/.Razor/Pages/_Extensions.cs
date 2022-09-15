using System.Diagnostics;

namespace X10sions.Fake.Pages {
  public static class _Extensions {

   public static void UseChromeoPrintToPdf() {
      var url = "https://stackoverflow.com/questions/564650/convert-html-to-pdf-in-net";
      var chromePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
      var output = System.IO.Path.Combine(Environment.CurrentDirectory, "printout.pdf");
      using (var p = new Process()) {
        p.StartInfo.FileName = chromePath;
        p.StartInfo.Arguments = $"--headless --disable-gpu --print-to-pdf={output} {url}";
        p.Start();
        p.WaitForExit();
      }
    }

  }
}
