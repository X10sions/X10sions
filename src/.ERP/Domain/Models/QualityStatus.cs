namespace X10sions.ERP.Domain.Models;

/// <summary>
/// Quality Control Status Codes
/// </summary>
public record QualityStatus(
  int Id, //PRIMARY KEY AUTO_INCREMENT,
  string Code,//VARCHAR(50) UNIQUE NOT NULL,
  string Name,//VARCHAR(100) NOT NULL,
  string? Description = null,
  bool AllowShipping = false,
  bool AllowProduction = false,
  int? DisplayOrder = null,
  bool IsActive = true
  ) {

  public static readonly QualityStatus Pending = new QualityStatus(1, "PENDING", "Pending Inspection", null, false, false, 1, true);
  public static readonly QualityStatus Approved = new QualityStatus(2, "APPROVED", "Approved for Use", null, true, true, 2, true);
  public static readonly QualityStatus Quarantine = new QualityStatus(3, "QUARANTINE", "Quarantine Hold", null, false, false, 3, true);
  public static readonly QualityStatus Rejected = new QualityStatus(4, "REJECTED", "Rejected", null, false, false, 4, true);
  public static readonly QualityStatus Rework = new QualityStatus(5, "REWORK", "Requires Rework", null, false, false, 5, true);

  /*
    INDEX idx_status_code (status_code)

  -- Insert standard quality statuses
  INSERT INTO quality_status_codes (status_code, status_name, allow_shipping, allow_production, display_order) VALUES
  ('PENDING', 'Pending Inspection', FALSE, FALSE, 1),
  ('APPROVED', 'Approved for Use', TRUE, TRUE, 2),
  ('QUARANTINE', 'Quarantine Hold', FALSE, FALSE, 3),
  ('REJECTED', 'Rejected', FALSE, FALSE, 4),
  ('REWORK', 'Requires Rework', FALSE, FALSE, 5);"

  */
}