using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Entities.Services;
using CleanOnionExample.Infrastructure.Factories;
using CleanOnionExample.Services;
using CleanOnionExample.Services.Cache;
using CleanOnionExample.Services.JWT;
using CleanOnionExample.Services.Mail;
using Common.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using System.Text.Json;
using X10sions.Fake.Features.ToDo.Item;

namespace Microsoft.AspNetCore.Builder;

public static class ConfigureServiceContainer {


  public static void AddApplicationDbContext(this IServiceCollection services, string connectinString)
    => services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectinString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

  public static void AddAutoMapper(this IServiceCollection serviceCollection) {
    var mappingConfig = new MapperConfiguration(mc => {
      mc.AddProfile(new CustomerProfile());
    });
    IMapper mapper = mappingConfig.CreateMapper();
    serviceCollection.AddSingleton(mapper);
  }

  public static void AddScopedServices(this IServiceCollection serviceCollection) {
    serviceCollection.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
  }

  public static void AddTransientServices(this IServiceCollection serviceCollection) {
    serviceCollection.AddTransient<IDateTimeService, DateTimeService>();
    serviceCollection.AddTransient<IAccountService1, AccountService1>();
  }

  public static void AddSwaggerOpenAPI(this IServiceCollection serviceCollection) {
    serviceCollection.AddSwaggerGen(setupAction => {
      setupAction.SwaggerDoc("OpenAPISpecification", new OpenApiInfo() {
        Title = "CleanOnionExample WebAPI",
        Version = "1",
        Description = "Through this API you can access customer details",
        Contact = new OpenApiContact() {
          Email = "tb@CleanOnionExample.com.au",
          Name = "CleanOnionExampleWeb",
          Url = new Uri("https://intranet.marotg.com.au/api/")
        },
        License = new OpenApiLicense() {
          Name = "MIT License",
          Url = new Uri("https://opensource.org/licenses/MIT")
        }
      });
      setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = $"Input your Bearer token in this format - Bearer token to access this API",
      });
      setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
          new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer",
            },
          }, new List<string>()
        },
      });
    });
  }

  public static void AddVersion(this IServiceCollection serviceCollection) {
    serviceCollection.AddApiVersioning(config => {
      config.DefaultApiVersion = new ApiVersion(1, 0);
      config.AssumeDefaultVersionWhenUnspecified = true;
      config.ReportApiVersions = true;
    });
  }

  public static void AddHealthCheck<T>(this IServiceCollection serviceCollection, IOptionsMonitor<T> appSettingsOptions, IConfiguration configuration) where T : class, IApplicationDetail {
    serviceCollection.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>("Application DB Context", HealthStatus.Degraded)
        .AddUrlGroup(new Uri(appSettingsOptions.CurrentValue.ApplicationDetail.ContactWebsite), "My personal website", HealthStatus.Degraded)
        .AddSqlServer(configuration.GetConnectionString("OnionArchConn"));

    serviceCollection.AddHealthChecksUI(setupSettings: setup => {
      setup.AddHealthCheckEndpoint("Basic Health Check", $"/healthz");
    }).AddInMemoryStorage();
  }

  public static void AddServiceLayer(this IServiceCollection services) {
    // or you can use assembly in Extension method in Infra layer with below command
    services.AddMediatR(Assembly.GetExecutingAssembly());
    services.AddTransient<IEmailService, MailService>();
  }

  public static void AddIdentityService(this IServiceCollection services, IConfiguration configuration) {
    if (configuration.GetValue<bool>("UseInMemoryDatabase")) {
      services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityDb"));
    } else {
      services.AddDbContext<IdentityContext>(options =>
      options.UseSqlServer(
          configuration.GetConnectionString("IdentityConnection"),
          b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
    }
    services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders();
    #region Services
    services.AddTransient<IAccountService1, AccountService1>();
    #endregion
    services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
    services.AddAuthentication(options => {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(o => {
          o.RequireHttpsMetadata = false;
          o.SaveToken = false;
          o.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = configuration["JWTSettings:Issuer"],
            ValidAudience = configuration["JWTSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
          };
          o.Events = new JwtBearerEvents() {
            OnAuthenticationFailed = c => {
              c.NoResult();
              c.Response.StatusCode = 500;
              c.Response.ContentType = "text/plain";
              return c.Response.WriteAsync(c.Exception.ToString());
            },
            OnChallenge = context => {
              context.HandleResponse();
              context.Response.StatusCode = 401;
              context.Response.ContentType = "application/json";
              var result = JsonSerializer.Serialize(new Response<string>("You are not Authorized"));
              return context.Response.WriteAsync(result);
            },
            OnForbidden = context => {
              context.Response.StatusCode = 403;
              context.Response.ContentType = "application/json";
              var result = JsonSerializer.Serialize(new Response<string>("You are not authorized to access this resource"));
              return context.Response.WriteAsync(result);
            },
          };
        });
  }

  public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration) {
    services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
    services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
    services.AddTransient<IDateTimeService, SystemDateTimeService>();
    services.AddTransient<IMailService, SMTPMailService>();
    services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();
  }

  public static void AddEssentials(this IServiceCollection services) {
    services.RegisterSwagger();
    services.AddVersioning();
  }

  private static void RegisterSwagger(this IServiceCollection services) {
    services.AddSwaggerGen(c => {
      //TODO - Lowercase Swagger Documents
      //c.DocumentFilter<LowercaseDocumentFilter>();
      //Refer - https://gist.github.com/rafalkasa/01d5e3b265e5aa075678e0adfd54e23f
      c.IncludeXmlComments(string.Format(@"{0}\AspNetCoreHero.Boilerplate.Api.xml", System.AppDomain.CurrentDomain.BaseDirectory));
      c.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v1",
        Title = "AspNetCoreHero.Boilerplate",
        License = new OpenApiLicense() {
          Name = "MIT License",
          Url = new Uri("https://opensource.org/licenses/MIT")
        }
      });
      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
      });
      c.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
    });
  }

  private static void AddVersioning(this IServiceCollection services) {
    services.AddApiVersioning(config => {
      config.DefaultApiVersion = new ApiVersion(1, 0);
      config.AssumeDefaultVersionWhenUnspecified = true;
      config.ReportApiVersions = true;
    });
  }

  public static void AddContextInfrastructure(this IServiceCollection services, IConfiguration configuration) {
    if (configuration.GetValue<bool>("UseInMemoryDatabase")) {
      services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase("IdentityDb"));
      services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
    } else {
      services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ApplicationConnection"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
    }
    services.AddIdentity<ApplicationUser, IdentityRole>(options => {
      options.SignIn.RequireConfirmedAccount = true;
      options.Password.RequireNonAlphanumeric = false;
    }).AddEntityFrameworkStores<IdentityDbContext>().AddDefaultUI().AddDefaultTokenProviders();

    #region Services

    //TODO  services.AddTransient<IIdentityService, IdentityService>();

    #endregion Services

    services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
    services.AddAuthentication(options => {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(o => {
          o.RequireHttpsMetadata = false;
          o.SaveToken = false;
          o.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = configuration["JWTSettings:Issuer"],
            ValidAudience = configuration["JWTSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
          };
          o.Events = new JwtBearerEvents() {
            OnAuthenticationFailed = c => {
              c.NoResult();
              c.Response.StatusCode = 500;
              c.Response.ContentType = "text/plain";
              return c.Response.WriteAsync(c.Exception.ToString());
            },
            OnChallenge = context => {
              context.HandleResponse();
              context.Response.StatusCode = 401;
              context.Response.ContentType = "application/json";
              var result = JsonSerializer.Serialize(Result.Fail("You are not Authorized"));
              return context.Response.WriteAsync(result);
            },
            OnForbidden = context => {
              context.Response.StatusCode = 403;
              context.Response.ContentType = "application/json";
              var result = JsonSerializer.Serialize(Result.Fail("You are not authorized to access this resource"));
              return context.Response.WriteAsync(result);
            },
          };
        });
  }

  public static void AddApplicationLayer(this IServiceCollection services) {
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    services.AddMediatR(Assembly.GetExecutingAssembly());
    //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
  }

  public static IServiceCollection ConfigureServices_AspNetCoreHeroBoilerplateApi(this IServiceCollection services, IConfiguration configuration) {
    services.AddApplicationLayer();
    services.AddContextInfrastructure(configuration);
    //TODO  services.AddPersistenceContexts(configuration);
    //TODO  services.AddRepositories();
    services.AddSharedInfrastructure(configuration);
    services.AddEssentials();
    services.AddControllers();
    services.AddMvc(o => {
      var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
      o.Filters.Add(new AuthorizeFilter(policy));
    });
    return services;
  }

  public static void ConfigureServices_PereirenWebApi(this IServiceCollection services) {
    services.AddControllers();
    services.AddScoped<IToDoItemService, ToDoItemService>();
    services.AddTransient<IToDoItemRepository, ToDoItemRepository>(); //just as an example, you may use it as .AddScoped
    services.AddSingleton<ToDoItemViewModelMapper>();
    services.AddTransient<IToDoItemFactory, EntityFactory>();
    services.AddScoped<TaskCommandHandler>();
    services.AddScoped<ToDoItemEventHandler>();
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    //services.AddFluentMediator(builder => {
    //   builder.On<CreateNewTaskCommand>().PipelineAsync().Return<Domain.Tasks.Task, TaskCommandHandler>((handler, request) => handler.HandleNewTask(request));
    //   builder.On<TaskCreatedEvent>().PipelineAsync().Call<TaskEventHandler>((handler, request) => handler.HandleTaskCreatedEvent(request));
    //   builder.On<DeleteTaskCommand>().PipelineAsync().Call<TaskCommandHandler>((handler, request) => handler.HandleDeleteTask(request));
    //   builder.On<TaskDeletedEvent>().PipelineAsync().Call<TaskEventHandler>((handler, request) => handler.HandleTaskDeletedEvent(request));
    // });
    //services.AddSingleton(serviceProvider => {
    //  var serviceName = Assembly.GetEntryAssembly().GetName().Name;
    //  var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    //  ISampler sampler = new ConstSampler(true);
    //  ITracer tracer = new Tracer.Builder(serviceName)
    //      .WithLoggerFactory(loggerFactory)
    //      .WithSampler(sampler)
    //      .Build();
    //  GlobalTracer.Register(tracer);
    //  return tracer;
    //});
    Log.Logger = new LoggerConfiguration().CreateLogger();
    //services.AddOpenTracing();
    services.AddOptions();
    services.AddMvc();
    services.AddSwaggerGen();
  }

  public static void ConfigureServices_CleanArh(this IServiceCollection services, IConfiguration configuration) {
    // https://github.com/jasontaylordev/CleanArchitecture/tree/main/src/WebUI
    services.AddApplication_CleanArch();
    services.AddInfrastructure_CleanArch(configuration);
    services.AddDatabaseDeveloperPageExceptionFilter();
    services.AddSingleton<ICurrentUserService, CurrentUserService>();
    services.AddHttpContextAccessor();
    services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();
    services.AddControllersWithViews(options => options.Filters.Add<ApiExceptionFilterAttribute>()).AddFluentValidation(x => x.AutomaticValidationEnabled = false);
    services.AddRazorPages();
    // Customise default API behaviour
    services.Configure<ApiBehaviorOptions>(options =>
        options.SuppressModelStateInvalidFilter = true);
    // In production, the Angular files will be served from this directory
    services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");
    services.AddOpenApiDocument(configure => {
      configure.Title = "CleanArchitecture API";
      configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Type into the textbox: Bearer {your JWT token}."
      });
      configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
    });
  }


}