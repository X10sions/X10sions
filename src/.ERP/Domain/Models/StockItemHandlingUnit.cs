namespace X10sions.ERP.Domain.Models;

/// <summary>
/// Handling Unit (HU) is optional but highly recommended for packaging control
/// </summary>
public record StockItemHandlingUnit(int Id) {
  public string Code { get; init; } //TEXT UNIQUE NOT NULL, --pallet ID, case ID, etc.
  public StockItem Item { get; init; }// BIGINT NOT NULL REFERENCES item(itemid),
  public string LotNumber { get; init; }//TEXT,
  public string SerialNumber { get; init; } //  TEXT,
  public decimal MaxQty { get; init; }//NUMERIC(18,6),        --Max Quantity in Package
  public decimal PlannedQqty { get; init; } //    NUMERIC(18, 6),
  public DateTime CreatedAt => DateTime.Now;
  /*
CONSTRAINT chkhuidents CHECK((lotnumber IS NOT NULL) OR(serialnumber IS NOT NULL))
 */
}



public enum StockItemUnitOfMeasure {
  EA,
  KG,
  L,
}
