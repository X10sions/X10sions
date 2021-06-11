using LinqToDB;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace LinqToDB.Tests.Base.Tools {
  public static class SettingsReader {

    public static TestSettings Deserialize(string configName, string defaultJson, string? userJson) {
      void Merge(TestSettings settings1, TestSettings settings2) {
        foreach (var connection in settings2.Connections)
          if (!settings1.Connections.ContainsKey(connection.Key))
            settings1.Connections.Add(connection.Key, connection.Value);

        if (settings1.Providers == null)
          settings1.Providers = settings2.Providers;

        if (settings1.Skip == null)
          settings1.Skip = settings2.Skip;

        if (settings1.TraceLevel == null)
          settings1.TraceLevel = settings2.TraceLevel;

        if (settings1.DefaultConfiguration == null)
          settings1.DefaultConfiguration = settings2.DefaultConfiguration;

        if (settings1.NoLinqService == null)
          settings1.NoLinqService = settings2.NoLinqService;
      }

      var defaultSettings = JsonConvert.DeserializeObject<Dictionary<string, TestSettings>>(defaultJson);

      if (userJson != null) {
        var userSettings = JsonConvert.DeserializeObject<Dictionary<string, TestSettings>>(userJson);

        foreach (var uSetting in userSettings) {
          if (defaultSettings.TryGetValue(uSetting.Key, out var dSetting)) {
            Merge(uSetting.Value, dSetting);

            if (uSetting.Value.BasedOn == null)
              uSetting.Value.BasedOn = dSetting.BasedOn;
          } else {
            defaultSettings.Add(uSetting.Key, uSetting.Value);
          }
        }

        foreach (var dSetting in defaultSettings)
          if (!userSettings.ContainsKey(dSetting.Key))
            userSettings.Add(dSetting.Key, dSetting.Value);

        defaultSettings = userSettings;
      }

      var readConfigs = new HashSet<string>();

      TestSettings GetSettings(string config) {
        if (readConfigs.Contains(config))
          throw new InvalidOperationException($"Circle basedOn configuration: '{config}'.");

        readConfigs.Add(config);

        if (!defaultSettings.TryGetValue(config, out var settings))
          throw new InvalidOperationException($"Configuration {config} not found.");

        if (settings.BasedOn != null) {
          var baseOnSettings = GetSettings(settings.BasedOn);

          Merge(settings, baseOnSettings);
        }

        return settings;
      }

      return GetSettings(configName ?? "");
    }

    public static void Serialize() {
      var json = JsonConvert.SerializeObject(
        new Dictionary<string, TestSettings>        {
          {
            "Default",
            new TestSettings            {
              Connections = new Dictionary<string,TestConnection>              {
                { "SqlServer", new TestConnection
                  {
                    ConnectionString = @"Server=DBHost\SQLSERVER2008;Database=TestData;User Id=sa;Password=TestPassword;",
                    Provider         = "SqlServer",
                  }
                },
                { "SqlServer1", new TestConnection
                  {
                    ConnectionString = @"Server=DBHost\SQLSERVER2008;Database=TestData;User Id=sa;Password=TestPassword;",
                    Provider         = "SqlServer1",
                  }
                },
              }
            }
          },
          {
            "CORE21",
            new TestSettings
            {
              Connections = new Dictionary<string,TestConnection>
              {
                { "SqlServer", new TestConnection
                  {
                    ConnectionString = @"Server=DBHost\SQLSERVER2008;Database=TestData;User Id=sa;Password=TestPassword;",
                    Provider         = "SqlServer",
                  }
                },
                { "SqlServer1", new TestConnection
                  {
                    ConnectionString = @"Server=DBHost\SQLSERVER2008;Database=TestData;User Id=sa;Password=TestPassword;",
                    Provider         = "SqlServer1",
                  }
                },
              }
            }
          },
        });

      File.WriteAllText("DefaultTestSettings.json", json);
    }
  }

  public class TestSettingsTests {
    static string _defaultData = @"{
  Default:  {
    Connections:    {
      'Con 1' : { ConnectionString : 'AAA', Provider : 'SqlServer' },
      'Con 2' : { ConnectionString : 'BBB', Provider : 'SqlServer' }
    },
    Providers:    [ '111', '222' ]
  },
  CORE1:  {
    TraceLevel  : 'Error',
    BasedOn     : 'Default',
    Connections :    {
      'Con 2' : { ConnectionString : 'AAA', Provider : 'SqlServer' },
      'Con 3' : { ConnectionString : 'CCC', Provider : 'SqlServer' }
    }
  },
  CORE21:  {
    BasedOn     : 'Default',
    Connections :    {
      'Con 2' : { ConnectionString : 'AAA', Provider : 'SqlServer' },
      'Con 3' : { ConnectionString : 'CCC', Provider : 'SqlServer' }
    }
  }
}";

    static string _userData = @"{
  Default:  {
    Connections:    {
      'Con 1' : { ConnectionString : 'DDD', Provider : 'SqlServer' },
      'Con 4' : { ConnectionString : 'FFF', Provider : 'SqlServer' }
    }
  },
  'CORE21':  {
    BasedOn     : 'Default',
    Connections :    {
      'Con 2' : { ConnectionString : 'WWW', Provider : 'SqlServer' },
      'Con 5' : { ConnectionString : 'EEE', Provider : 'SqlServer' }
    }
  }
}";

    public class TestCaseData {
      public TestCaseData(string name, string config, string defaultJson, string? userJson, string setName, _ReturnItem[] returns) {
        Name = name;
        Config = config;
        DefaultJson = defaultJson;
        UserJson = userJson;
        SetName = setName;
        Returns = returns;
      }

      public string Name { get; set; }
      public string SetName { get; set; }
      public string Config { get; set; }
      public string DefaultJson { get; set; }
      public string? UserJson { get; set; }
      public _ReturnItem[] Returns { get; set; }

      public class _ReturnItem {
        public _ReturnItem(string k, string cs, string p) {
          Key = k;
          ConnectionString = cs;
          Provider = p;
        }
        public string Key { get; set; }
        public string ConnectionString { get; set; }
        public string Provider { get; set; }
      }

    }

    public static TheoryData<TestCaseData> TestTheoryData => new TheoryData<TestCaseData> {
      new TestCaseData("Default", "Default", _defaultData, null, "Tests.Tools.Default",new[]  {
        new TestCaseData._ReturnItem( "Con 1",  "AAA",  "SqlServer" ),
        new TestCaseData._ReturnItem( "Con 2",  "BBB",  "SqlServer" ),
      }),
      new TestCaseData("Core 1", "CORE1", _defaultData, null,"Tests.Tools.Core1",new[]          {
        new TestCaseData._ReturnItem( "Con 1","AAA",  "SqlServer" ),
        new TestCaseData._ReturnItem( "Con 2","AAA",  "SqlServer" ),
        new TestCaseData._ReturnItem( "Con 3","CCC",  "SqlServer" ),
      }),
      new TestCaseData("Core 2.1", "CORE21", _defaultData, null, "Tests.Tools.Core2",new[]          {
        new TestCaseData._ReturnItem( "Con 1","AAA",  "SqlServer" ),
        new TestCaseData._ReturnItem( "Con 2","AAA",  "SqlServer" ),
        new TestCaseData._ReturnItem( "Con 3","CCC",  "SqlServer" ),
      }),
      new TestCaseData("User Default", "Default", _defaultData, _userData, "Tests.Tools.UserDefault",new[]          {
        new TestCaseData._ReturnItem  ( "Con 1","DDD",  "SqlServer" ),
        new TestCaseData._ReturnItem  ( "Con 2","BBB",  "SqlServer" ),
        new  TestCaseData._ReturnItem ( "Con 4","FFF",  "SqlServer" ),
      }),
      new TestCaseData("User Core 1", "CORE1", _defaultData, _userData,"Tests.Tools.UserCore1",new[]          {
        new TestCaseData._ReturnItem  ( "Con 1","DDD",  "SqlServer" ),
        new TestCaseData._ReturnItem  ( "Con 2","AAA",  "SqlServer" ),
        new TestCaseData._ReturnItem  ( "Con 3","CCC",  "SqlServer" ),
        new TestCaseData._ReturnItem  ( "Con 4","FFF",  "SqlServer" ),
      }),
      new TestCaseData("User Core 2.1", "CORE21", _defaultData, _userData,"Tests.Tools.UserCore2",new[]          {
        new  TestCaseData._ReturnItem ( "Con 1","DDD",  "SqlServer" ),
        new TestCaseData._ReturnItem  ( "Con 2","WWW",  "SqlServer" ),
        new TestCaseData._ReturnItem  ( "Con 3","CCC",  "SqlServer" ),
        new TestCaseData._ReturnItem  ( "Con 4","FFF",  "SqlServer" ),
        new TestCaseData._ReturnItem ( "Con 5","EEE",  "SqlServer" ),
      })
    };

    [Theory, MemberData(nameof(TestTheoryData))]
    public IEnumerable DeserializeTest(TestCaseData item) {
      var settings = SettingsReader.Deserialize(item.Config, item.DefaultJson, item.UserJson);
      return settings.Connections.Select(c => new { c.Key, c.Value.ConnectionString, c.Value.Provider }).OrderBy(c => c.Key);
    }

    [Fact(Skip = "Explicit")] public void SerializeTest() => SettingsReader.Serialize();

  }
}