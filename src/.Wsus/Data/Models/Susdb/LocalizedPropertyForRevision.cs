using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X10sions.Wsus.Data.Models.Susdb;

[Table("tbLocalizedPropertyForRevision")]
public class LocalizedPropertyForRevision {
  [Column(Order = 0), Key] public int RevisionID { get; set; }
  [Column(Order = 1), Key] public int LocalizedPropertyID { get; set; }
  [Column(Order = 2), Key] public int LanguageID { get; set; }

  // ALTER TABLE[dbo].[tbLocalizedPropertyForRevision] WITH CHECK ADD FOREIGN KEY([LanguageID]) REFERENCES[dbo].[tbLanguage]   ([LanguageID])
  // ALTER TABLE[dbo].[tbLocalizedPropertyForRevision] WITH CHECK ADD FOREIGN KEY([LocalizedPropertyID]) REFERENCES[dbo].[tbLocalizedProperty]  ([LocalizedPropertyID])
  // ALTER TABLE[dbo].[tbLocalizedPropertyForRevision] WITH CHECK ADD FOREIGN KEY([RevisionID])REFERENCES[dbo].[tbRevision]  ([RevisionID])

}

