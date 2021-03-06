﻿//using LinqToDB;
//using LinqToDB.Common;
//using LinqToDB.Data;
//using LinqToDB.DataProvider.Informix;
//using LinqToDB.Mapping;
//using LinqToDB.Reflection;
//using LinqToDB.Tests.Base.Tools;
//using LinqToDB.Tests.Model;
//using LinqToDB.Tools;
//using LinqToDB.Tools.Comparers;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using Xunit;
//using static LinqToDB.Tests._Extensions;

//namespace LinqToDB.Tests.Base {
//  //	[Order(1000)]
//  public class TestBase : IDisposable {
//    private const int TRACES_LIMIT = 50000;

//    static TestBase() {
//      Console.WriteLine("Tests started in {0}...", Environment.CurrentDirectory);
//      Console.WriteLine("CLR Version: {0}...", Environment.Version);
//      var traceCount = 0;

//      DataConnection.TurnTraceSwitchOn(TraceLevel.Info);
//      DataConnection.WriteTraceLine = (message, name, level) => {
//        var ctx = CustomTestContext.Get();
//        var trace = ctx.Get<StringBuilder>(CustomTestContext.TRACE);
//        if (trace == null) {
//          trace = new StringBuilder();
//          ctx.Set(CustomTestContext.TRACE, trace);
//        }
//        trace.AppendLine($"{name}: {message}");
//        if (traceCount < TRACES_LIMIT || level == TraceLevel.Error) {
//          ctx.Set(CustomTestContext.LIMITED, true);
//          Console.WriteLine("{0}: {1}", name, message);
//          Debug.WriteLine(message, name);
//        }
//        traceCount++;
//      };
//      //			Configuration.RetryPolicy.Factory = db => new Retry();
//      LinqToDB.Common.Configuration.Linq.TraceMapperExpression = false;
//      //			Configuration.Linq.GenerateExpressionTest  = true;
//      var assemblyPath = typeof(TestBase).Assembly.GetPath();
//      if (1 == 2) {
//        Environment.CurrentDirectory = assemblyPath;

//        var dataProvidersJsonFile = GetFilePath(assemblyPath, @"DataProviders.json");
//        var userDataProvidersJsonFile = GetFilePath(assemblyPath, @"UserDataProviders.json");

//        var dataProvidersJson = File.ReadAllText(dataProvidersJsonFile);
//        var userDataProvidersJson = File.Exists(userDataProvidersJsonFile) ? File.ReadAllText(userDataProvidersJsonFile) : null;
//        var configName = "CORE31";
//        var testSettings = SettingsReader.Deserialize(configName, dataProvidersJson, userDataProvidersJson);
//        var databasePath = Path.GetFullPath(Path.Combine("Database"));
//        var dataPath = Path.Combine(databasePath, "Data");
//        if (Directory.Exists(dataPath))
//          Directory.Delete(dataPath, true);
//        Directory.CreateDirectory(dataPath);
//        foreach (var file in Directory.GetFiles(databasePath, "*.*")) {
//          var destination = Path.Combine(dataPath, Path.GetFileName(file));
//          Console.WriteLine("{0} => {1}", file, destination);
//          File.Copy(file, destination, true);
//        }
//        //UserProviders = new HashSet<string>(testSettings.Providers ?? Array<string>.Empty, StringComparer.OrdinalIgnoreCase);
//        //SkipCategories = new HashSet<string>(testSettings.Skip ?? Array<string>.Empty, StringComparer.OrdinalIgnoreCase);

//        var logLevel = testSettings.TraceLevel;
//        var traceLevel = TraceLevel.Info;

//        if (!string.IsNullOrEmpty(logLevel))
//          if (!Enum.TryParse(logLevel, true, out traceLevel))
//            traceLevel = TraceLevel.Info;

//        if (!string.IsNullOrEmpty(testSettings.NoLinqService))
//          DataSourcesBaseAttribute.NoLinqService = ConvertTo<bool>.From(testSettings.NoLinqService);
//        Console.WriteLine("Connection strings:");
//        TxtSettings.Instance.DefaultConfiguration = "SQLiteMs";
//        foreach (var provider in testSettings.Connections/*.Where(c => UserProviders.Contains(c.Key))*/) {
//          if (string.IsNullOrWhiteSpace(provider.Value.ConnectionString))
//            throw new InvalidOperationException("ConnectionString should be provided");
//          Console.WriteLine($"\tName=\"{provider.Key}\", Provider=\"{provider.Value.Provider}\", ConnectionString=\"{provider.Value.ConnectionString}\"");

//          TxtSettings.Instance.AddConnectionString(
//             provider.Key, provider.Value.Provider ?? "", provider.Value.ConnectionString);
//        }
//        //DataConnection.DefaultSettings = TxtSettings.Instance;


//        Console.WriteLine("Providers:");

//        //foreach (var userProvider in UserProviders)
//        //  Console.WriteLine($"\t{userProvider}");

//        //DefaultProvider = testSettings.DefaultConfiguration;
//      }
//      DataConnection.TurnTraceSwitchOn(TraceLevel.Info);
//    }

//    //public static readonly HashSet<string> UserProviders;
//    //public static readonly string? DefaultProvider;
//    //public static readonly HashSet<string> SkipCategories;

//    protected ITestDataContext GetDataContext(string configuration, MappingSchema? ms = null) {
//      if (configuration.EndsWith(".LinqService")) {
//        configuration = configuration.Substring(0, configuration.Length - ".LinqService".Length);
//      }
//      Debug.WriteLine(configuration, "Provider ");
//      var res = new TestDataConnection(configuration);
//      if (ms != null)
//        res.AddMappingSchema(ms);
//      return res;
//    }

//    protected void TestOnePerson(int id, string firstName, IQueryable<Person> persons) {
//      var list = persons.ToList();
//      Assert.Single(list);
//      var person = list[0];
//      Assert.Equal(id, person.ID);
//      Assert.Equal(firstName, person.FirstName);
//    }

//    protected void TestOneJohn(IQueryable<Person> persons) => TestOnePerson(1, "John", persons);

//    protected void TestPerson(int id, string firstName, IQueryable<IPerson> persons) {
//      var person = persons.ToList().First(p => p.ID == id);

//      Assert.Equal(id, person.ID);
//      Assert.Equal(firstName, person.FirstName);
//    }

//    protected void TestJohn(IQueryable<IPerson> persons) => TestPerson(1, "John", persons);

//    private List<LinqDataTypes>? _types;
//    protected IEnumerable<LinqDataTypes> Types {
//      get {
//        if (_types == null)
//          using (new DisableLogging())
//          using (var db = new TestDataConnection())
//            _types = db.Types.ToList();

//        return _types;
//      }
//    }

//    private List<LinqDataTypes2>? _types2;
//    protected List<LinqDataTypes2> Types2 {
//      get {
//        if (_types2 == null)
//          using (new DisableLogging())
//          using (var db = new TestDataConnection())
//            _types2 = db.Types2.ToList();

//        return _types2;
//      }
//    }

//    protected internal const int MaxPersonID = 4;

//    private List<Person>? _person;
//    protected IEnumerable<Person> Person {
//      get {
//        if (_person == null) {
//          using (new DisableLogging())
//          using (var db = new TestDataConnection())
//            _person = db.Person.ToList();

//          foreach (var p in _person)
//            p.Patient = Patient.SingleOrDefault(ps => p.ID == ps.PersonID);
//        }

//        return _person;
//      }
//    }

//    private List<Patient>? _patient;
//    protected List<Patient> Patient {
//      get {
//        if (_patient == null) {
//          using (new DisableLogging())
//          using (var db = new TestDataConnection())
//            _patient = db.Patient.ToList();

//          foreach (var p in _patient)
//            p.Person = Person.Single(ps => ps.ID == p.PersonID);
//        }

//        return _patient;
//      }
//    }

//    private List<Doctor>? _doctor;
//    protected List<Doctor> Doctor {
//      get {
//        if (_doctor == null) {
//          using (new DisableLogging())
//          using (var db = new TestDataConnection())
//            _doctor = db.Doctor.ToList();
//        }

//        return _doctor;
//      }
//    }

//    #region Parent/Child Model

//    private List<Parent>? _parent;
//    protected IEnumerable<Parent> Parent {
//      get {
//        if (_parent == null)
//          using (new DisableLogging())
//          using (var db = new TestDataConnection()) {
//            db.Parent.Delete(c => c.ParentID >= 1000);
//            _parent = db.Parent.ToList();
//            db.Close();

//            foreach (var p in _parent) {
//              p.ParentTest = p;
//              p.Children = Child.Where(c => c.ParentID == p.ParentID).ToList();
//              p.GrandChildren = GrandChild.Where(c => c.ParentID == p.ParentID).ToList();
//              p.Types = Types.FirstOrDefault(t => t.ID == p.ParentID);
//            }
//          }

//        return _parent;
//      }
//    }

//    private List<Parent1>? _parent1;
//    protected IEnumerable<Parent1> Parent1 {
//      get {
//        if (_parent1 == null)
//          _parent1 = Parent.Select(p => new Parent1 { ParentID = p.ParentID, Value1 = p.Value1 }).ToList();

//        return _parent1;
//      }
//    }

//    private List<Parent4>? _parent4;
//    protected List<Parent4> Parent4 => _parent4 ??= Parent.Select(p => new Parent4 { ParentID = p.ParentID, Value1 = ConvertTo<TypeValue>.From(p.Value1) }).ToList();

//    private List<Parent5>? _parent5;
//    protected List<Parent5> Parent5 {
//      get {
//        if (_parent5 == null) {
//          _parent5 = Parent.Select(p => new Parent5 { ParentID = p.ParentID, Value1 = p.Value1 }).ToList();

//          foreach (var p in _parent5)
//            p.Children = _parent5.Where(c => c.Value1 == p.ParentID).ToList();
//        }

//        return _parent5;
//      }
//    }

//    private List<ParentInheritanceBase>? _parentInheritance;
//    protected IEnumerable<ParentInheritanceBase> ParentInheritance {
//      get {
//        if (_parentInheritance == null)
//          _parentInheritance = Parent.Select(p =>
//            p.Value1 == null ? new ParentInheritanceNull { ParentID = p.ParentID } :
//            p.Value1.Value == 1 ? new ParentInheritance1 { ParentID = p.ParentID, Value1 = p.Value1.Value } :
//             (ParentInheritanceBase)new ParentInheritanceValue { ParentID = p.ParentID, Value1 = p.Value1.Value }
//          ).ToList();

//        return _parentInheritance;
//      }
//    }

//    private List<ParentInheritanceValue>? _parentInheritanceValue;
//    protected List<ParentInheritanceValue> ParentInheritanceValue => _parentInheritanceValue ??=
//          ParentInheritance.Where(p => p is ParentInheritanceValue).Cast<ParentInheritanceValue>().ToList();

//    private List<ParentInheritance1>? _parentInheritance1;
//    protected List<ParentInheritance1> ParentInheritance1 => _parentInheritance1 ??=
//          ParentInheritance.Where(p => p is ParentInheritance1).Cast<ParentInheritance1>().ToList();

//    private List<ParentInheritanceBase4>? _parentInheritance4;
//    protected List<ParentInheritanceBase4> ParentInheritance4 => _parentInheritance4 ??= Parent
//          .Where(p => p.Value1.HasValue && (new[] { 1, 2 }.Contains(p.Value1.Value)))
//          .Select(p => p.Value1 == 1 ?
//            new ParentInheritance14 { ParentID = p.ParentID } :
//            (ParentInheritanceBase4)new ParentInheritance24 { ParentID = p.ParentID }
//        ).ToList();

//    protected List<Child>? _child;
//    protected IEnumerable<Child> Child {
//      get {
//        if (_child == null)
//          using (new DisableLogging())
//          using (var db = new TestDataConnection()) {
//            db.Child.Delete(c => c.ParentID >= 1000);
//            _child = db.Child.ToList();
//            db.Close();

//            foreach (var ch in _child) {
//              ch.Parent = Parent.Single(p => p.ParentID == ch.ParentID);
//              ch.Parent1 = Parent1.Single(p => p.ParentID == ch.ParentID);
//              ch.ParentID2 = new Parent3 { ParentID2 = ch.Parent.ParentID, Value1 = ch.Parent.Value1 };
//              ch.GrandChildren = GrandChild.Where(c => c.ParentID == ch.ParentID && c.ChildID == ch.ChildID).ToList();
//            }
//          }

//        foreach (var item in _child)
//          yield return item;
//      }
//    }

//    private List<GrandChild>? _grandChild;
//    protected IEnumerable<GrandChild> GrandChild {
//      get {
//        if (_grandChild == null)
//          using (new DisableLogging())
//          using (var db = new TestDataConnection()) {
//            _grandChild = db.GrandChild.ToList();
//            db.Close();

//            foreach (var ch in _grandChild)
//              ch.Child = Child.Single(c => c.ParentID == ch.ParentID && c.ChildID == ch.ChildID);
//          }

//        return _grandChild;
//      }
//    }

//    private List<GrandChild1>? _grandChild1;
//    protected IEnumerable<GrandChild1> GrandChild1 {
//      get {
//        if (_grandChild1 == null)
//          using (new DisableLogging())
//          using (var db = new TestDataConnection()) {
//            _grandChild1 = db.GrandChild1.ToList();

//            foreach (var ch in _grandChild1) {
//              ch.Parent = Parent1.Single(p => p.ParentID == ch.ParentID);
//              ch.Child = Child.Single(c => c.ParentID == ch.ParentID && c.ChildID == ch.ChildID);
//            }
//          }

//        return _grandChild1;
//      }
//    }

//    #endregion

//    #region Inheritance Parent/Child Model

//    private List<InheritanceParentBase>? _inheritanceParent;
//    protected List<InheritanceParentBase> InheritanceParent {
//      get {
//        if (_inheritanceParent == null) {
//          using (new DisableLogging())
//          using (var db = new TestDataConnection())
//            _inheritanceParent = db.InheritanceParent.ToList();
//        }

//        return _inheritanceParent;
//      }
//    }

//    private List<InheritanceChildBase>? _inheritanceChild;
//    protected List<InheritanceChildBase> InheritanceChild {
//      get {
//        if (_inheritanceChild == null) {
//          using (new DisableLogging())
//          using (var db = new TestDataConnection())
//            _inheritanceChild = db.InheritanceChild.LoadWith(_ => _.Parent).ToList();
//        }

//        return _inheritanceChild;
//      }
//    }

//    #endregion

//    #region Northwind

//    public TestBaseNorthwind GetNorthwindAsList(string context) => new TestBaseNorthwind(context);

//    public class TestBaseNorthwind {
//      private string _context;

//      public TestBaseNorthwind(string context) {
//        _context = context;
//      }

//      private List<Northwind.Category>? _category;
//      public List<Northwind.Category> Category {
//        get {
//          if (_category == null)
//            using (new DisableLogging())
//            using (var db = new NorthwindDB(_context))
//              _category = db.Category.ToList();
//          return _category;
//        }
//      }

//      private List<Northwind.Customer>? _customer;
//      public List<Northwind.Customer> Customer {
//        get {
//          if (_customer == null) {
//            using (new DisableLogging())
//            using (var db = new NorthwindDB(_context))
//              _customer = db.Customer.ToList();

//            foreach (var c in _customer)
//              c.Orders = (from o in Order where o.CustomerID == c.CustomerID select o).ToList();
//          }

//          return _customer;
//        }
//      }

//      private List<Northwind.Employee>? _employee;
//      public List<Northwind.Employee> Employee {
//        get {
//          if (_employee == null) {
//            using (new DisableLogging())
//            using (var db = new NorthwindDB(_context)) {
//              _employee = db.Employee.ToList();

//              foreach (var employee in _employee) {
//                employee.Employees = (from e in _employee where e.ReportsTo == employee.EmployeeID select e).ToList();
//                employee.ReportsToEmployee = (from e in _employee where e.EmployeeID == employee.ReportsTo select e).SingleOrDefault();
//              }
//            }
//          }

//          return _employee;
//        }
//      }

//      private List<Northwind.EmployeeTerritory>? _employeeTerritory;
//      public List<Northwind.EmployeeTerritory> EmployeeTerritory {
//        get {
//          if (_employeeTerritory == null)
//            using (new DisableLogging())
//            using (var db = new NorthwindDB(_context))
//              _employeeTerritory = db.EmployeeTerritory.ToList();
//          return _employeeTerritory;
//        }
//      }

//      private List<Northwind.OrderDetail>? _orderDetail;
//      public List<Northwind.OrderDetail> OrderDetail {
//        get {
//          if (_orderDetail == null)
//            using (new DisableLogging())
//            using (var db = new NorthwindDB(_context))
//              _orderDetail = db.OrderDetail.ToList();
//          return _orderDetail;
//        }
//      }

//      private List<Northwind.Order>? _order;
//      public List<Northwind.Order> Order {
//        get {
//          if (_order == null) {
//            using (new DisableLogging())
//            using (var db = new NorthwindDB(_context))
//              _order = db.Order.ToList();

//            foreach (var o in _order) {
//              o.Customer = Customer.Single(c => o.CustomerID == c.CustomerID);
//              o.Employee = Employee.Single(e => o.EmployeeID == e.EmployeeID);
//            }
//          }

//          return _order;
//        }
//      }

//      private IEnumerable<Northwind.Product>? _product;
//      public IEnumerable<Northwind.Product> Product {
//        get {
//          if (_product == null)
//            using (new DisableLogging())
//            using (var db = new NorthwindDB(_context))
//              _product = db.Product.ToList();

//          foreach (var product in _product)
//            yield return product;
//        }
//      }

//      private List<Northwind.ActiveProduct>? _activeProduct;
//      public List<Northwind.ActiveProduct> ActiveProduct => _activeProduct ??= Product.OfType<Northwind.ActiveProduct>().ToList();

//      public IEnumerable<Northwind.DiscontinuedProduct> DiscontinuedProduct => Product.OfType<Northwind.DiscontinuedProduct>();

//      private List<Northwind.Region>? _region;
//      public List<Northwind.Region> Region {
//        get {
//          if (_region == null)
//            using (new DisableLogging())
//            using (var db = new NorthwindDB(_context))
//              _region = db.Region.ToList();
//          return _region;
//        }
//      }

//      private List<Northwind.Shipper>? _shipper;
//      public List<Northwind.Shipper> Shipper {
//        get {
//          if (_shipper == null)
//            using (new DisableLogging())
//            using (var db = new NorthwindDB(_context))
//              _shipper = db.Shipper.ToList();
//          return _shipper;
//        }
//      }

//      private List<Northwind.Supplier>? _supplier;
//      public List<Northwind.Supplier> Supplier {
//        get {
//          if (_supplier == null)
//            using (new DisableLogging())
//            using (var db = new NorthwindDB(_context))
//              _supplier = db.Supplier.ToList();
//          return _supplier;
//        }
//      }

//      private List<Northwind.Territory>? _territory;
//      public List<Northwind.Territory> Territory {
//        get {
//          if (_territory == null)
//            using (new DisableLogging())
//            using (var db = new NorthwindDB(_context))
//              _territory = db.Territory.ToList();
//          return _territory;
//        }
//      }
//    }

//    #endregion

//    protected IEnumerable<LinqDataTypes2> AdjustExpectedData(ITestDataContext db, IEnumerable<LinqDataTypes2> data) {
//      if (db.ProviderNeedsTimeFix(db.ContextID)) {
//        var adjusted = new List<LinqDataTypes2>();
//        foreach (var record in data) {
//          var copy = new LinqDataTypes2() {
//            ID = record.ID,
//            MoneyValue = record.MoneyValue,
//            DateTimeValue = record.DateTimeValue,
//            DateTimeValue2 = record.DateTimeValue2,
//            BoolValue = record.BoolValue,
//            GuidValue = record.GuidValue,
//            SmallIntValue = record.SmallIntValue,
//            IntValue = record.IntValue,
//            BigIntValue = record.BigIntValue,
//            StringValue = record.StringValue
//          };

//          if (copy.DateTimeValue != null) {
//            copy.DateTimeValue = copy.DateTimeValue.Value.AddMilliseconds(-copy.DateTimeValue.Value.Millisecond);
//          }

//          adjusted.Add(copy);
//        }

//        return adjusted;
//      }

//      return data;
//    }

//    protected void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> result, bool allowEmpty = false) => AreEqual(t => t, expected, result, EqualityComparer<T>.Default, allowEmpty);
//    protected void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> result, Func<IEnumerable<T>, IEnumerable<T>> sort) => AreEqual(t => t, expected, result, EqualityComparer<T>.Default, sort);
//    protected void AreEqualWithComparer<T>(IEnumerable<T> expected, IEnumerable<T> result) => AreEqual(t => t, expected, result, ComparerBuilder.GetEqualityComparer<T>());
//    protected void AreEqualWithComparer<T>(IEnumerable<T> expected, IEnumerable<T> result, Func<MemberAccessor, bool> memberPredicate) => AreEqual(t => t, expected, result, ComparerBuilder.GetEqualityComparer<T>(memberPredicate));
//    protected void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> result, IEqualityComparer<T> comparer) => AreEqual(t => t, expected, result, comparer);
//    protected void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> result, IEqualityComparer<T> comparer, Func<IEnumerable<T>, IEnumerable<T>> sort) => AreEqual(t => t, expected, result, comparer, sort);
//    protected void AreEqual<T>(Func<T, T> fixSelector, IEnumerable<T> expected, IEnumerable<T> result) => AreEqual(fixSelector, expected, result, EqualityComparer<T>.Default);
//    protected void AreEqual<T>(Func<T, T> fixSelector, IEnumerable<T> expected, IEnumerable<T> result, IEqualityComparer<T> comparer, bool allowEmpty = false) => AreEqual<T>(fixSelector, expected, result, comparer, null, allowEmpty);

//    protected void AreEqual<T>(
//      Func<T, T> fixSelector,
//      IEnumerable<T> expected,
//      IEnumerable<T> result,
//      IEqualityComparer<T> comparer,
//      Func<IEnumerable<T>, IEnumerable<T>>? sort,
//      bool allowEmpty = false) {
//      var resultList = result.Select(fixSelector).ToList();
//      var expectedList = expected.Select(fixSelector).ToList();
//      if (sort != null) {
//        resultList = sort(resultList).ToList();
//        expectedList = sort(expectedList).ToList();
//      }
//      if (!allowEmpty)
//        Assert.NotEmpty(expectedList);
//      Assert.Equal(expectedList.Count, resultList.Count);

//      var exceptExpectedList = resultList.Except(expectedList, comparer).ToList();
//      var exceptResultList = expectedList.Except(resultList, comparer).ToList();

//      var exceptExpected = exceptExpectedList.Count;
//      var exceptResult = exceptResultList.Count;
//      var message = new StringBuilder();

//      if (exceptResult != 0 || exceptExpected != 0) {
//        Debug.WriteLine(resultList.ToDiagnosticString());
//        Debug.WriteLine(expectedList.ToDiagnosticString());
//        for (var i = 0; i < resultList.Count; i++) {
//          Debug.WriteLine("{0} {1} --- {2}", comparer.Equals(expectedList[i], resultList[i]) ? " " : "-", expectedList[i], resultList[i]);
//          message.AppendFormat("{0} {1} --- {2}", comparer.Equals(expectedList[i], resultList[i]) ? " " : "-", expectedList[i], resultList[i]);
//          message.AppendLine();
//        }
//      }

//      Assert.Equal(0, exceptExpected);// $"Expected Was{Environment.NewLine}{message}");
//      Assert.Equal(0, exceptResult);// $"Expect Result{Environment.NewLine}{message}");
//    }

//    protected void AreEqual<T>(IEnumerable<IEnumerable<T>> expected, IEnumerable<IEnumerable<T>> result) {
//      var resultList = result.ToList();
//      var expectedList = expected.ToList();
//      Assert.NotEmpty(expectedList);
//      Assert.Equal(expectedList.Count, resultList.Count);//, "Expected and result lists are different. Length: ");
//      for (var i = 0; i < resultList.Count; i++) {
//        var elist = expectedList[i].ToList();
//        var rlist = resultList[i].ToList();
//        if (elist.Count > 0 || rlist.Count > 0)
//          AreEqual(elist, rlist);
//      }
//    }

//    protected void AreSame<T>(IEnumerable<T> expected, IEnumerable<T> result) {
//      var resultList = result.ToList();
//      var expectedList = expected.ToList();

//      Assert.NotEmpty(expectedList);
//      Assert.Equal(expectedList.Count, resultList.Count);

//      var b = expectedList.SequenceEqual(resultList);

//      if (!b)
//        for (var i = 0; i < resultList.Count; i++)
//          Debug.WriteLine("{0} {1} --- {2}", Equals(expectedList[i], resultList[i]) ? " " : "-", expectedList[i], resultList[i]);

//      Assert.True(b);
//    }

//    protected void CompareSql(string expected, string result) {
//      Assert.Equal(normalize(expected), normalize(result));

//      string normalize(string sql) {
//        var lines = sql.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
//        return string.Join("\n", lines.Where(l => !l.StartsWith("-- ")).Select(l => l.TrimStart('\t', ' ')));
//      }
//    }

//    protected List<LinqDataTypes> GetTypes(string context) => DataCache<LinqDataTypes>.Get(context);


//    //[TearDown]
//    public virtual void OnAfterTest() =>
//      //if (TestContext.CurrentContext.Result.FailCount > 0) {
//      //  var ctx = CustomTestContext.Get();
//      //  var trace = ctx.Get<StringBuilder>(CustomTestContext.TRACE);
//      //  if (trace != null && ctx.Get<bool>(CustomTestContext.LIMITED)) {
//      //    // we need to set ErrorInfo.Message element text
//      //    // because Azure displays only ErrorInfo node data
//      //    TestExecutionContext.CurrentContext.CurrentResult.SetResult(
//      //      TestExecutionContext.CurrentContext.CurrentResult.ResultState,
//      //      TestExecutionContext.CurrentContext.CurrentResult.Message + "\r\n" + trace.ToString(),
//      //      TestExecutionContext.CurrentContext.CurrentResult.StackTrace);
//      //  }
//      //}
//      CustomTestContext.Release();

//    protected bool IsIDSProvider(string context) {
//      if (!context.Contains("Informix"))
//        return false;
//      var providerName = GetProviderName(context, out var _);
//      if (providerName == ProviderName.InformixDB2)
//        return true;
//      using (DataConnection dc = new TestDataConnection(GetProviderName(context, out var _)))
//        return ((InformixDataProvider)dc.DataProvider).Adapter.IsIDSProvider;
//    }

//    public void Dispose() => OnAfterTest();

//  }
//}