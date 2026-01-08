using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Sitting.Services;
using Sitting.ViewModels;

namespace Sitting
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Load secrets.json
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
            var configuration = configBuilder.Build();

            builder.Configuration.AddConfiguration(configuration);

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Read Firebase settings
            var firebaseSection = configuration.GetSection("Firebase");
            var firebaseApiKey = firebaseSection["ApiKey"];
            var firebaseDatabaseUrl = firebaseSection["DatabaseUrl"];

            // Register HttpClient
            builder.Services.AddSingleton(new HttpClient());

            // Register FirebaseAuthService
            builder.Services.AddSingleton(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                return new FirebaseAuthService(
                    httpClient,
                    firebaseApiKey,
                    firebaseDatabaseUrl
                );
            });

            // Register ViewModel
            builder.Services.AddTransient<LoginViewModel>();

            return builder.Build();
        }
    }
}