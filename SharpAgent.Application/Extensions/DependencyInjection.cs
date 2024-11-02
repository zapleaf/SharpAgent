
using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.Extensions.DependencyInjection;

// file-scoped namespace
namespace SharpAgent.Application.Extensions;

// This static class and it's extension method are used to inject services into the DI container.
// It will be invoked in the API layer's Program.cs file to register application-level services.
public static class DependencyInjection
{
    // This method could return void, but returning IServiceCollection allows for method chaining.
    // We use the name "AddApplication" so in Programs.cs we know what is being added 
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // The assembly paramater is used by our various services to find the services they relate to
        var applicationAssembly = typeof(DependencyInjection).Assembly;

        // Register application-specific services here, forming a "Service Collection"
        // services.AddScoped<IYourEntityService, YourEntityService>();
        // The MediatR more or less replaces things like AddScoped<IService, Service> for entity
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(applicationAssembly));

        // Take any entity with a "Profile" base class and include it's Automapper configuration
        services.AddAutoMapper(applicationAssembly);

        // Take validators with the AbstractValidotor base class
        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();

        return services;
    }
}
