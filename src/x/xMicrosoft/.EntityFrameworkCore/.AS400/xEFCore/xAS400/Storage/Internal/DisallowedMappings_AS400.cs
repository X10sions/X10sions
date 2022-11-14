using System;
using System.Collections.Generic;

namespace xEFCore.xAS400.Storage.Internal {
  public class DisallowedMappings_AS400 : HashSet<string> {
    public DisallowedMappings_AS400() : base(StringComparer.OrdinalIgnoreCase) {
      Add("binary varying");
      Add("binary");
      Add("char varying");
      Add("char");
      Add("character varying");
      Add("character");
      Add("national char varying");
      Add("national character varying");
      Add("national character");
      Add("nchar");
      Add("nvarchar");
      Add("varbinary");
      Add("varchar");
    }
  }
}