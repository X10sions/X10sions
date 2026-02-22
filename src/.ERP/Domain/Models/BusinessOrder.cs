namespace X10sions.ERP.Domain.Models;

//TradeOrder, BusinessOrder
public record BusinessOrder {

  public BusinessOrder(BusinessEntity customer, GeoLocation customerLocation, BusinessEntity supplier, GeoLocation supplierLocation) {
    Customer = customer;
    CustomerToAddress = customerLocation;
    Supplier = supplier;
    SupplierFromAddress = supplierLocation;
  }
  public static BusinessOrder Purchase(BusinessEntity entity, GeoLocation fromLocation, BusinessEntity supplier, GeoLocation toLocation) => new BusinessOrder(entity, fromLocation, supplier, toLocation);
  public static BusinessOrder Sale(BusinessEntity entity, GeoLocation fromLocation, BusinessEntity customer, GeoLocation toLocation) => new BusinessOrder(customer, toLocation, entity, fromLocation);
  public static BusinessOrder Transit(BusinessEntity entity, GeoLocation origin, GeoLocation destination) => new BusinessOrder(entity, origin, entity, destination);
  public static BusinessOrder Work(BusinessEntity entity, GeoLocation location) => new BusinessOrder(entity, location, entity, location);

  public long Id { get; }
  public DateTime CreatedDate { get; } = DateTime.UtcNow;//TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  public Person CreatedByPerson { get; init; } = default;
  public BusinessOrderStatus Status { get; } = BusinessOrderStatus.DRAFT_PlANNED;


  public BusinessEntity Customer { get; init; }//Origins of the order (Customer for Sales Orders, Supplier for Purchase Orders)
  public BusinessEntity Supplier { get; init; }//Destination of the order (Supplier for Sales Orders, Customer for Purchase Orders)
  public GeoLocation SupplierFromAddress { get; init; }//BIGINT REFERENCES stockroomsite(stockroomsiteid),
  public GeoLocation CustomerToAddress { get; init; }//BIGINT REFERENCES stockroomsite(stockroomsiteid),


  /// <summary> Purchase Order Number </summary>
  public string CustomerReference { get; init; } = string.Empty; //VARCHAR(50) UNIQUE NOT NULL



  public DateTime? SupplierDate { get; init; }
  public DateTime? CustomerDate { get; init; }

  public DateTime? RequestedDeliveryDate { get; init; }
  public DateTime? PromisedDeliveryDate { get; init; }
  public DateTime? PlannedDate { get; init; }
  public DateTime? ActualDate { get; init; }



  /// <summary> Sales Order Number </summary>
  public string SupplierReference { get; init; } = string.Empty; //VARCHAR(50) UNIQUE NOT NULL


  public string TransitReference { get; init; } = string.Empty; //VARCHAR(50) UNIQUE NOT NULL


  public BusinessInvoice? BusinessInvoice { get; set; } = null;

  public decimal? TotalAmount { get; set; } = null;
  public CurrencyCode CurrencyCode { get; set; } = CurrencyCode.Undefined;

  public bool IsWorkOrder => Supplier.Id == Customer.Id && SupplierFromAddress.Id == CustomerToAddress.Id;
  public bool IstransferOrder => Supplier.Id == Customer.Id && SupplierFromAddress.Id != CustomerToAddress.Id;

  public OrderTypeOption OrderType(BusinessEntity entity) => Supplier.Id == Customer.Id ? SupplierFromAddress.Id == CustomerToAddress.Id ? OrderTypeOption.Work : OrderTypeOption.Transfer
    : Supplier.Id == entity.Id ? OrderTypeOption.Sale
    : Customer.Id == entity.Id ? OrderTypeOption.Purchase
    : throw new NotImplementedException();


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