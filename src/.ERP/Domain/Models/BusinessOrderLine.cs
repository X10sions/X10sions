namespace X10sions.ERP.Domain.Models;

public record BusinessOrderLine(long Id) {
  public BusinessOrder BusinessOrder { get; init; }

  public int LineNumber { get; init; }
  public StockItem Item { get; init; }
  public decimal RequestedQuantity { get; init; }
  public decimal ShippedQuantity { get; init; } = 0;
  public decimal ReceivedQuantity { get; init; } = 0;
  public decimal UnitPrice { get; init; }
  public decimal OrderValue => RequestedQuantity * UnitPrice;
  public decimal ShippedValue => ShippedQuantity * UnitPrice;
  public decimal ReceivedValue => ReceivedQuantity * UnitPrice;
  /*
      FOREIGN KEY (po_id) REFERENCES purchase_orders(po_id),
      FOREIGN KEY (item_id) REFERENCES stock_items(item_id),
      INDEX idx_po (po_id),
      INDEX idx_item (item_id)

    FOREIGN KEY (so_id) REFERENCES sales_orders(so_id),
    FOREIGN KEY (item_id) REFERENCES stock_items(item_id),
    INDEX idx_so (so_id),
    INDEX idx_item (item_id)

    FOREIGN KEY (to_id) REFERENCES transit_orders(to_id),
    FOREIGN KEY (item_id) REFERENCES stock_items(item_id),
    INDEX idx_to (to_id),
    INDEX idx_item (item_id)
);"


   */
}
