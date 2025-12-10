using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Design;


public sealed class DB2iSeriesProviderCodeGenerator : IProviderConfigurationCodeGenerator {
  public MethodCallCodeFragment? GenerateContextOptions() => throw new NotImplementedException();

  public MethodCallCodeFragment GenerateProviderOptions() => new("UseDB2iSeries");

  public MethodCallCodeFragment GenerateUseProvider(string connectionString, MethodCallCodeFragment? providerOptions) {
    var useISeries = new MethodCallCodeFragment("UseDB2iSeries", connectionString);
    return useISeries;
  }

  public IEnumerable<string> GetProviderMethodNames() => new[] { "UseDB2iSeries" };
}
