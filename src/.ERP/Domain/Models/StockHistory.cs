namespace X10sions.ERP.Domain.Models;

/// <summary>Inventory History (Complete Audit Trail) </summary>
public record StockHistory(
  long Id,
  long InventoryId,
  StockHistoryChangeType ChangeType,
  StockHistoryTransactionReferenceType TransactionReferenceType,
  StockHistorySystemSource SystemSource,
  string FieldName,
  object? OldValue = null,
  object? NewValue = null,
  string Changereason = null,
  string changedBy = null,
  DateTime? changeTimestamp = null
  ) {
  public DateTime ChangeTimestamp { get; init; } = changeTimestamp ?? DateTime.UtcNow;// TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  /* 
    FOREIGN KEY (inventory_id) REFERENCES inventory(inventory_id),
    
    INDEX idx_inventory (inventory_id),
    INDEX idx_change_type (change_type),
    INDEX idx_timestamp (change_timestamp),
    INDEX idx_transaction_ref (transaction_reference)
   */
}

public enum StockHistoryChangeType { ENTRY, UPDATE, MOVEMENT, ALLOCATION, EXIT }
public enum EventTypeOption {
  RECEIPT,
  PUTAWAY,
  MOVE,
  ALLOCATE,
  UNALLOCATE,
  PICK,
  CONSUME,
  SHIP,
  ADJUST,
  REVALUE,
  QCCHANGE,
  OWNERSHIPTRANSFER,
  COUNT,
  SPLIT,
  MERGE,
  TRANSITCREATE,
  TRANSITRECEIPT,
  WOOUTPUT,
  WOINPUT
}

public enum StockHistoryTransactionReferenceType { PO, SO, TO, WO }

public enum StockHistorySystemSource { MANUAL, API, BATCH_JOB }

/// <summary>
/// Immutable events capturing full lifecycle changes
/// </summary>
public interface StockEvent {
  int Id { init; }//BIGSERIAL PRIMARY KEY,
  DateTime EventTime { init; }
  EventTypeOption EventType { init; }
  //StockUnit InventoryUnit { init; }// BIGINT NOT NULL REFERENCES inventoryunit(inventoryunitid),
  GeoLocation FromLocation { init; }//BIGINT REFERENCES location(locationid),
  GeoLocation ToLocation { init; }//BIGINT REFERENCES location(locationid),
  decimal QuantityDelta { init; } //NUMERIC(18, 6) NOT NULL, --+ for inbound / increase, - for consumption / ship
  string UOM => string.Empty;
  decimal? ValueDelta => null; // NUMERIC(18, 6), --+or -
  CurrencyCode Currency => CurrencyCode.Undefined;
  QualityStatus QualityStatus { init; }//SMALLINT REFERENCES qualitystatus(qualitystatusid),
  BusinessOrder Order { init; }//REFERENCES orderheader(orderid), --links to PO / SO / TO / WO
  decimal? PlannedQtyInPackage => null;
  decimal? ActualQtyInPackage => null;
  string? Notes => null;
  Person? CreatedBy { init; }
  DateTime CreatedAt => DateTime.Now;

  /*
CREATE INDEX idxeventunittime ON inventoryevent(inventoryunitid, eventtime);
CREATE INDEX idxeventtypetime ON inventoryevent(eventtype, eventtime);
CREATE INDEX idxeventorder ON inventoryevent(orderid); "   

 */
}
