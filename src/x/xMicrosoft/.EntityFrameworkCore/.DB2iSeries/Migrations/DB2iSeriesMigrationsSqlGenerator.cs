using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace xMicrosoft.EntityFrameworkCore.DB2iSeries.Migrations;

public class DB2iSeriesMigrationsSqlGenerator : MigrationsSqlGenerator {
  public DB2iSeriesMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies) : base(dependencies) { }
  //public DB2iSeriesMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, ICommandBatchPreparer commandBatchPreparer) : base(dependencies, commandBatchPreparer) { }
  //public DB2iSeriesMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider annotationProvider) : base(dependencies) { }

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


  //protected override void Generate(CreateTableOperation op, IModel? model, MigrationCommandListBuilder b) {
  //  b.Append("CREATE TABLE ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.Name, op.Schema))
  //   .Append(" (");

  //  for (int i = 0; i < op.Columns.Count; i++) {
  //    var c = op.Columns[i];
  //    b.Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(c.Name))
  //     .Append(" ")
  //     .Append(GetColumnType(c))
  //     .Append(c.IsNullable ? "" : " NOT NULL");

  //    if (!string.IsNullOrEmpty(c.DefaultValueSql)) {
  //      b.Append(" DEFAULT ").Append(c.DefaultValueSql);
  //    } else if (c.DefaultValue != null) {
  //      b.Append(" DEFAULT ").Append(Dependencies.TypeMappingSource.GetMappingForValue(c.DefaultValue).GenerateSqlLiteral(c.DefaultValue));
  //    }

  //    if (i < op.Columns.Count - 1) b.Append(", ");
  //  }

  //  if (op.PrimaryKey != null) {
  //    b.Append(", CONSTRAINT ")
  //     .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.PrimaryKey.Name))
  //     .Append(" PRIMARY KEY (")
  //     .Append(string.Join(", ", op.PrimaryKey.Columns.Select(Dependencies.SqlGenerationHelper.DelimitIdentifier)))
  //     .Append(")");
  //  }

  //  b.Append(")").AppendLine(";");
  //  EndStatement(b);
  //}

  //protected override void Generate(AddColumnOperation op, IModel? model, MigrationCommandListBuilder b) {
  //  b.Append("ALTER TABLE ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.Table, op.Schema))
  //   .Append(" ADD COLUMN ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.Name))
  //   .Append(" ")
  //   .Append(GetColumnType(op))
  //   .Append(op.IsNullable ? "" : " NOT NULL");

  //  if (!string.IsNullOrEmpty(op.DefaultValueSql)) {
  //    b.Append(" DEFAULT ").Append(op.DefaultValueSql);
  //  } else if (op.DefaultValue != null) {
  //    b.Append(" DEFAULT ").Append(Dependencies.TypeMappingSource.GetMappingForValue(op.DefaultValue).GenerateSqlLiteral(op.DefaultValue));
  //  }

  //  b.AppendLine(";");
  //  EndStatement(b);
  //}

  //protected override void Generate(DropColumnOperation op, IModel? model, MigrationCommandListBuilder b) {
  //  b.Append("ALTER TABLE ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.Table, op.Schema))
  //   .Append(" DROP COLUMN ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.Name))
  //   .AppendLine(";");
  //  EndStatement(b);
  //}

  //protected override void Generate(CreateIndexOperation op, IModel? model, MigrationCommandListBuilder b) {
  //  b.Append("CREATE ")
  //   .Append(op.IsUnique ? "UNIQUE " : "")
  //   .Append("INDEX ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.Name))
  //   .Append(" ON ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.Table, op.Schema))
  //   .Append(" (")
  //   .Append(string.Join(", ", op.Columns.Select(Dependencies.SqlGenerationHelper.DelimitIdentifier)))
  //   .Append(")")
  //   .AppendLine(";");
  //  EndStatement(b);
  //}

  //protected override void Generate(DropIndexOperation op, IModel? model, MigrationCommandListBuilder b) {
  //  b.Append("DROP INDEX ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.Name))
  //   .Append(" ON ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.Table, op.Schema))
  //   .AppendLine(";");
  //  EndStatement(b);
  //}

  //protected override void Generate(AddForeignKeyOperation op, IModel? model, MigrationCommandListBuilder b) {
  //  b.Append("ALTER TABLE ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.Table, op.Schema))
  //   .Append(" ADD CONSTRAINT ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.Name))
  //   .Append(" FOREIGN KEY (")
  //   .Append(string.Join(", ", op.Columns.Select(Dependencies.SqlGenerationHelper.DelimitIdentifier)))
  //   .Append(") REFERENCES ")
  //   .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(op.PrincipalTable, op.PrincipalSchema))
  //   .Append(" (")
  //   .Append(string.Join(", ", op.PrincipalColumns.Select(Dependencies.SqlGenerationHelper.DelimitIdentifier)))
  //   .Append(")");

  //  if (op.OnDelete.HasValue) {
  //    b.Append(" ON DELETE ").Append(op.OnDelete.Value switch {
  //      ReferentialAction.Cascade => "CASCADE",
  //      ReferentialAction.SetNull => "SET NULL",
  //      ReferentialAction.Restrict => "RESTRICT",
  //      _ => "NO ACTION"
  //    });
  //  }

  //  b.AppendLine(";");
  //  EndStatement(b);
  //}

  private string GetColumnType(ColumnOperation op) {
    if (!string.IsNullOrEmpty(op.ColumnType)) return op.ColumnType!;
    var clr = op.ClrType;
    return clr == typeof(int) ? "INTEGER"
         : clr == typeof(short) ? "SMALLINT"
         : clr == typeof(long) ? "BIGINT"
         : clr == typeof(Guid) ? "CHAR(36)"
         : clr == typeof(string) ? "VARCHAR(256)"
         : clr == typeof(bool) ? "SMALLINT"
         : clr == typeof(DateTime) ? "TIMESTAMP"
         : clr == typeof(decimal) ? "DECIMAL(18, 6)"
         : "VARCHAR(256)";
  }
}
