namespace X10sions.ERP.Domain.Models;

/// <summary>Allocations table (supports multiple allocations per unit)</summary>
public class StockAllocation {
  public int Id { get; init; }
  
  public Stock Stock { get; init; }
  //inventory_id   public StockUnit InventoryUnit { get; init; }
  public BusinessOrderLine OrderLine { get; init; }
  public StockAllocationType AllocationType { get; init; }
  public decimal AllocatedQuantity { get; init; }
  public decimal PickedQuantity { get; init; } = 0;
  public decimal ShippedQuantity { get; init; } = 0;
  public StockAllocationStatus Status { get; init; }
  public DateTime CreatedAt { get; init; } = DateTime.Now;
  public DateTime UpdatedAt { get; init; } = DateTime.Now;
  /*
  CREATE INDEX idxallocunit ON inventoryallocation(inventoryunitid);
  CREATE INDEX idxallocorder ON inventoryallocation(order_id); "    
  
    FOREIGN KEY (po_line_id) REFERENCES purchase_order_lines(po_line_id),
    FOREIGN KEY (so_line_id) REFERENCES sales_order_lines(so_line_id),
    FOREIGN KEY (inventory_id) REFERENCES inventory(inventory_id)

    FOREIGN KEY (to_id) REFERENCES transit_orders(to_id),
    FOREIGN KEY (to_line_id) REFERENCES transit_order_lines(to_line_id),
    FOREIGN KEY (inventory_id) REFERENCES inventory(inventory_id),
    INDEX idx_to (to_id),
    INDEX idx_inventory (inventory_id)
   */
}

public enum StockAllocationType {  PO,  SO,  TO, WOINPUT,  WOOUTPUT}

public enum StockAllocationStatus {  Planned,  Firm,  Picked,  Consumed,  Shipped,  Canceled}
