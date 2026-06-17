namespace PrayerTimeV1.Maui;

public partial class App : Application
{
    public App()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            // Prevent crash on startup — log to console
            Console.WriteLine($"App init error: {ex.Message}");
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        try
        {
            return new Window(new AppShell())
            {
                Title = "Waktu Solat",
                MinimumWidth = 320,
                MinimumHeight = 480,
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Window error: {ex.Message}");
            // Fallback: blank page with error
            return new Window(new ContentPage
            {
                Content = new Label
                {
                    Text = $"Gagal memulakan aplikasi.\n{ex.Message}",
                    TextColor = Colors.White,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                }
            })
            {
                Title = "Waktu Solat",
            };
        }
    }
}
