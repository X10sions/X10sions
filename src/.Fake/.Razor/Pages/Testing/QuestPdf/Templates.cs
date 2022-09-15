using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace X10sions.Fake.Pages.Testing.QuestPdf {
  public class InvoiceDocument : IDocument {
    public InvoiceModel Model { get; }

    public InvoiceDocument(InvoiceModel model) {
      Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container) {
      container.Page(page => {
        page.Margin(50);
        page.Header().Height(100).Background(Colors.Grey.Lighten1);
        page.Content().Background(Colors.Grey.Lighten3);
        page.Footer().Height(50).Background(Colors.Grey.Lighten1);
        page.Footer().AlignCenter().Text(x => {
          x.CurrentPageNumber();
          x.Span(" / ");
          x.TotalPages();
        });
      });
    }

    void ComposeHeader(IContainer container) {
      var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
      container.Row(row => {
        row.RelativeItem().Column(column => {
          column.Item().Text($"Invoice #{Model.InvoiceNumber}").Style(titleStyle);
          column.Item().Text(text => {
            text.Span("Issue date: ").SemiBold();
            text.Span($"{Model.IssueDate:d}");
          });
          column.Item().Text(text => {
            text.Span("Due date: ").SemiBold();
            text.Span($"{Model.DueDate:d}");
          });
        });
        row.ConstantItem(100).Height(50).Placeholder();
      });
    }

    void ComposeContentNotUsed(IContainer container) {
      container
          .PaddingVertical(40)
          .Height(250)
          .Background(Colors.Grey.Lighten3)
          .AlignCenter()
          .AlignMiddle()
          .Text("Content").FontSize(16);
    }

    void ComposeContent(IContainer container) {
      container
        .PaddingVertical(40)
        .Column(column => {
          column.Spacing(5);
          column.Item().Row(row => {
            row.RelativeItem().Component(new AddressComponent("From", Model.SellerAddress));
            row.ConstantItem(50);
            row.RelativeItem().Component(new AddressComponent("For", Model.CustomerAddress));
          });
          column.Item().Element(ComposeTable);
          var totalPrice = Model.Items.Sum(x => x.Price * x.Quantity);
          column.Item().AlignRight().Text($"Grand total: {totalPrice}$").FontSize(14);
          if (!string.IsNullOrWhiteSpace(Model.Comments))
            column.Item().PaddingTop(25).Element(ComposeComments);
        });
    }

    void ComposeComments(IContainer container) {
      container.Background(Colors.Grey.Lighten3).Padding(10).Column(column => {
        column.Spacing(5);
        column.Item().Text("Comments").FontSize(14);
        column.Item().Text(Model.Comments);
      });
    }

    void ComposeTableNotUsed(IContainer container) {
      container
          .Height(250)
          .Background(Colors.Grey.Lighten3)
          .AlignCenter()
          .AlignMiddle()
          .Text("Table").FontSize(16);
    }

    void ComposeTable(IContainer container) {
      container.Table(table => {
        // step 1
        table.ColumnsDefinition(columns => {
          columns.ConstantColumn(25);
          columns.RelativeColumn(3);
          columns.RelativeColumn();
          columns.RelativeColumn();
          columns.RelativeColumn();
        });

        // step 2
        table.Header(header => {
          header.Cell().Element(CellStyle).Text("#");
          header.Cell().Element(CellStyle).Text("Product");
          header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
          header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
          header.Cell().Element(CellStyle).AlignRight().Text("Total");
          static IContainer CellStyle(IContainer container) {
            return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
          }
        });
        // step 3
        foreach (var item in Model.Items) {
          table.Cell().Element(CellStyle).Text(Model.Items.IndexOf(item) + 1);
          table.Cell().Element(CellStyle).Text(item.Name);
          table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price}$");
          table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity);
          table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price * item.Quantity}$");
          static IContainer CellStyle(IContainer container) {
            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
          }
        }
      });
    }


  }

  public class AddressComponent : IComponent {
    private string Title { get; }
    private Address Address { get; }
    public AddressComponent(string title, Address address) {
      Title = title;
      Address = address;
    }

    public void Compose(IContainer container) {
      container.Column(column => {
        column.Spacing(2);
        column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();
        column.Item().Text(Address.CompanyName);
        column.Item().Text(Address.Street);
        column.Item().Text($"{Address.City}, {Address.State}");
        column.Item().Text(Address.Email);
        column.Item().Text(Address.Phone);
      });
    }
  }
}
