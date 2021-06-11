using System;
using System.Collections.Generic;

namespace OfficeOpenXml {

  public static class OfficePropertiesExtensions {

    public static void Clone(this OfficeProperties props, OfficeProperties values) {
      props.Application = values.Application;
      props.AppVersion = values.AppVersion;
      props.Author = values.Author;
      props.Category = values.Category;
      props.Comments = values.Comments;
      props.Company = values.Company;
      props.Created = values.Created;
      props.HyperlinkBase = values.HyperlinkBase;
      props.HyperlinksChanged = values.HyperlinksChanged;
      props.Keywords = values.Keywords;
      props.LastModifiedBy = values.LastModifiedBy;
      props.LastPrinted = values.LastPrinted;
      props.LinksUpToDate = values.LinksUpToDate;
      props.Manager = values.Manager;
      props.Modified = values.Modified;
      props.ScaleCrop = values.ScaleCrop;
      props.SharedDoc = values.SharedDoc;
      props.Status = values.Status;
      props.Subject = values.Subject;
      props.Title = values.Title;
    }

    public static Dictionary<string, object> ToDictionary(this OfficeProperties props) {
      var dic = new Dictionary<string, object>();
      dic[nameof(props.Application)] = props.Application;
      dic[nameof(props.AppVersion)] = props.AppVersion;
      dic[nameof(props.Author)] = props.Author;
      dic[nameof(props.Category)] = props.Category;
      dic[nameof(props.Comments)] = props.Comments;
      dic[nameof(props.Company)] = props.Company;
      dic[nameof(props.Created)] = props.Created;
      dic[nameof(props.HyperlinkBase)] = props.HyperlinkBase;
      dic[nameof(props.HyperlinksChanged)] = props.HyperlinksChanged;
      dic[nameof(props.Keywords)] = props.Keywords;
      dic[nameof(props.LastModifiedBy)] = props.LastModifiedBy;
      dic[nameof(props.LastPrinted)] = props.LastPrinted;
      dic[nameof(props.LinksUpToDate)] = props.LinksUpToDate;
      dic[nameof(props.Manager)] = props.Manager;
      dic[nameof(props.Modified)] = props.Modified;
      dic[nameof(props.ScaleCrop)] = props.ScaleCrop;
      dic[nameof(props.SharedDoc)] = props.SharedDoc;
      dic[nameof(props.Status)] = props.Status;
      dic[nameof(props.Subject)] = props.Subject;
      dic[nameof(props.Title)] = props.Title;
      return dic;
    }





  }
}