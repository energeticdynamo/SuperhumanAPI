using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Data;
using SuperhumanAPI.Repositories;

namespace SuperhumanAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //This is an example of dependency injection in the api.

            builder.Services.AddProjectDbContexts(builder.Configuration);

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyCorsPolicy", policyBuilder =>
                {
                    policyBuilder.WithOrigins("http://192.168.1.220:8080", "http://localhost:4200/")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            }
            );

            //This is an example of dependency injection in the api.
            builder.Services.AddScoped<ISuperhumanRepository, SuperhumanRepository>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SuperhumanAPI v1");
                    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
                });
            }

            app.UseCors("MyCorsPolicy");

            app.MapControllers();

            app.Run();
        }
    }
}
