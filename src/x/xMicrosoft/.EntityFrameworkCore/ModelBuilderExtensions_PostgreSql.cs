using Microsoft.EntityFrameworkCore;
using System;

namespace Martogg.Data.Orms.EfCoreDbContexts {
  public static class ModelBuilderExtensions {

    public static void UseSnakeCase(this ModelBuilder builder) {
      foreach (var entity in builder.Model.GetEntityTypes()) {
        // Replace table names
        entity.SetTableName(entity.GetTableName().ToSnakeCase());
        // Replace column names            
        foreach (var property in entity.GetProperties()) {
          property.SetColumnName(property.Name.ToSnakeCase());
        }
        foreach (var key in entity.GetKeys()) {
          key.SetName(key.GetName().ToSnakeCase());
        }
        foreach (var key in entity.GetForeignKeys()) {
          key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
        }
        foreach (var index in entity.GetIndexes()) {
          index.SetName(index.GetName().ToSnakeCase());
        }
        //foreach (var nav in entity.GetNavigations()) {
        //  nav.SetField(nav.GetFieldName().ToSnakeCase());
        //}
        //foreach (var fk in entity.GetReferencingForeignKeys()) {
        //  fk.SetConstraintName(fk.GetConstraintName().ToSnakeCase());
        //}
      }
    }

  }
}