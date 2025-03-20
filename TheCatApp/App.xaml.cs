using System.Windows;
using DataAccess.Repositories;
using DataAccess.Repositories.Abstractions;
using DataAccess.Services;
using DataAccess.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using TheCatApp.Constants;
using TheCatApp.Infrastructure.Factories;
using TheCatApp.Infrastructure.Factories.Abstractions;
using TheCatApp.Infrastructure.Services;
using TheCatApp.Infrastructure.Services.Abstractions;
using TheCatApp.Presentation.View;
using TheCatApp.Presentation.ViewModels;
using TheCatApp.Presentation.ViewModels.Abstractions;

namespace TheCatApp;

public partial class App : Application
{
    private readonly IServiceProvider serviceProvider = ConfigureServices().BuildServiceProvider();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var window = serviceProvider.GetRequiredService<MainWindow>();
        window.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        var breedsService = serviceProvider.GetRequiredService<IBreedsService>();
        using var cts = new CancellationTokenSource(TheCatConstants.DefaultTimeout);

        Task.Run(async () => await breedsService.SaveChangesAsync(cts.Token), cts.Token).Wait(cts.Token);
    }

    private static IServiceCollection ConfigureServices()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        return new ServiceCollection()
            .AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Debug);
                loggingBuilder.AddNLog();
            })
            .AddSingleton<IConfiguration>(_ => configuration)
            .AddHttpClient()
            .AddTransient<IConfigurationService, ConfigurationService>()
            .AddTransient<IBreedsService, BreedsService>()
            .AddTransient<IBreedsRepository, BreedsRepository>()
            .AddTransient<IImagesRepository, ImagesRepository>()
            .AddSingleton<ICacheRepository, CacheRepository>()
            .AddTransient<MainWindow>()
            .AddTransient<CatBreedDetailsWindow>()
            .AddTransient<IMainViewModel, MainViewModel>()
            .AddTransient<ICatBreedDetailsViewModel, CatBreedDetailsViewModel>()
            .AddTransient<IWindowFactory, WindowFactory>()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}

