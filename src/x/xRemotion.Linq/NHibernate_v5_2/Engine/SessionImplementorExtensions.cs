using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Multi;

namespace NHibernate_v5_2.Engine {
  public static class SessionImplementorExtensions {

    public static IQueryBatch GetFutureBatch(this ISessionImplementor session) => Util.ReflectHelper.CastOrThrow<AbstractSessionImpl>(session, "future batch").FutureBatch;

  }
}
