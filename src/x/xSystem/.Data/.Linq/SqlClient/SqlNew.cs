using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.Linq.SqlClient;

namespace System.Data.Linq.SqlClient {
  internal class SqlNew : SqlSimpleTypeExpression {
    private MetaType metaType;
    private ConstructorInfo constructor;
    private List<SqlExpression> args;
    private List<MemberInfo> argMembers;
    private List<SqlMemberAssign> members;

    internal SqlNew(MetaType metaType, ProviderType sqlType, ConstructorInfo cons, IEnumerable<SqlExpression> args, IEnumerable<MemberInfo> argMembers, IEnumerable<SqlMemberAssign> members, Expression sourceExpression)
        : base(SqlNodeType.New, metaType.Type, sqlType, sourceExpression) {
      this.metaType = metaType;

      if (cons == null && metaType.Type.IsClass) { // structs do not need to have a constructor
        throw Error.ArgumentNull("cons");
      }
      constructor = cons;
      this.args = new List<SqlExpression>();
      this.argMembers = new List<MemberInfo>();
      this.members = new List<SqlMemberAssign>();
      if (args != null) {
        this.args.AddRange(args);
      }
      if (argMembers != null) {
        this.argMembers.AddRange(argMembers);
      }
      if (members != null) {
        this.members.AddRange(members);
      }
    }

    internal MetaType MetaType => metaType;

    internal ConstructorInfo Constructor => constructor;

    internal List<SqlExpression> Args => args;

    internal List<MemberInfo> ArgMembers => argMembers;

    internal List<SqlMemberAssign> Members => members;

    internal SqlExpression Find(MemberInfo mi) {
      for (int i = 0, n = argMembers.Count; i < n; i++) {
        var argmi = argMembers[i];
        if (argmi.Name == mi.Name) {
          return args[i];
        }
      }

      foreach (var ma in Members) {
        if (ma.Member.Name == mi.Name) {
          return ma.Expression;
        }
      }

      return null;
    }
  }

}
