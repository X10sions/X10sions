using LinqToDB.Mapping;
using Microsoft.AspNetCore.Identity;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public static class MappingSchemaExtensions {

    public static void EncryptIdentityPersonalData<T>(this MappingSchema mappingSchema, StoreOptions storeOptions, DefaultPersonalDataProtector personalDataConverter) where T : class {
      var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
      if (encryptPersonalData) {
        mappingSchema.SetConverter<string, ProtectedDataString>(value => ProtectedDataString.Protect(value));
        mappingSchema.SetConverter<ProtectedDataString, string>(value => ProtectedDataString.Unprotect(value));
      }
    }

  }

}