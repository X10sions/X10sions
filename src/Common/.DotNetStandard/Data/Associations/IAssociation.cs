namespace Common.Data.Assocations;

public interface IAssociation {
  IAssociationJoin Join1 { get; }
  IAssociationJoin Join2 { get; }
}

public interface IAssociation<T1, T2> : IAssociation where T1 : class? where T2 : class? {
  new IAssociationJoin<T1, T2> Join1 { get; }
  new IAssociationJoin<T2, T1> Join2 { get; }
}
