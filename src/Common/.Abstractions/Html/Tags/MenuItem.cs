﻿using Common.Attributes;

namespace Common.Html.Tags {
  [ToDo()]
  public class MenuItem : HtmlTag5Base<MenuItem> {
    public Menu Menu { get; set; }

    public override string ToHtml() => throw new NotImplementedException();

    // checked	checked	Specifies that the command/menu item should be checked When the page loads. Only For type="radio" Or type="checkbox"
    // command	 	 
    // Default Marks the command/menu item As being a Default command
    // disabled	disabled	Specifies that the command/menu item should be disabled
    // icon	URL	Specifies an icon For the command/menu item
    // label	text	Required. Specifies the name Of the command/menu item, As shown To the user
    // radiogroup	groupname	Specifies the name Of the group Of commands that will be toggled When the command/menu item itself Is toggled. Only For type="radio"
    // type	checkbox
    // command
    // radio	Specifi

    public enum Menu_Type {
      List,
      Toolbar,
      Context
    }

  }
}