using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Repository;
using NZWalk.Services.IServices;
using NZWalk.Services.Mapping;
using NZWalk.Services.Services;
using NZWalks.API.Configurations;
//using NZWalks.Configuration;
using NZWalks.MiddleWare;
using NZWalks.Validation;
using Serilog;
using System.Text;

namespace NZWalks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region SeriLog
            var logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Services.AddSerilog(logger);
            #endregion

            // Add services to the container.
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(option =>
            {
                option.SuppressModelStateInvalidFilter = true;
            });
            builder.Services.AddHttpContextAccessor();

            #region APIVersioning
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            #endregion
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            #region Dbcontext Adding
            builder.Services.AddDbContext<ApplicationDBContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AuthorizationDBContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnection"));
            });
            #endregion

            #region Dependenci Injection

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IRegionServices, RegionServices>();
            builder.Services.AddScoped<IWalkServices, WalkServices>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<ITokenServices, TokenServices>();
            builder.Services.AddScoped<IImageServices, ImageServices>();
            builder.Services.AddScoped<ValidationFilter>();
            builder.Services.AddScoped<ImageValidation>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
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
            #endregion

            #region JWT Config
            builder.Services.AddAuthentication(option =>
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
                    ValidIssuer = builder.Configuration["JWT:IssuerIP"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:AudienceIP"],
                    ValidateLifetime = true, //  add this
                    ValidateIssuerSigningKey = true, //  add this
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"])),
                    RequireSignedTokens = true,
                    //ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
                    ClockSkew = TimeSpan.Zero//To prevent token to be vaild even time for expiration is over

                };
            });
            #endregion

            #region Swagger Config
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            #region Swagger Settings At Web
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "ASP.NET 9 Web API",
                    Description = "NZWalk Project"
                });
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
                    Id = "Bearer"
                    }
                    },
                    new string[] {}
                    }
                    });
            });
            #endregion

            builder.Services.ConfigureOptions<SwaggerOptionsConfiguration>();
            var app = builder.Build();
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            $"My API {description.GroupName.ToUpperInvariant()}");
                    }
                }
                );
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/Images"
            });
            app.UseMiddleware<TimeEstimate>();
            app.UseMiddleware<GlobalExceptionMiddleWare>();
            //app.UseExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}
