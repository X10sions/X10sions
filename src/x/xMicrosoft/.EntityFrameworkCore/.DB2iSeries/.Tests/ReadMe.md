# EntityFrameworkCore.ISeries.Tests

## Running Tests

### Unit Tests
Unit tests can be run without an DB2iSeries connection:

```bash
dotnet test --filter "FullyQualifiedName!~Integration"
```

### Integration Tests
Integration tests require an actual DB2iSeries v5r4 connection.

1. Update the connection string in `IntegrationTests.cs`
2. Remove the `Skip` attribute from test methods
3. Run:

```bash
dotnet test --filter "FullyQualifiedName~Integration"
```

## Test Coverage

- **DbContext Options**: Tests for UseISeries extension methods
- **Type Mappings**: Tests for CLR to DB2iSeries type conversions
- **SQL Generation**: Tests for identifier delimiting and SQL syntax
- **Migrations**: Tests for DDL statement generation
- **Updates**: Tests for INSERT/UPDATE/DELETE operations
- **Integration**: End-to-end tests with real database

## Mock Setup

The tests use Moq for mocking EF Core dependencies. Key mocked components:
- ISqlGenerationHelper
- IRelationalTypeMappingSource
- IModel

## CI/CD Considerations

For CI/CD pipelines, integration tests should be skipped or run against a test DB2iSeries instance:

```yaml
# Example GitHub Actions
- name: Run Unit Tests
  run: dotnet test --filter "FullyQualifiedName!~Integration"
```


## Running Tests:

### Unit tests only:

dotnet test --filter "FullyQualifiedName!~Integration"

### Integration tests (after updating connection string):

dotnet test --filter "FullyQualifiedName~Integration"

### All tests:

dotnet test











# EFCore.ISeries

An Entity Framework Core provider for IBM i (AS/400) focusing on v5r4 compatibility via ODBC.

## Features
- Querying and CRUD over ODBC.
- Identifier quoting with double-quotes.
- Boolean emulation via SMALLINT (0/1).
- `Take()` via `FETCH FIRST n ROWS ONLY`.
- Basic migrations: create table, add/drop column, PK, indexes, FKs.

## Limitations
- `Skip()` not supported by default on v5r4 (use `Take()` and keyset pagination).
- No native BOOLEAN; use SMALLINT.
- Identity columns may be limited. Prefer client-generated GUIDs.

## Connection string example


## Usage
```csharp
var options = new DbContextOptionsBuilder<MyDbContext>()
    .UseISeries("Driver={IBM i Access ODBC Driver};System=MYISERIES;Uid=USER;Pwd=PASS;Naming=1;DefaultLibraries=MYLIB")
    .Options;
