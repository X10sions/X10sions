namespace X10sions.Fake;
  public static class DomainExtensions {
    public static int? GetWholeYearsBetween(this DateTime? minDate, DateTime maxDate) => (minDate == null) ? null : maxDate.Year - minDate.Value.Year - ((maxDate.Month < minDate.Value.Month || (maxDate.Month == minDate.Value.Month && maxDate.Day < minDate.Value.Day)) ? 1 : 0);

}
