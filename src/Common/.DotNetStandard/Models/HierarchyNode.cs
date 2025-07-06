using System.Collections.Generic;

namespace Common.Models {
  public class HierarchyNode<T> where T : class {
    public T Parent { get; set; }
    public IEnumerable<HierarchyNode<T>> ChildList { get; set; }
    public int Depth { get; set; }
  }
}