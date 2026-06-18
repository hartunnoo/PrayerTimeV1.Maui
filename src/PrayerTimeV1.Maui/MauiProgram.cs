using Plugin.Maui.Audio;
using PrayerTimeV1.Maui.Interfaces;
using PrayerTimeV1.Maui.Services;
using PrayerTimeV1.Maui.ViewModels;
using PrayerTimeV1.Maui.Views;

namespace PrayerTimeV1.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddSingleton<AzanPlayerService>();
        builder.Services.AddHttpClient<IApiClient, ApiClient>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddSingleton<AzanSchedulerService>();

        return builder.Build();
    }
}
