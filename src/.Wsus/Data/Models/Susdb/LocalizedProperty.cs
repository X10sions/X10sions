using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbLocalizedProperty")]
public class LocalizedProperty {
  [Column] public int LocalizedPropertyID { get; set; }
  [Column] public string Title { get; set; }
  [Column] public string? Description { get; set; }
  [Column] public string? ReleaseNote { get; set; }
  // PRIMARY KEY CLUSTERED (  [LocalizedPropertyID] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON[PRIMARY]) ON[PRIMARY]
}