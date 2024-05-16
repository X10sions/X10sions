using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbCategory")]
public class Category {
  [Column] public int CategoryIndex { get; set; }
  [Column] public int CategoryID { get; set; }
  [Column] public int? ParentCategoryID { get; set; }
  [Column] public string CategoryType { get; set; } = string.Empty;
  [Column] public DateTime LastChange { get; set; } = DateTime.UtcNow;
  [Column] public bool ProhibitsSubcategories { get; set; }
  [Column] public bool ProhibitsUpdates { get; set; }
  [Column] public int? DisplayOrder { get; set; }

  //PRIMARY KEY CLUSTERED(  [CategoryID] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]) ON[PRIMARY]
  //ALTER TABLE[dbo].[tbCategory] WITH CHECK ADD FOREIGN KEY([CategoryType]) REFERENCES[dbo].[tbCategoryType]  ([CategoryType])
  //ALTER TABLE[dbo].[tbCategory] WITH CHECK ADD FOREIGN KEY([CategoryID]) REFERENCES[dbo].[tbUpdate]   ([LocalUpdateID])
  //ALTER TABLE[dbo].[tbCategory] WITH CHECK ADD FOREIGN KEY([ParentCategoryID]) REFERENCES[dbo].[tbCategory]   ([CategoryID])
}

