﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotion.Linq.Utilities;
using SharpRepository.ODataRepository.Linq.QueryGeneration;
using System.Collections.Generic;
using System.Linq;

namespace SharpRepository.ODataRepository.Linq {
  public class ODataQuery {
    private readonly string _url;
    private readonly string _collectionName;
    private readonly QueryPartsAggregator _queryParts;

    public ODataQuery(string url, string collectionName, QueryPartsAggregator queryParts) {
      _url = url;
      _collectionName = collectionName;
      _queryParts = queryParts;
    }

    public IEnumerable<T> Enumerable<T>() {
      var querystring = string.Empty;
      var resultType = typeof(T);
      var hasFilter = false;

      if (_queryParts.ReturnCount) {
        querystring += "/$count";
      }
      querystring += "?$format=json&";
      if (_queryParts.Take.HasValue)
        querystring += "$top=" + _queryParts.Take.Value + "&";
      if (_queryParts.Skip.HasValue)
        querystring += "$skip=" + _queryParts.Skip.Value + "&";
      if (!string.IsNullOrEmpty(_queryParts.OrderBy))
        querystring += "$orderby=" + _queryParts.OrderBy + "&";
      var filter = SeparatedStringBuilder.Build(" and ", _queryParts.WhereParts);
      if (!string.IsNullOrEmpty(filter))
        querystring += "$filter=" + filter + "&";
      if (!string.IsNullOrEmpty(_queryParts.SelectPart))
        querystring += "$select=" + _queryParts.SelectPart + "&";
      var fullUrl = _url + "/" + _collectionName + querystring;
      var json = UrlHelper.Get(fullUrl);
      // Netflix retuns a separate array inside d when a filter is used for some reason, so hard-coded check for now during tests
      hasFilter = !querystring.EndsWith("$format=json&");
      //var json = ODataRequest.Execute(fullUrl, "POST", _queryParts.BuildODataApiPostData(), "application/json");
      JObject res;
      // check for Count() [Int32] and LongCOunt() [Int64]
      if (_queryParts.ReturnCount && (resultType == typeof(int) || resultType == typeof(long))) {
        var results = new List<T>();
        res = JObject.Parse(json);
        results.Add(res["total_rows"].ToObject<T>());
        return results;
      }
      // get the rows property and deserialize that
      var jobject = JsonConvert.DeserializeObject(json) as JObject;
      var rows = jobject["d"];
      if (hasFilter) {
        rows = rows["results"];
      }
      var items = rows.Select(row => row.ToObject<T>());
      return items;
    }
  }

}