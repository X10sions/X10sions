using LinqToDB;
using Microsoft.AspNetCore.Identity;
using System;

namespace Common.AspNetCore.Identity.Providers.LinqToDB {
  public interface IIdentityDataConnection<TKey> : IDataContext where TKey : IEquatable<TKey> {
    ITable<IIdentityRole<TKey>> AspNetRoles { get; set; }
    ITable<IIdentityRoleClaim<TKey>> AspNetRoleClaims { get; set; }
    ITable<IIdentityUser<TKey>> AspNetUsers { get; set; }
    ITable<IIdentityUserClaim<TKey>> AspNetUserClaims { get; set; }
    ITable<IIdentityUserLogin<TKey>> AspNetUserLogins { get; set; }
    ITable<IIdentityUserRole<TKey>> AspNetUserRoles { get; set; }
    ITable<IIdentityUserToken<TKey>> AspNetUserTokens { get; set; }
  }

  //  public abstract class IdentityDataConnection<TKey>
  //    : IdentityUserDataConnection<TKey>
  //    , IIdentityContext_WithUserAndRoles<TKey>
  //    where TKey : IEquatable<TKey> {

  //    public IdentityDataConnection(IDataConnectionOptions options) : base(options) {
  //      OnModelCreating();
  //    }

  //    protected IdentityDataConnection() {
  //      OnModelCreating();
  //    }

  //    public virtual ITable<IIdentityRole<TKey>> AspNetRoles { get; set; }
  //    public virtual ITable<IIdentityRoleClaim<TKey>> AspNetRoleClaims { get; set; }
  //    public virtual ITable<IIdentityUserRole<TKey>> AspNetUserRoles { get; set; }

  //    protected override void OnModelCreating() {
  //      base.OnModelCreating();

  //      FluentMappingBuilder.Entity<IIdentityRole<TKey>>().BuildIdentityRole<IIdentityRole<TKey>, TKey>();
  //      FluentMappingBuilder.Entity<IIdentityRoleClaim<TKey>>().BuildIdentityRoleClaim();

  //      FluentMappingBuilder.Entity<IIdentityUser<TKey>>().BuildIdentityUser<IIdentityUser<TKey>, TKey>(StoreOptions, PersonalDataConverter);
  //      FluentMappingBuilder.Entity<IIdentityUserClaim<TKey>>().BuildIdentityUserClaim();
  //      FluentMappingBuilder.Entity<IIdentityUserLogin<TKey>>().BuildIdentityUserLogin(StoreOptions);
  //      FluentMappingBuilder.Entity<IIdentityUserRole<TKey>>().BuildIdentityUserRole();
  //      FluentMappingBuilder.Entity<IIdentityUserToken<TKey>>().BuildIdentityUserToken<IIdentityUserToken<TKey>, TKey>(StoreOptions, PersonalDataConverter);

  //    }

  //  }

  //  public abstract class IdentityUserDataConnection<TKey> : DataConnection, IIdentityContext_WithUsers<TKey> where TKey : IEquatable<TKey> {

  //    public IdentityUserDataConnection(IDataConnectionOptions options) : base(options.DataProvider, options.ConnectionString) {
  //      DataConnectionOptions = options;
  //      OnModelCreating();
  //    }
  //    protected IdentityUserDataConnection() { }

  //    public IDataConnectionOptions DataConnectionOptions { get; }
  //    public IServiceProvider ServiceProvider => DataConnectionOptions.ServiceProvider;

  //    public virtual ITable<IIdentityUser<TKey>> AspNetUsers { get; set; }
  //    public virtual ITable<IIdentityUserClaim<TKey>> AspNetUserClaims { get; set; }
  //    public virtual ITable<IIdentityUserLogin<TKey>> AspNetUserLogins { get; set; }
  //    public virtual ITable<IIdentityUserToken<TKey>> AspNetUserTokens { get; set; }

  //    protected FluentMappingBuilder FluentMappingBuilder => MappingSchema.GetFluentMappingBuilder();
  //    //protected DefaultPersonalDataProtector PersonalDataConverter => new DefaultPersonalDataProtector(ServiceProvider.GetService<IPersonalDataProtector>(), ServiceProvider.GetService<IPersonalDataProtector>());
  //    protected DefaultPersonalDataProtector PersonalDataConverter => new DefaultPersonalDataProtector(ServiceProvider.GetService<ILookupProtectorKeyRing>(), ServiceProvider.GetService<ILookupProtector>());
  //    protected StoreOptions StoreOptions => GetStoreOptions();

  //    public StoreOptions GetStoreOptions() => ServiceProvider.GetService<IOptions<IdentityOptions>>()?.Value?.Stores;

  //    protected virtual void OnModelCreating() =>
  //        //base.OnModelCreating(builder);
  //        FluentMappingBuilder.BuildIdentityModelsForUserOnly<TKey>(StoreOptions, PersonalDataConverter);
  //  }
}