using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.FileProviders;
using NZWalk.DataAccess.DBInitializer;
using NZWalk.Services.IServices;
using NZWalk.Ulity;
using NZWalk.utility;
using NZWalk.utility.ConfExstinsion;
using NZWalks.MiddleWare;
using Scrutor;
using StackExchange.Redis;
using System.Threading.RateLimiting;
namespace NZWalks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region SeriLog
            builder.Services.LoggingConfiguration(builder.Logging, builder.Configuration);
            #endregion

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(option =>
            {
                option.SuppressModelStateInvalidFilter = true;
            });


            builder.Services.AddHttpContextAccessor();

            // Add services to the container.
            #region RateLimiter
            builder.Services.AddingRateLimiter();
            #endregion

            //// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            #region Dbcontext Adding
            builder.Services.AddingDbContextes(builder.Configuration);
            #endregion

            #region Dependenci Injection
            builder.Services.AddingDependencyInjection();
            #endregion

            #region Adding Redis
            //  builder.Services.CacheConfiguration(builder.Configuration);
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Redis");
                var config = ConfigurationOptions.Parse(connectionString);
                config.Ssl = true;
                config.AbortOnConnectFail = false;

                options.ConfigurationOptions = config;
            });
            #endregion

            #region HealthCheck
            builder.Services.HealthCheck(builder.Configuration);
            #endregion



            #region JWT Config
            builder.Services.Configure<JWTToken>(builder.Configuration.GetSection("JWT"));
            builder.Services.JWTConfiguration(builder.Configuration);
            #endregion

            #region Email Sender 
            builder.Services.EmailSenderCongiuration(builder.Configuration);
            #endregion

            #region Swagger Config
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            #region Swagger Settings At Web
            builder.Services.SwaggerConfigration();
            #endregion

            #region APIVersioning
            builder.Services.AddingAPIVersionging();
            #endregion

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("NZWalks", opt =>
                {
                    opt.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            builder.Services.ConfigureOptions<SwaggerOptionsConfiguration>();
            var app = builder.Build();
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(

                    options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            $"NZWalks {description.GroupName.ToUpperInvariant()}");
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
            app.UseCors("NZWalks");
            //app.usepo
            SeedDatabase().GetAwaiter().GetResult();
            app.UseHealthChecks("/health", new HealthCheckOptions());
            app.MapControllers();
            async Task SeedDatabase()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                    await dbInitializer.Initialize();
                }
            }
            app.UseRateLimiter();
            app.MapGet("/test", () => $"Hello! Time: {DateTime.Now}")
                .RequireRateLimiting("FixedWindow");
            

            app.Run();
        }
    }
}