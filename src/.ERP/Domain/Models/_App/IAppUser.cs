namespace X10sions.ERP.Data.Models {
  public interface IAppUser {
    int Id { get; set; }
    IEnumerable<IAppUserGroup> AppUserGroupList { get; set; }
  }

  public interface IAppUserGroup {
    int Id { get; set; }
    int AppUserId { get; set; }
    IAppUser AppUser { get; set; }
  }

  public interface IAppMenu {
    int Id { get; set; }
    IEnumerable<IAppMenuOption> AppMenuOptionList { get; set; }
  }

  public interface IAppMenuOption {
    int Id { get; set; }
    IAppMenu AppMenu { get; set; }
    IAppDisplay AppDisplay { get; set; }
  }

  public interface IAppDisplay {
    int Id { get; set; }
    IEnumerable<IAppMenuOption> AppMenuOptionList { get; set; }
  }


}
