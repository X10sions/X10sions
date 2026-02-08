namespace X10sions.ERP.Domain.Models;

/// <summary>Stock Items Master </summary>
public record StockItem(long Id) {
  public string Code { get; init; } = string.Empty; //UNIQUE 
  public string Description { get; init; } = string.Empty;
  public string? Category { get; init; } = null;
  public string? Subcategory { get; init; } = null;
  public StockItemUnitOfMeasure UnitOfMeasure { get; init; } = StockItemUnitOfMeasure.EA;
  public decimal? StandardCost { get; init; } = null;
  public bool IsActive { get; init; } = true;

  public decimal DefaultPackMaxQty { get; init; } = 0;
  public CurrencyCode DefaultCurrency { get; init; } = CurrencyCode.Undefined;

  public DateTime CreatedDate { get; init; } = DateTime.UtcNow;//TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  public DateTime ModifiedDate { get; init; } = DateTime.UtcNow;// TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

  /*
    INDEX idx_item_code (item_code),
    INDEX idx_category (item_category)
  
  */
}
