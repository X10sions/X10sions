using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

namespace xEFCore.xAS400.Storage.Internal {
  public class ClrTypeMappings_AS400: Dictionary<Type, RelationalTypeMapping> {

    public ClrTypeMappings_AS400() {
      //Add(typeof(int), AS400TypeMapper.Int);
      //Add(typeof(long), AS400TypeMapper.BigInt);
      //Add(typeof(DateTime), AS400TypeMapper.DateTime);
      //Add(typeof(Guid), AS400TypeMapper.UniqueIdentifier);
      //Add(typeof(bool), AS400TypeMapper.Boolean);
      //Add(typeof(byte), AS400TypeMapper.Byte);
      //Add(typeof(double), AS400TypeMapper.Double);
      //Add(typeof(DateTimeOffset), AS400TypeMapper.DateTimeOffset);
      //Add(typeof(char), AS400TypeMapper.Int);
      //Add(typeof(short), AS400TypeMapper.SmallInt);
      //Add(typeof(float), AS400TypeMapper.Real);
      //Add(typeof(decimal), AS400TypeMapper.Decimal);
      //Add(typeof(TimeSpan), AS400TypeMapper.Time);
    }

  }
}