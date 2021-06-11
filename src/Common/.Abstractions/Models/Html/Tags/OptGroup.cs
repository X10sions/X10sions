using System;
using System.Collections.Generic;

namespace Common.Models.Html.Tags {
  // TODO
  public class OptGroup : _BaseHtml5 {
    public bool? Disabled { get; set; }
    public string Label { get; set; } // text	Specifies a shorter label For an Option
    public List<Option> OptionList { get; set; } = new List<Option>();

    public override string TagName { get; set; } = nameof(OptGroup);

    public override string ToHtml() => throw new NotImplementedException();

  }
}
