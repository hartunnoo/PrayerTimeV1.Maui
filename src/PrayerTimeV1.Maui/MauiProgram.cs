using Microsoft.Extensions.Logging;
using PrayerTimeV1.Maui.Core.Interfaces;
using PrayerTimeV1.Maui.Core.Services;
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

        // Services
        builder.Services.AddHttpClient<IApiClient, ApiClient>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
