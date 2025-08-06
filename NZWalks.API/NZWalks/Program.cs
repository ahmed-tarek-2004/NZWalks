using Microsoft.EntityFrameworkCore;
using NZWalk.DataAccess.Data;
using NZWalk.DataAccess.IRepository;
using NZWalk.DataAccess.Repository;
using NZWalk.Services.IServices;
using NZWalk.Services.Services;
using NZWalks.MiddleWare;
using NZWalk.Services.Mapping;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace NZWalks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(option =>
            {
                option.SuppressModelStateInvalidFilter = true;
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            #region Dbcontext Adding
            builder.Services.AddDbContext<ApplicationDBContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion

            #region Depemdenci Injection

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IRegionServices, RegionServices>();
            builder.Services.AddScoped<IWalkServices, WalkServices>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            #endregion


            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

           app.UseMiddleware<TimeEstimate>();
           
            app.MapControllers();

            app.Run();
        }
    }
}
