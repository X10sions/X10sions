namespace X10sions.ERP.Domain.Models;

/// <summary>
/// Handling Unit (HU) is optional but highly recommended for packaging control(LPN, BAG, PALLET)
/// </summary>
public record StockHandlingUnit(int Id) {
  public StockItem Item { get; init; }// BIGINT NOT NULL REFERENCES item(itemid),
  //public BusinessEntity OwningEntity { get; init; }  // BIGINT NOT NULL REFERENCES owningentity(owningentityid),

  //public BusinessOrder WorkOrder { get; init; }

  //public string Code { get; init; } //TEXT UNIQUE NOT NULL, --pallet ID, case ID, etc.
  //public string LotNumber { get; init; }//TEXT,
  //public string SerialNumber { get; init; } //  TEXT,
  public decimal PlannedQuantity { get; init; } //    NUMERIC(18, 6),
  public DateTime CreatedAt => DateTime.Now;
  /*
CONSTRAINT chkhuidents CHECK((lotnumber IS NOT NULL) OR(serialnumber IS NOT NULL))
 */
}


