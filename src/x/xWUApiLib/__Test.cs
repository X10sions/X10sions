using System;

namespace WUApiLib {
  public static class __Test {

    public static IInstallationResult InstallUpdates(UpdateCollection DownloadedUpdates, string query) {
      var UpdateSession = new UpdateSession();
      var InstallAgent = UpdateSession.CreateUpdateInstaller() as UpdateInstaller;
      InstallAgent.Updates = DownloadedUpdates;
      //Starts a synchronous installation of the updates.
      // http://msdn.microsoft.com/en-us/library/windows/desktop/aa386491(v=VS.85).aspx#methods
      return InstallAgent.Install();

    }

    public static UpdateCollection InsallWsusUpdates() {
      //var sSecurePassword = ConvertTo-SecureString –String sPassword –AsPlainText -Force
      //  var oCredential = CreateObject("System.Management.Automation.PSCredential") ' - ArgumentList sUser, sSecurePassword
      var sDateAndTime = DateTime.Now;
      var sCriteria = "IsInstalled=0 and Type='Software'";
      var oSearcher = new UpdateSearcher();
      var oSearchResult = oSearcher.Search(sCriteria).Updates;
      var count = oSearchResult.Count;
      var oSession = new UpdateSession();
      var oDownloader = oSession.CreateUpdateDownloader();
      oDownloader.Updates = oSearchResult;
      oDownloader.Download();
      var oInstaller = new UpdateInstaller();
      oInstaller.Updates = oSearchResult;
      //Result -> 2 = Succeeded, 3 = Succeeded with Errors, 4 = Failed, 5 = Aborted
      return (UpdateCollection)oInstaller.Install();
    }

    public static void Test3() {
      // http://www.nullskull.com/a/1592/install-windows-updates-using-c--wuapi.aspx
      var uSession = new UpdateSessionClass();
      var uSearcher = uSession.CreateUpdateSearcher();
      var uResult = uSearcher.Search("IsInstalled=0 and Type = 'Software'");
      foreach (IUpdate update in uResult.Updates) {
        Console.WriteLine(update.Title);
      }
      var updatesToInstall = new UpdateCollection();
      foreach (IUpdate update in uResult.Updates) {
        if (update.IsDownloaded)
          updatesToInstall.Add(update);
      }
      var installer = uSession.CreateUpdateInstaller();
      installer.Updates = updatesToInstall;
      var installationRes = installer.Install();
      for (int i = 0; i < updatesToInstall.Count; i++) {
        if (installationRes.GetUpdateResult(i).HResult == 0) {
          Console.WriteLine("Installed : " + updatesToInstall[i].Title);
        } else {
          Console.WriteLine("Failed : " + updatesToInstall[i].Title);
        }
      }


    }
    #region https://stackoverflow.com/questions/7639439/using-wua-remotely-using-c-sharp

    static void Main4(string[] args) {
      UpdatesAvailable();
      if (NeedsUpdate()) {
        EnableUpdateServices();//enables everything windows need in order to make an update
        InstallUpdates(DownloadUpdates());
      } else {
        Console.WriteLine("There are no updates for your computer at this time.");
      }
      Console.WriteLine("Press any key to finalize the process");
      Console.Read();
    }
    //this is my first try.. I can see the need for abstract classes here...
    //but at least it gives most people a good starting point.
    public static void InstalledUpdates() {
      var UpdateSession = new UpdateSession();
      var UpdateSearchResult = UpdateSession.CreateUpdateSearcher();
      UpdateSearchResult.Online = true;//checks for updates online
      var SearchResults = UpdateSearchResult.Search("IsInstalled=1 AND IsHidden=0");
      //for the above search criteria refer to
      //http://msdn.microsoft.com/en-us/library/windows/desktop/aa386526(v=VS.85).aspx
      //Check the remarks section
      Console.WriteLine("The following updates are available");
      foreach (IUpdate x in SearchResults.Updates) {
        Console.WriteLine(x.Title);
      }
    }
    public static void UpdatesAvailable() {
      var UpdateSession = new UpdateSession();
      var UpdateSearchResult = UpdateSession.CreateUpdateSearcher();
      UpdateSearchResult.Online = true;//checks for updates online
      var SearchResults = UpdateSearchResult.Search("IsInstalled=0 AND IsPresent=0");
      //for the above search criteria refer to
      //http://msdn.microsoft.com/en-us/library/windows/desktop/aa386526(v=VS.85).aspx
      //Check the remarks section
      foreach (IUpdate x in SearchResults.Updates) {
        Console.WriteLine(x.Title);
      }
    }
    public static bool NeedsUpdate() {
      var UpdateSession = new UpdateSession();
      var UpdateSearchResult = UpdateSession.CreateUpdateSearcher();
      UpdateSearchResult.Online = true;//checks for updates on-line
      var SearchResults = UpdateSearchResult.Search("IsInstalled=0 AND IsPresent=0");
      //for the above search criteria refer to
      //http://msdn.microsoft.com/en-us/library/windows/desktop/aa386526(v=VS.85).aspx
      //Check the remakrs section
      if (SearchResults.Updates.Count > 0)
        return true;
      else return false;
    }
    public static UpdateCollection DownloadUpdates() {
      var UpdateSession = new UpdateSession();
      var SearchUpdates = UpdateSession.CreateUpdateSearcher();
      var UpdateSearchResult = SearchUpdates.Search("IsInstalled=0 and IsPresent=0");
      var UpdateCollection = new UpdateCollection();
      //Accept Eula code for each update
      for (int i = 0; i < UpdateSearchResult.Updates.Count; i++) {
        IUpdate Updates = UpdateSearchResult.Updates[i];
        if (Updates.EulaAccepted == false) {
          Updates.AcceptEula();
        }
        UpdateCollection.Add(Updates);
      }
      //Accept Eula ends here
      //if it is zero i am not sure if it will trow an exception -- I haven't tested it.
      if (UpdateSearchResult.Updates.Count > 0) {
        var DownloadCollection = new UpdateCollection();
        var Downloader = UpdateSession.CreateUpdateDownloader();
        for (int i = 0; i < UpdateCollection.Count; i++) {
          DownloadCollection.Add(UpdateCollection[i]);
        }
        Downloader.Updates = DownloadCollection;
        Console.WriteLine("Downloading Updates... This may take several minutes.");
        var DownloadResult = Downloader.Download();
        var InstallCollection = new UpdateCollection();
        for (int i = 0; i < UpdateCollection.Count; i++) {
          if (DownloadCollection[i].IsDownloaded) {
            InstallCollection.Add(DownloadCollection[i]);
          }
        }
        Console.WriteLine("Download Finished");
        return InstallCollection;
      } else
        return UpdateCollection;
    }
    public static void InstallUpdates(UpdateCollection DownloadedUpdates) {
      Console.WriteLine("Installing updates now...");
      var UpdateSession = new UpdateSession();
      var InstallAgent = UpdateSession.CreateUpdateInstaller() as UpdateInstaller;
      InstallAgent.Updates = DownloadedUpdates;
      //Starts a synchronous installation of the updates.
      // http://msdn.microsoft.com/en-us/library/windows/desktop/aa386491(v=VS.85).aspx#methods
      if (DownloadedUpdates.Count > 0) {
        var InstallResult = InstallAgent.Install();
        if (InstallResult.ResultCode == OperationResultCode.orcSucceeded) {
          Console.WriteLine("Updates installed succesfully");
          if (InstallResult.RebootRequired == true) {
            Console.WriteLine("Reboot is required for one of more updates.");
          }
        } else {
          Console.WriteLine("Updates failed to install do it manually");
        }
      } else {
        Console.WriteLine("The computer that this script was executed is up to date");
      }

    }
    public static void EnableUpdateServices() {
      IAutomaticUpdates updates = new AutomaticUpdates();
      if (!updates.ServiceEnabled) {
        Console.WriteLine("Not all updates services where enabled. Enabling Now" + updates.ServiceEnabled);
        updates.EnableService();
        Console.WriteLine("Service enable success");
      }


    }

    #endregion


  }
}
