using PrayerTimeV1.Maui.ViewModels;

namespace PrayerTimeV1.Maui.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;
    private bool _loaded;

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
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
            }
            catch (Exception ex)
            {
                _vm.ErrorMessage = $"Ralat: {ex.Message}";
                _vm.IsLoading = false;
            }
        }
    }
}
