using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Common.Models.Html.Tags {
  // TODO
  [Obsolete("TODO", false)]
  public class Form : _BaseHtml5 {
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

    [Obsolete("TODO")] public List<object> InputList { get; set; } = new List<object>(); // input)
    [Obsolete("TODO")] public List<object> ButtonList { get; set; } = new List<object>(); // button)
    [Obsolete("TODO")] public List<object> LabelList { get; set; } = new List<object>(); // label)
    [Obsolete("TODO")] public List<object> FieldSetList { get; set; } = new List<object>(); // fieldset)
    public List<OptGroup> OptGroupList { get; set; } = new List<OptGroup>();
    public List<Option> OptionList { get; set; } = new List<Option>();
    public List<Select> SelectList { get; set; } = new List<Select>();
    [Obsolete("TODO")] public List<object> TextAreaList { get; set; } = new List<object>(); // textarea)

    public override string TagName { get; set; } = nameof(Form);

    public override string ToHtml() => throw new NotImplementedException();

    public enum OnOff {
      On,
      Off
    }

    public enum Form_EncType {
      [Description("application/x-www-form-urlencoded")]
      application_x_www_form_urlencoded,
      [Description("multipart/form-data")]
      multipart_form_data,
      [Description("text/plain")]
      text_plain
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
}
