using System.Text;

namespace PrayerTimeV1.Maui;

public partial class App : Application
{
    public static string? LastError;

    public App()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            LastError = $"App.Init: {ex}";
            Console.WriteLine(LastError);
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        try
        {
            if (LastError != null)
                return ErrorWindow(LastError);

            return new Window(new AppShell())
            {
                Title = "Waktu Solat",
                MinimumWidth = 320,
                MinimumHeight = 480,
            };
        }
        catch (Exception ex)
        {
            LastError = $"CreateWindow: {ex}";
            return ErrorWindow(LastError);
        }
    }

    private static Window ErrorWindow(string error)
    {
        return new Window(new ContentPage
        {
            BackgroundColor = Color.FromArgb("#060b14"),
            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 10,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label { Text = "⚠️ Ralat Aplikasi", TextColor = Color.FromArgb("#ef4444"), FontSize = 20, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center },
                        new Label { Text = error, TextColor = Color.FromArgb("#94a3b8"), FontSize = 12, FontFamily = "monospace" },
                    }
                }
            }
        })
        {
            Title = "Waktu Solat",
        };
    }
}
