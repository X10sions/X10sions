namespace X10sions.ERP.Domain.Models;

//TradeOrder, BusinessOrder
public record BusinessOrder(long Id) {
  public DateTime CreatedDate { get; init; } = DateTime.UtcNow;//TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  public Person CreatedByPerson { get; init; }


  public BusinessOrderStatus Status { get; init; } = BusinessOrderStatus.DRAFT_PlANNED;


  public BusinessEntity Customer { get; init; }
  /// <summary> Purchase Order Number </summary>
  public string CustomerReference { get; init; } = string.Empty; //VARCHAR(50) UNIQUE NOT NULL
  public GeoLocation CustomerToAddress { get; init; }//BIGINT REFERENCES stockroomsite(stockroomsiteid),


  public DateTime? SupplierDate { get; init; }
  public DateTime? CustomerDate { get; init; }
  public DateTime? RequestedDeliveryDate { get; init; }
  public DateTime? PromisedDeliveryDate { get; init; }
  public DateTime? PlannedDate { get; init; }
  public DateTime? ActualDate { get; init; }



  public BusinessEntity Supplier { get; init; }
  /// <summary> Sales Order Number </summary>
  public string SupplierReference { get; init; } = string.Empty; //VARCHAR(50) UNIQUE NOT NULL
  public GeoLocation SupplierFromAddress { get; init; }//BIGINT REFERENCES stockroomsite(stockroomsiteid),

  public string TransitReference { get; init; } = string.Empty; //VARCHAR(50) UNIQUE NOT NULL


  public BusinessInvoice? BusinessInvoice { get; set; } = null;

  public decimal? TotalAmount { get; set; } = null;
  public CurrencyCode CurrencyCode { get; set; } = CurrencyCode.Undefined;
  public OrderTypeOption OrderType => Supplier.Id == Customer.Id ? OrderTypeOption.Transfer : (Supplier.Id == 0 ? OrderTypeOption.Sale : (Customer.Id == 0 ? OrderTypeOption.Purchase : OrderTypeOption.Work));


  /*
      INDEX idx_po_number (po_number),
      INDEX idx_po_status (po_status),

      INDEX idx_so_number (so_number),
      INDEX idx_so_status (so_status),


    FOREIGN KEY (from_location_id) REFERENCES locations(location_id),
    FOREIGN KEY (to_location_id) REFERENCES locations(location_id),
    INDEX idx_to_number (to_number),
    INDEX idx_to_status (to_status),
    INDEX idx_from_location (from_location_id),
    INDEX idx_to_location (to_location_id)
);"
   
   */

}

public enum CurrencyCode { Undefined, AUD, USD }


public enum BusinessOrderStatus {
  DRAFT_PlANNED,
  APPROVED_CONFIRMED,
  //ALLOCATED,
  PICKING,
  SENT_DISPATCHED_SHIPPED,
  IN_TRANSIT,
  RECEIVED,
  INVOICED,
  CLOSED,
  CANCELLED
}

public enum OrderTypeOption {
  Purchase,
  Sale,
  Transfer,
  Work
}