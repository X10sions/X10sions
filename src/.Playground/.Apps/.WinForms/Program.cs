namespace SharedRazor.BlazorWinForms {
  internal static class Program {
    /// <summary>
    ///  https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/tutorials/windows-forms?view=aspnetcore-6.0
    /// </summary>
    [STAThread]
    static void Main() {
      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
      ApplicationConfiguration.Initialize();
      Application.Run(new Form1());
    }
  }
}