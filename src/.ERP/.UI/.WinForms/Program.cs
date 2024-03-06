namespace X10sions.ERP.WinForms {
  internal static class Program {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
      AppDomain.CurrentDomain.UnhandledException += (sender, error) => {
        MessageBox.Show(text: error.ExceptionObject.ToString(), caption: "Error");
      };
      ApplicationConfiguration.Initialize();
      Application.Run(new Form1());
    }
  }
}