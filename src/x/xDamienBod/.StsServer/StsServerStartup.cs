using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using xDamienBod.StsServer.Data;
using xDamienBod.StsServer.Models;

namespace xDamienBod.StsServer {
  public class StsServerStartup {
    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    public StsServerStartup(IConfiguration configuration, IWebHostEnvironment environment) {
      Configuration = configuration;
      Environment = environment;
    }

    public void ConfigureServices(IServiceCollection services) {
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

      services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

      services.AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      services.Configure<IISOptions>(iis => {
        iis.AuthenticationDisplayName = "Windows";
        iis.AutomaticAuthentication = true;
      });

      var identityServerBuilder = services.AddIdentityServer(options => {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
      });
      identityServerBuilder.AddInMemoryIdentityResources(Config.GetIdentityResources());
      identityServerBuilder.AddInMemoryApiResources(Config.GetApis());
      identityServerBuilder.AddInMemoryClients(Config.GetClients());
      identityServerBuilder.AddAspNetIdentity<ApplicationUser>();

      if (Environment.IsDevelopment()) {
        identityServerBuilder.AddDeveloperSigningCredential();
      } else {
        throw new Exception("need to configure key material");
      }

    }

    public void Configure(IApplicationBuilder app) {
      if (Environment.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      } else {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();
      app.UseIdentityServer();
      app.UseMvcWithDefaultRoute();
    }
  }
}
