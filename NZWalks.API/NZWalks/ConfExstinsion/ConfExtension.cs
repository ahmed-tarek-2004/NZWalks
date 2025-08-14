using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.DBInitializer;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Repository;
using NZWalk.Services.IServices;
using NZWalk.Services.Mapping;
using NZWalk.Services.Services;
using NZWalk.Ulity;
using NZWalks.Validation;
using Serilog;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.RateLimiting;

namespace NZWalk.utility.ConfExstinsion
{

    public static class ConfExtension
    {
        public static IServiceCollection SwaggerConfigration(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "Oauth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                    });
            });
            return services;
        }
        public static IServiceCollection AddingDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IRegionServices, RegionServices>();
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<IImageServices, ImageServices>();
            services.AddScoped<ICacheServices, CacheServices>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IWalkServices, WalkServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ValidationFilter>();
            services.AddScoped<ImageValidation>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<AuthorizationDBContext>()
                .AddDefaultTokenProviders();
            return services;
        }
        public static IServiceCollection AddingDbContextes(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDBContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<AuthorizationDBContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("AuthConnection"));
            });
            return services;
        }
        public static IServiceCollection JWTConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:IssuerIP"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:AudienceIP"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"])),
                    RequireSignedTokens = true,
                    //ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
                    ClockSkew = TimeSpan.Zero//To prevent token to be vaild even time for expiration is over

                };
            });
            return services;
        }

        public static IServiceCollection LoggingConfiguration(this IServiceCollection services, ILoggingBuilder logging
            , IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

            logging.ClearProviders();
            services.AddSerilog(logger);

            return services;
        }
        public static IServiceCollection AddingAPIVersionging(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
            }).AddMvc(options =>
            {
                options.Conventions.Add(new VersionByNamespaceConvention());
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            return services;
        }
        public static IServiceCollection CacheConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(option =>
            {
                option.InstanceName = "NZWalk Caching";
                option.Configuration = configuration.GetConnectionString("Redis");
            });
            return services;
        }
        public static IServiceCollection HealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                    .AddSqlServer(
                          connectionString: configuration.GetConnectionString("DefaultConnection"),
                          healthQuery: "Select 1",
                          name: "Sql Server Test",
                          failureStatus: HealthStatus.Unhealthy,
                          tags: new[] { "Sql", "SqlServer", "healthChecks" }
                           )
                    .AddRedis(
                           redisConnectionString: configuration.GetConnectionString("Redis"),
                          name: "Redis Test",
                          failureStatus: HealthStatus.Unhealthy,
                          tags: new[] { "Sql", "SqlServer", "healthChecks" }
                           );
            return services;
        }

        public static IServiceCollection EmailSenderCongiuration(this IServiceCollection services, IConfiguration configuration)
        {
            var smtpConfig = configuration.GetSection("SmtpSettings").Get<EmailSettings>();

            // FluentEmail setup
            services.AddFluentEmail(smtpConfig.FromEmail, smtpConfig.FromName)
                //.AddRazorRenderer()
                .AddSmtpSender(() => new SmtpClient(smtpConfig.Host)
                {
                    Port = smtpConfig.Port,
                    Credentials = new NetworkCredential(smtpConfig.User, smtpConfig.Password),
                    EnableSsl = smtpConfig.EnableSsl
                });
            return services;
        }

        public static IServiceCollection AddingRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("FixedWindow", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 5;
                    limiterOptions.Window = TimeSpan.FromSeconds(60);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = 0;
                });
            });
            return services;
        }
    }
}
