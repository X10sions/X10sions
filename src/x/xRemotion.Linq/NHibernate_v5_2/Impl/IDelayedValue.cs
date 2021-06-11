using System;
using System.Collections;

namespace NHibernate_v5_2.Impl {
  [Obsolete("Since 5.2")]
  internal interface IDelayedValue {
    Delegate ExecuteOnEval { get; set; }

    IList TransformList(IList collection);
  }
}