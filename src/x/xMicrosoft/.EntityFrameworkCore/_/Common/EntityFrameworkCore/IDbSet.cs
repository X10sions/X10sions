using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace Common.EntityFrameworkCore {
  public interface IDbSet<TEntity> : IQueryable<TEntity>, IAsyncEnumerable<TEntity>, IInfrastructure<IServiceProvider>, IListSource where TEntity : class {
  } 
}