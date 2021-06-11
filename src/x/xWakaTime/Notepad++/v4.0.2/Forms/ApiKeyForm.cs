using System;
using System.Windows.Forms;

namespace WakaTime.Forms {
  public partial class ApiKeyForm : Form {
    readonly ConfigFile _wakaTimeConfigFile;

    public ApiKeyForm() {
      InitializeComponent();

      _wakaTimeConfigFile = new ConfigFile();
    }

    void ApiKeyForm_Load(object sender, EventArgs e) {
      try {
        txtAPIKey.Text = _wakaTimeConfigFile.ApiKey;
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }

    void btnOk_Click(object sender, EventArgs e) {
      try {
        var parse = Guid.TryParse(txtAPIKey.Text.Trim(), out var apiKey);
        if (parse) {
          _wakaTimeConfigFile.ApiKey = apiKey.ToString();
          _wakaTimeConfigFile.Save();
          WakaTimePackage.ApiKey = apiKey.ToString();
        } else {
          MessageBox.Show("Please enter valid Api Key.");
          DialogResult = DialogResult.None; // do not close dialog box
        }
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }
  }
}
