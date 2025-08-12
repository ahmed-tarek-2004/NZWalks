using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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
using NZWalk.utility;
using NZWalk.utility.ConfExstinsion;
//using NZWalks.API.Configurations;
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
            builder.Logging.LoggingConfiguration(builder.Services,builder.Configuration);
            #endregion

            // Add services to the container.
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(option =>
            {
                option.SuppressModelStateInvalidFilter = true;
            });
          
            builder.Services.AddHttpContextAccessor();


            //// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            #region Dbcontext Adding
            builder.Services.AddingDbContextes(builder.Configuration);
            #endregion

            #region Dependenci Injection
            builder.Services.AddingDependencyInjection();
            #endregion

            #region JWT Config
            builder.Services.Configure<JWTToken>(builder.Configuration.GetSection("JWT"));
            builder.Services.JWTConfiguration(builder.Configuration);
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
            builder.Services.ConfigureOptions<SwaggerOptionsConfiguration>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(
                    options =>
                {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
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
            //app.UseExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}