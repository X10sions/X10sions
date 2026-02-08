namespace X10sions.ERP.Domain.Models;

/// <summary>
/// Main Inventory Records (Current State)
/// </summary>
public record Stock(
  long Id, //PRIMARY KEY AUTO_INCREMENT,
  string InventoryNumber,//VARCHAR(50) UNIQUE NOT NULL,
  BusinessEntity EntityId,
  int ItemId,
  GeoLocation LocationId,
  QualityStatus QualityStatus,
  decimal CurrentPrice,
  // Quantities
  decimal MaxQuantity,
  decimal PlannedQuantity,
  decimal ActualQuantity,
  DateTime EntryDate,
  decimal AllocatedQuantity = 0,
  CurrencyCode CurrencyCode = CurrencyCode.Undefined,
  // Package Info
  string? LotNumber = null,
  string? SerialNumber = null,
  string? BatchNumber = null,
  // Dates
  DateTime? ManufactureDate = null,
  DateTime? ExpiryDate = null,
  DateTime? LastInspectionDate = null,
  DateTime? NextInspectionDate = null,
  // Status
  string InventoryStatus = "ACTIVE", // 'ACTIVE', 'ALLOCATED', 'SHIPPED', 'CONSUMED'
  // Audit
  Person? CreatedBy = null,
  DateTime? createdDate = null,
  Person? ModifiedBy = null,
  DateTime? modifiedDate = null) {
  public DateTime CreatedDate { get; init; } = createdDate ?? DateTime.UtcNow;//TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  public DateTime ModifiedDate { get; init; } = modifiedDate ?? DateTime.UtcNow;// TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  /*
    FOREIGN KEY (entity_id) REFERENCES owning_entities(entity_id),
    FOREIGN KEY (item_id) REFERENCES stock_items(item_id),
    FOREIGN KEY (location_id) REFERENCES locations(location_id),
    FOREIGN KEY (quality_status_id) REFERENCES quality_status_codes(status_id),

    INDEX idx_inventory_number (inventory_number),
    INDEX idx_entity (entity_id),
    INDEX idx_item (item_id),
    INDEX idx_location (location_id),
    INDEX idx_status (inventory_status),
    INDEX idx_entry_date (entry_date),
    INDEX idx_lot_number (lot_number),
    INDEX idx_serial_number (serial_number)
  */

  public decimal AvailableQuantity => ActualQuantity;
}


/// <summary>
/// Current (or periodic) materialized snapshot for fast queries
/// </summary>
public interface StockPositionSnapshot {
  StockUnit InventoryUnit { init; } //  inventoryunitid  BIGINT PRIMARY KEY REFERENCES inventoryunit(inventory¨C227Cid),
  DateTime AsOfTime { init; }
  StockItem Itemid { init; }    //  BIGINT NOT NULL REFERENCES item(itemid),
  BusinessEntity OwningEntity { init; }  // BIGINT NOT NULL REFERENCES owningentity(owningentityid),
  GeoLocation Location { init; }    // BIGINT REFERENCES location(locationid),
  QualityStatus QualityStatus { init; }// SMALLINT REFERENCES qualitystatus(qualitystatusid),
  decimal OnHandQty => 0;
  decimal AllocatedQty => 0;
  decimal AvailableQty => 0;
  decimal? Unitvalue => null;
  string? Currency => null;
  StockItemHandlingUnit HandlingUnit { init; } //    BIGINT REFERENCES handlingunit(huid),
  decimal? HandlingUnitMaxQty { get; }
  decimal PlannedqQtyInPackage => 0;
  decimal ActualQtyInPackage => 0;
  DateTime UpdatedAt => DateTime.Now;
}
