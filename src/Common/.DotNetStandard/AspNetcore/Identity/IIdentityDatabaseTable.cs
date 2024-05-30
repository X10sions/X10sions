//using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common.AspNetCore.Identity {
  public interface IIdentityDatabaseTable<T>
    : IEnumerable, IEnumerable<T>
    , IQueryable, IQueryable<T> {

    // Microsoft.EntityFrameworkCore.DbSet
    // LinqToDB.ITable
  }

}