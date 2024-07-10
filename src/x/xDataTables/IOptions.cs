using Newtonsoft.Json;
using System.Net.Http;

namespace DataTables {
  public interface IOptions {
    // https://datatables.net/reference/option/

    public bool InitDataTables { get; set; }

    #region _Default
    public int? DeferLoading { get; set; }
    public bool? Destroy { get; set; }
    public int? DisplayStart { get; set; }
    public string? Dom { get; set; }
    public ListMenuItem[]? LengthMenu { get; set; }//[ [10, 25, 50, -1], [10, 25, 50, "All"] ]
    public OrderItem[]? Order { get; set; }//[ [0, 'asc'] ]
    public bool? OrderCellsTop { get; set; }
    public bool? OrderClasses { get; set; }//=true;
    public OrderItem[]? OrderFixed { get; set; }//[ [0, 'asc'] ]
    public bool? OrderMulti { get; set; }//=true;
    public int? PageLength { get; set; }//=10;
    public PagingTypes? PagingType { get; set; }//=simple_numbers;
    //public Renderer? renderer { get; set; }
    public bool? Retrieve { get; set; }
    public string? RowId { get; set; } //= "DT_RowId";
    public bool? ScrollCollapse { get; set; }
    public Search? search { get; set; }
    public Search[]? searchCols { get; set; }
    public int? SearchDelay { get; set; }
    public int? StateDuration { get; set; }//=7200;
    public string[]? StripeClasses { get; set; }//= new[]{"stripe1", "stripe2", "stripe3"};
    public int? TabIndex { get; set; }
    #endregion

    #region Features

    public bool? AutoWidth { get; set; } //= true;
    public bool? DeferRender { get; set; }
    public bool? Info { get; set; } //= true;
    public bool? LengthChange { get; set; } //= true;
    public bool? Ordering { get; set; } //= true;
    public bool? Paging { get; set; } //= true;
    public bool? Processing { get; set; }
    public bool? ScrollX { get; set; }
    public string? ScrollY { get; set; }
    public bool? Searching { get; set; } //= true;
    public bool? ServerSide { get; set; }
    public bool? StateSave { get; set; }
    #endregion

    #region Data
    public Ajax? ajax { get; set; }
    public object Data { get; set; }
    #endregion

    #region CallBacks
    /*
     createdRow
drawCallback
footerCallback
formatNumber
headerCallback
infoCallback
initComplete
preDrawCallback
rowCallback
stateLoadCallback
stateLoadParams
stateLoaded
stateSaveCallback
stateSaveParams
     */
    #endregion

    #region Columns
    public Column[]? ColumnDefs { get; set; }
    public Column[]? Columns { get; set; }

    public class Column {
      public string? cellType { get; set; }//="td";
      public string? className { get; set; }
      public string? contentPadding { get; set; }
      public object? createdCell { get; set; }
      public object? data { get; set; }
      public string? defaultContent { get; set; }
      public string? name { get; set; }
      public bool? orderable { get; set; }//=true;
      public int[]? orderData { get; set; }
      public string? orderDataType { get; set; }
      public OrderDirection[]? orderSequence { get; set; }
      public Render[]? render { get; set; }
      public bool? searchable { get; set; }//=true;
      public string? title { get; set; }
      public string? type { get; set; }
      public int[]? targets { get; set; }
      public bool? visible { get; set; }//=true;
      public string? width { get; set; }
    }

    public class Render {
      public string? _ { get; set; }
      public string? filter { get; set; }
      public string? display { get; set; }
    }

    #endregion

    #region Internationalisation, ColReorder, FixedColumns, FixedHeader, KeyTable, Responsive, RowGroup, RowReorder,  Scroller, SearchBuilder,  SearchPanes, Select
    // TODO
    #endregion

    public enum OrderDirection {
      asc,
      desc
    }

    public class OrderItem {
      public OrderItem(int columnIndex, OrderDirection direction = OrderDirection.asc) {
        ColumnIndex = columnIndex;
        Direction = direction;
      }

      public OrderDirection Direction { get; set; }
      public int ColumnIndex { get; set; }
    }

    public class ListMenuItem {
      public ListMenuItem(int value, string? text = null) {
        Value = value;
        Text = text ?? value.ToString();
      }

      public string Text { get; set; }
      public int Value { get; set; }
    }

    public enum PagingTypes {
      simple_numbers,
      numbers,
      simple,
      full,
      full_numbers,
      first_last_numbers
    }

    public class Renderer {
      public string Header { get; set; } = "jqueryui";
      public string PageButton { get; set; } = "bootstrap";
    }

    public class Search {
      public Search(string value) {
        search = value;
      }
      public bool? CaseInsensitive { get; set; }//=true;
      public bool? Regex { get; set; }
      public string search { get; set; }
      public bool? Smart { get; set; }//=true;
    }

    public class Ajax {
      public Ajax(string url, HttpMethod? type = null) {
        Url = url;
        Type = type;
      }

      public object Callback { get; set; }
      public string? ContentType { get; set; }
      public object Data { get; set; }
      public string? DataSrc { get; set; }
      public string Url { get; set; }
      public HttpMethod? Type { get; set; }

    }

  }

  public static class IOptionsExtensions {
    // https://datatables.net/reference/option/

    public static string ToCssString(this IOptions options) => options.InitDataTables ? " data-tables-init" : string.Empty;

    public static string ToDataHtmlAttributesString(this IOptions options)
      =>  JsonConvert.SerializeObject(options, DataTablesJsonSerializerSettings.Instance).Replace(":", "=").Replace(";", " ");

  }

}