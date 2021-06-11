using System.Reflection;

namespace NHibernate_v5_2.Linq.Functions {
  public interface IRuntimeMethodHqlGenerator {
    bool SupportsMethod(MethodInfo method);
    IHqlGeneratorForMethod GetMethodGenerator(MethodInfo method);
  }
}