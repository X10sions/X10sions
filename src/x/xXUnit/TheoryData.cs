//using System.Collections;
//using System.Collections.Generic;

//namespace xXUnit {
//  public abstract class TheoryData : IEnumerable<object[]> {
//    readonly List<object[]> data = new List<object[]>();

//    protected void AddRow(params object[] values) => data.Add(values);
//    public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();
//    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//  }
//}
