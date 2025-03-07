using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using SharpAgent.Blazor.Components;
using SharpAgent.Blazor.Components.Account;
using SharpAgent.Domain.Entities;
using SharpAgent.Infrastructure.Extensions;
using SharpAgent.Application.Extensions;
using SharpAgent.Infrastructure.Data;

namespace SharpAgent.Blazor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Add your infrastructure and application services using Clean Architecture pattern
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();

        // Add Identity-related services
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        //builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider<User>>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        // Configure authentication
        builder.Services.AddAuthentication();
        //builder.Services.AddAuthentication(options =>
        //{
        //    options.DefaultScheme = IdentityConstants.ApplicationScheme;
        //    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        //})
        //    .AddIdentityCookies();

        // Removed this default section since this is already registered in the Infrastructure project
        // Configure Identity to use your existing User class and AppDbContext
        //builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
        //    .AddEntityFrameworkStores<AppDbContext>()
        //    .AddSignInManager()
        //    .AddDefaultTokenProviders();

        // Add email sender (replace with your actual implementation if needed)
        // builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.UseStaticFiles();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        app.Run();
    }
}