namespace X10sions.ERP.Domain.Models;

/// <summary>
/// Stockroom Locations
/// </summary>
public record GeoLocation(
  int Id, //PRIMARY KEY AUTO_INCREMENT,
  string Code,//VARCHAR(50) UNIQUE NOT NULL,
  string Name,//VARCHAR(200) NOT NULL,
  GeoLocation Parent,  
  GeoLocationStorageCategory StorageCategory = GeoLocationStorageCategory.Undefined,
  GeoLocationType Type = GeoLocationType.Undefined
  ) {
  public int? Capacity { get; init; } = null;
  public bool IsActive { get; init; } = true;
  public DateTime CreatedDate { get; init; } = DateTime.UtcNow;//TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  public decimal? GpsLat { get; init; } = null;
  public decimal? GpsLon { get; init; } = null;
public string TimeZone { get; set; }= string.Empty;

  /*
  UNIQUE(stockroomsiteid, code)

    INDEX idx_location_code (location_code),
    INDEX idx_warehouse (warehouse)
  */

}
public enum GeoLocationType { Undefined, Country, State, City, AddressLine1, Site, Warhouse, Building, Zone, Aisle, Bay, Level, Shelf, Bin }


//StorageCategory, LogisticStage
public enum GeoLocationStorageCategory { Undefined, Storage, Receiving, Shipping, Quarantine }

public static class GeoLocationExtensions {
  
}