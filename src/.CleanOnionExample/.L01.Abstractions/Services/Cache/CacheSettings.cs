namespace CleanOnionExample.Services.Cache;

public class CacheSettings {
  public int AbsoluteExpirationInHours { get; set; }
  public int SlidingExpirationInMinutes { get; set; }
}