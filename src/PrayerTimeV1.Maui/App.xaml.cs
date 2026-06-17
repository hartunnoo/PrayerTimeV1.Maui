namespace PrayerTimeV1.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell())
        {
            Title = "Waktu Solat",
            MinimumWidth = 320,
            MinimumHeight = 480,
        };
    }
}
