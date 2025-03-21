using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using SharpAgent.Application.IServices;
using SharpAgent.Application.IRepositories;

using SharpAgent.Domain.Entities;

using SharpAgent.Infrastructure.Data;
using SharpAgent.Infrastructure.Services;
using SharpAgent.Infrastructure.Seeders;
using SharpAgent.Infrastructure.Repositories;
using SharpAgent.Domain.Common;


// file-scoped namespace
namespace SharpAgent.Infrastructure.Extensions;

// This static class contains an extension method to register infrastructure-related services into the DI container.
// It will be invoked in the API layer's Program.cs file to set up services that belong to the infrastructure layer.
public static class DependencyInjection
{
    // This method could return void, but returning IServiceCollection allows for method chaining.
    // We use the name "AddInfrastructure" so in Programs.cs we know what is being added 
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // We could have just passed in the connectionString, however, passing the entire IConfiguration object
        // provides access to additional configuration settings, if needed in the future.
        var connectionString = configuration.GetConnectionString("AppDb");

        // Passes the connection string to its constructor via options.
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        // Any seed data can be placed in the AppSeeder class
        services.AddScoped<IAppSeeder, AppSeeder>();

        // Matching Endpoints are added in the Program.cs file of the API
        services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<AppDbContext>();

        // Register Services
        services.AddScoped<IDocumentAnalysisService, AzureDocumentService>();
        services.AddScoped<IOpenAIChatService, OpenAIChatService>();
        services.AddTransient<IEmbeddingService, OpenAIEmbeddingService>();
        services.AddTransient<IVectorDbService, PineconeService>();
        services.AddTransient<ITranscriptService, TranscriptService>();

        // Register Repositories
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAiSummaryRepository, AiSummaryRepository>();
        services.AddScoped<IAiAnalysisRepository, AiAnalysisRepository>();
        services.AddScoped<IPromptVersionRepository, PromptVersionRepository>();

        // Get YouTube API key from settings and register the YouTube Service
        services.Configure<YouTubeApiKey>(configuration.GetSection("YouTubeApiKey"));
        var youtubeApiKey = configuration.GetSection("YouTubeApiKey").Get<YouTubeApiKey>();
        services.AddSingleton<YouTubeApiKey>(youtubeApiKey);
        services.AddScoped<IYouTubeApiService, YouTubeApiService>();

        return services;
    }
}
