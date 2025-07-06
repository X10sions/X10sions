﻿namespace RCommon.Collections;

public interface IPaginatedList<T> : IList<T> {
  bool HasNextPage { get; }
  bool HasPreviousPage { get; }
  int PageIndex { get; }
  int PageSize { get; }
  int TotalCount { get; }
  int TotalPages { get; }
}
