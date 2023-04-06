using System.Text;

namespace Common.Html.Tags;
public class Table : HtmlTag5Base<Table> {
  public Caption Caption { get; set; } = new Caption();
  public ColGroup? ColGroup { get; set; }
  public THead? THead { get; set; }
  public List<TBody> TBody { get; set; } = new List<TBody>();
  public TFoot? TFoot { get; set; }

  public override string ToHtml() {
    var sb = new StringBuilder();
    sb.AppendLine($"<{TagName}>");
    sb.AppendLine(Caption.ToHtml());
    if (ColGroup != null) sb.AppendLine(ColGroup.ToHtml());
    if (THead != null) sb.AppendLine(THead.ToHtml());
    foreach (var tbody in TBody) {
      sb.AppendLine(tbody.ToHtml());
    }
    if (TFoot != null) sb.AppendLine(TFoot.ToHtml());
    sb.AppendLine($"</{TagName}>");
    return sb.ToString();
  }
}


public class ColGroup : HtmlTag5Base<ColGroup> {
  public override string ToHtml() => throw new NotFiniteNumberException("https://www.w3schools.com/tags/tag_colgroup.asp");
}

public class Caption : HtmlTag5Base<Caption> {
  public string InnerText { get; set; }
  public override string ToHtml() => $"<{TagName}{InnerText}></{TagName}>";
}

#region TableRows

public interface ITableRows {
  public List<TR> TR { get; set; }
}

public static class ITableRowsExtensions {
  public static string TableRowToHtml(this ITableRows tableRows) {
    var sb = new StringBuilder();
    foreach (var tr in tableRows.TR) {
      sb.AppendLine(tr.ToHtml());
    }
    return sb.ToString();
  }

}

public class TBody : HtmlTag5Base<TBody>, ITableRows {
  public List<TR> TR { get; set; } = new List<TR>();

  public override string ToHtml() => $"<{TagName}>{this.TableRowToHtml()}</{TagName}>";

}

public class THead : HtmlTag5Base<THead>, ITableRows {
  public List<TR> TR { get; set; } = new List<TR>();
  public override string ToHtml() => $"<{TagName}>{this.TableRowToHtml()}</{TagName}>";

}

public class TFoot : HtmlTag5Base<TFoot>, ITableRows {
  public List<TR> TR { get; set; } = new List<TR>();
  public override string ToHtml() => $"<{TagName}>{this.TableRowToHtml()}</{TagName}>";
}

#endregion

public class TR : HtmlTag5Base<TR> {

  public List<ITableCellHtmlTag> Cells { get; set; } = new List<ITableCellHtmlTag>();

  public override string ToHtml() {
    var sb = new StringBuilder();
    sb.AppendLine($"<{TagName}");
    sb.Append(">");
    foreach (var cell in Cells) {
      sb.AppendLine(cell.ToHtml());
    }
    sb.AppendLine($"</{TagName}>");
    return sb.ToString();
  }

  #region TableCell

  public interface ITableCellHtmlTag : IHtmlTag {
    public string InnerHtml { get; set; }
  }

  public abstract class TableCellHtmlTagBase<T> : HtmlTag5Base<T>, ITableCellHtmlTag where T : IHtmlTag {
    public string InnerHtml { get; set; } = string.Empty;
  }

  public class TD : TableCellHtmlTagBase<TD> {
    public override string ToHtml() => $"<{TagName}>{InnerHtml}</{TagName}>";
  }

  public class TH : TableCellHtmlTagBase<TH> {
    public override string ToHtml() => $"<{TagName}>{InnerHtml}</{TagName}>";
  }

  #endregion

}
