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
                options.AddPolicy("MyCors", builder =>
                {
                    builder.WithOrigins("http://localhost:4200/")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            }
            );

            //This is an example of dependency injection in the api.
            builder.Services.AddScoped<ISuperhumanRepository, SuperhumanRepository>();

            builder.Services.AddControllers();

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

            app.UseCors("MyCors");

            app.MapControllers();

            app.Run();
        }
    }
}
