﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using xDamienBod.AppAuthorizationService;

namespace xDamienBod.MvcHybridClient {
  public class MvcHybridClientStartup {
    public MvcHybridClientStartup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    string stsServer = "https://localhost:44364";

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.Configure<CookiePolicyOptions>(options => {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });

      services.AddAuthentication(options => {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
      })
      .AddCookie()
      .AddOpenIdConnect(options => {
        options.SignInScheme = "Cookies";
        options.SignOutScheme = "OpenIdConnect";
        options.Authority = stsServer;
        options.RequireHttpsMetadata = true;
        options.ClientId = "hybridclient";
        options.ClientSecret = "hybrid_flow_secret";
        options.ResponseType = "code id_token";
        options.GetClaimsFromUserInfoEndpoint = true;
        options.Scope.Add("scope_used_for_hybrid_flow");
        options.Scope.Add("profile");
        options.Scope.Add("offline_access");
        options.SaveTokens = true;
        // Set the correct name claim type
        options.TokenValidationParameters = new TokenValidationParameters {
          NameClaimType = "name"
        };
      });

      services.AddSingleton<IAppAuthorizationService, AppAuthorizationService.AppAuthorizationService>();
      services.AddSingleton<IAuthorizationHandler, IsAdminAuthorizationHandler>();
      services.AddSingleton<IAuthorizationHandler, BobIsAnAdminAuthorizationHandler>();
      services.AddAuthorization(options => {
        options.AddPolicy("RequireWindowsProviderPolicy", MyPolicies.GetRequireWindowsProviderPolicy());
        options.AddPolicy("IsAdminRequirementPolicy", policyIsAdminRequirement => {
          policyIsAdminRequirement.Requirements.Add(new IsAdminRequirement());
        });
      });
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      } else {
        app.UseExceptionHandler("/Shared/Error");
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseCookiePolicy();
      app.UseAuthentication();
      app.UseMvcWithDefaultRoute();

    }
  }
}
