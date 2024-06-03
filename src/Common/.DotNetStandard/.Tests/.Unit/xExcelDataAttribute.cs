using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace Xunit.Extensions {

  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  public sealed class xExcelDataAttribute : DataAttribute {
    /*

      https://ikeptwalking.com/writing-data-driven-tests-using-xunit/

      [Theory]
      [ExcelData("ExcelDataSource.xls", "Select * from TestData")]
      public void SampleTest1(int number, bool expectedResult) {
        var sut = new CheckThisNumber(1);
        var result = sut.CheckIfEqual(number);
        Assert.Equal(result, expectedResult);
      }     
     
    */

    private static readonly string connectionTemplate = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}; Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';";

    public xExcelDataAttribute(string fileName, string queryString) {
      FileName = fileName;
      QueryString = queryString;
    }

    public string FileName { get; }
    public string QueryString { get; }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod) {
      if (testMethod == null) throw new ArgumentNullException("testMethod");
      var pars = testMethod.GetParameters();
      return DataSource(FileName, QueryString, pars.Select(par => par.ParameterType).ToArray());
    }

    private IEnumerable<object[]> DataSource(string fileName, string selectString, Type[] parameterTypes) {
      var connectionString = string.Format(connectionTemplate, GetFullFilename(fileName));
      IDataAdapter adapter = new OleDbDataAdapter(selectString, connectionString);
      var dataSet = new DataSet();

      try {
        adapter.Fill(dataSet);

        foreach (DataRow row in dataSet.Tables[0].Rows)
          yield return ConvertParameters(row.ItemArray, parameterTypes);
      } finally {
        var disposable = adapter as IDisposable;
        disposable.Dispose();
      }
    }

    private static string GetFullFilename(string filename) {
      var executable = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
      return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(executable), filename));
    }

    private static object[] ConvertParameters(object[] values, Type[] parameterTypes) {
      var result = new object[values.Length];

      for (var idx = 0; idx < values.Length; idx++)
        result[idx] = ConvertParameter(values[idx], idx >= parameterTypes.Length ? null : parameterTypes[idx]);

      return result;
    }

    /// <summary>
    /// Converts a parameter to its destination parameter type, if necessary.
    /// </summary>
    /// <param name="parameter">The parameter value</param>
    /// <param name="parameterType">The destination parameter type (null if not known)</param>
    /// <returns>The converted parameter value</returns>
    private static object ConvertParameter(object parameter, Type parameterType) {
      if ((parameter is double || parameter is float) &&
          (parameterType == typeof(int) || parameterType == typeof(int?))) {
        int intValue;
        var floatValueAsString = parameter.ToString();

        if (int.TryParse(floatValueAsString, out intValue))
          return intValue;
      }

      return parameter;
    }
  }
}