namespace X10sions.ERP.Domain.Models;

/// <summary>
/// Inventory unit - if you track at HU level, this can be 1:1 with HU.
/// </summary>
public interface StockUnit {
  int Id { init; }//BIGSERIAL PRIMARY KEY,

  StockItemHandlingUnit HandlingUnit { init; } //    BIGINT REFERENCES handlingunit(huid),
  StockItem Itemid { init; }    //  BIGINT NOT NULL REFERENCES item(itemid),
public  BusinessEntity OwningEntity { get;  init; }  // BIGINT NOT NULL REFERENCES owningentity(owningentityid),
  DateTime CreatedAt => DateTime.Now;
}

