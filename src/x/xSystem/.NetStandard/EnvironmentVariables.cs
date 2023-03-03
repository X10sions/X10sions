namespace System {
  public static class EnvironmentVariables {
    public static string AspNetCore_Environment() => Environment.GetEnvironmentVariable(nameof(AspNetCore_Environment));
  }
}