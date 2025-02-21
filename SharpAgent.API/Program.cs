using SharpAgent.Infrastructure.Extensions;
using SharpAgent.Infrastructure.Seeders;
using SharpAgent.Application.Extensions;
using SharpAgent.API.Middlewares;
using SharpAgent.Domain.Entities;
using Microsoft.OpenApi.Models;

namespace SharpAgent.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication();

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                // Add the Authorize button to swagger
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            // "bearerAuth" references the Security Definition above 
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}
                        },
                        []
                    }
                });
            });

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

            // Adding error handling 1st in our request pipeline (step 2 of 2)
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Exposes Identity Endpoints
            // Prefix each of the Identity endpoints with "identity"
            app.MapGroup("api/identity").MapIdentityApi<User>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
