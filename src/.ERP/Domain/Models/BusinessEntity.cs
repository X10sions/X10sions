namespace X10sions.ERP.Domain.Models;

/// <summary>
/// Owning Entities (Companies/Organizations)
/// </summary>
/// <param name="Id"></param>
public record BusinessEntity(int Id) {
  public string Code { get; init; } = string.Empty;//VARCHAR(50) UNIQUE NOT NULL,
  public string Name { get; init; } = string.Empty;//VARCHAR(200) NOT NULL,
  public DateTime CreatedDate { get; init; } = DateTime.UtcNow;//TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  public DateTime ModifiedDate { get; init; } = DateTime.UtcNow;// TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  public BusinessEntityType EntityType { get; init; } = BusinessEntityType.Undefined;

  public bool IsActive { get; init; } = true;
  public string? ABN_VAT { get; init; }
  /*
    INDEX idx_entity_code (entity_code),
    INDEX idx_entity_name (entity_name)   
   */
}

public enum BusinessEntityType {
  Undefined,
  Internal,
  Subsidiary,
  Partner
}
