using PrayerTimeV1.Maui.Services;
using PrayerTimeV1.Maui.ViewModels;

namespace PrayerTimeV1.Maui.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;
    private readonly AzanSchedulerService _azan;
    private bool _loaded;

    public MainPage(MainViewModel vm, AzanSchedulerService azan)
    {
        InitializeComponent();
        _vm = vm;
        _azan = azan;
        BindingContext = vm;
        AzanSwitch.Toggled += (s, e) => _azan.Enabled = e.Value;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!_loaded)
        {
            _loaded = true;
            try
            {
                await _vm.LoadAsync();
                _azan.Start();
            }
            catch (Exception ex)
            {
                _vm.ErrorMessage = $"Ralat: {ex.Message}";
                _vm.IsLoading = false;
            }
        }
    }
}
