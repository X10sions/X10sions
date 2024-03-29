﻿using Common.Attributes;
using System.ComponentModel;

namespace Common.Html.Tags;
[ToDo]
public class Form : HtmlTag5Base<Form> {
  // WARNING: THIS is a test warning
  // ERROR: test error
  // FIX: test fix
  // UNDONE: test undonde
  // HACK: this is a hack
  // NOTE: test note
  public string accept { get; set; } // file_type'	Not supported In HTML5. Specifies a comma-separated list Of file types  that the server accepts (that can be submitted through the file upload)
  [Description("accept-charset")]
  public string AcceptCharset { get; set; } // (https://www.w3schools.com/TAGs/ref_charactersets.asp) 'Specifies the character encodings that are To be used For the form submission
  public string action { get; set; }  // Specifies where To send the form-data When a form Is submitted
  public OnOff? autocomplete { get; set; } // Specifies whether a form should have autocomplete On Or off
  public Form_EncType? EncType { get; set; } // Specifies how the form-data should be encoded When submitting it To the server (only For method="post")
  public Form_Method? method { get; set; } // Specifies the HTTP method To use When sending form-data
  public string name { get; set; }  // Specifies the name Of a form
  public bool? novalidate { get; set; }  // Specifies that the form should Not be validated When submitted
  public Form_Target? target { get; set; } // Specifies where To display the response that Is received

  [ToDo] public List<object> InputList { get; set; } = new List<object>(); // input)
  [ToDo] public List<object> ButtonList { get; set; } = new List<object>(); // button)
  [ToDo] public List<object> LabelList { get; set; } = new List<object>(); // label)
  [ToDo] public List<object> FieldSetList { get; set; } = new List<object>(); // fieldset)
  public List<OptGroup> OptGroupList { get; set; } = new List<OptGroup>();
  public List<Option> OptionList { get; set; } = new List<Option>();
  public List<Select> SelectList { get; set; } = new List<Select>();
  [ToDo()] public List<object> TextAreaList { get; set; } = new List<object>(); // textarea)

  public override string ToHtml() => throw new NotImplementedException();

  public enum OnOff {
    On,
    Off
  }

  public enum Form_EncType {
    [Description("application/x-www-form-urlencoded")] application_x_www_form_urlencoded,
    [Description("multipart/form-data")] multipart_form_data,
    [Description("text/plain")] text_plain
  }

  public enum Form_Method {
    Get,
    Post
  }

  public enum Form_Target {
    _blank,
    _self,
    _parent,
    _top
  }

}
