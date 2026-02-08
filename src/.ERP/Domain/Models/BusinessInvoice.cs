
namespace X10sions.ERP.Domain.Models;

public record BusinessInvoice(long Id) {
  public BusinessOrder? BusinessOrder { get; init; } = null;
  public DateTime CreatedDate { get; init; } = DateTime.UtcNow;//TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  public bool IsConfirmed { get; init; } = false;
  public BusinessInvoiceStatus Status { get; init; } = BusinessInvoiceStatus.None;
  public BusinessEntity Customer { get; init; }
  public string CustomerInvoiceReference { get; init; } = string.Empty; //VARCHAR(50) UNIQUE NOT NULL
  public BusinessEntity Supplier { get; init; }
  public string SupplierInvoiceReference { get; init; } = string.Empty; //VARCHAR(50) UNIQUE NOT NULL

  /*
    INDEX idx_po_status (po_status),
    INDEX idx_invoice_confirmed (invoice_confirmed)
  */

}

public enum BusinessInvoiceStatus {
  None,
  Invoiced,
  PartiallyInvoiced
}
