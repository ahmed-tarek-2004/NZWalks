using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
using NZWalks.Validation;
/*
  <PackageReference Include="Microsoft.AspNetCore.Mvc.Versioning" Version="5.1.0" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer" Version="5.1.0" />
 */
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.utility.ConfExstinsion
{

    public static class ConfExtension
    {
        public static IServiceCollection SwaggerConfigration(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                //swagger.SwaggerDoc("v1", new OpenApiInfo
                //{
                //    Version = "v1",
                //    Title = "ASP.NET 9 Web API",
                //    Description = "NZWalk Project"
                //});
                // To Enable authorization using Swagger (JWT)    
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRegionServices, RegionServices>();
            services.AddScoped<IWalkServices, WalkServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<IImageServices, ImageServices>();
            services.AddScoped<ICacheServices, CacheServices>();
            services.AddScoped<IDbInitializer,DbInitializer>();
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
                options.GroupNameFormat = "'v'V";
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
    }
}
