using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Testing.AuthServices;

namespace Testing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Image upload service
            builder.Services.AddScoped<IimageUploadService, ImageUploadService>();
            builder.Services.Configure<stripe>(builder.Configuration.GetSection(nameof(stripe)));
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<NasaService>();
            builder.Services.Configure<Form>(builder.Configuration.GetSection("Cloudinary"));
            builder.Services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IOptions<Form>>().Value;
                var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
                return new Cloudinary(account);
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Policy", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });

            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Testing API v1");
                    c.RoutePrefix = string.Empty; // Swagger UI at root (https://localhost:5001/)
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("Policy");
            app.MapControllers();

            app.Run();
        }
    }
}
