using SharpAgent.Infrastructure.Extensions;
using SharpAgent.Infrastructure.Seeders;
using SharpAgent.Application.Extensions;
using SharpAgent.API.Middlewares;

namespace SharpAgent.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            //// Logging is included by default in WebApplicationBuilder
            //// But you can configure it further:
            //builder.Logging.ClearProviders();
            //builder.Logging.AddConsole();
            //builder.Logging.AddDebug();

            //// Optional: Set minimum log level
            //builder.Logging.SetMinimumLevel(LogLevel.Information);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Injecting our error handling middleware (step 1 of 2)
            builder.Services.AddScoped<ErrorHandlingMiddleware>();

            // You could get connection string from appsetting and pass it for DbContext or 
            // as in this case, pass the entire config to our Infrastructure service
            builder.Services.AddInfrastructure(builder.Configuration);     // Goes to the AddInfrastructure method in ServiceCollectionExtension class
            builder.Services.AddApplication();                             // Goes to the AddApplication method in ServiceCollectionExtension class

            var app = builder.Build();
            
            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IAppSeeder>();

            await seeder.Seed();

            // Configure the HTTP request pipeline.

            // Adding error handling 1st in our request pipeline (step 2 of 2)
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
