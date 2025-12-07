using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Migrations;

public class DB2iSeriesMigrationsSqlGenerator : MigrationsSqlGenerator {
  public DB2iSeriesMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider annotationProvider) : base(dependencies) { }

  //protected override void Generate(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true) => base.Generate(operation, model, builder, terminate);


  protected override void Generate(DropTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true) {
    builder
        .Append("DROP TABLE ")
        .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Name, operation.Schema))
        .AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
    EndStatement(builder);
  }

  protected override void Generate(AddColumnOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true) {
    builder
        .Append("ALTER TABLE ")
        .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema))
        .Append(" ADD COLUMN ");
    ColumnDefinition(operation, model, builder);
    builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
    EndStatement(builder);
  }

  protected override void ColumnDefinition(string schema, string table, string name, ColumnOperation operation, IModel model, MigrationCommandListBuilder builder) {
    builder.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(name))
        .Append(" ")
        .Append(operation.ColumnType ?? GetColumnType(schema, table, name, operation, model));
    if (!operation.IsNullable) {
      builder.Append(" NOT NULL");
    }
    if (operation.DefaultValue != null) {
      builder.Append(" DEFAULT ");
      var typeMapping = Dependencies.TypeMappingSource.FindMapping(operation.ClrType);
      if (typeMapping != null) {
        builder.Append(typeMapping.GenerateSqlLiteral(operation.DefaultValue));
      } else {
        builder.Append(operation.DefaultValue.ToString());
      }
    } else if (!string.IsNullOrWhiteSpace(operation.DefaultValueSql)) {
      builder.Append(" DEFAULT ")
          .Append(operation.DefaultValueSql);
    }
  }

}