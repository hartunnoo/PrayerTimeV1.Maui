using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PrayerTimeV1.Maui.Core.Interfaces;
using PrayerTimeV1.Maui.Core.Models;

namespace PrayerTimeV1.Maui.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly IApiClient _api;

    public MainViewModel(IApiClient api)
    {
        _api = api;
        PrayerSlots = [];
    }

    private PrayerTimeDto? _today;
    public PrayerTimeDto? Today
    {
        get => _today;
        set { _today = value; OnPropertyChanged(); OnPropertyChanged(nameof(DateDisplay)); OnPropertyChanged(nameof(HijriDisplay)); }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNotLoading)); }
    }

    public bool IsNotLoading => !IsLoading;

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    private TimeSpan _countdown = TimeSpan.Zero;
    public TimeSpan Countdown
    {
        get => _countdown;
        set { _countdown = value; OnPropertyChanged(); OnPropertyChanged(nameof(CountdownDisplay)); }
    }

    private string? _nextPrayerName;
    public string? NextPrayerName
    {
        get => _nextPrayerName;
        set { _nextPrayerName = value; OnPropertyChanged(); }
    }

    private string? _currentPrayerName;
    public string? CurrentPrayerName
    {
        get => _currentPrayerName;
        set { _currentPrayerName = value; OnPropertyChanged(); }
    }

    public ObservableCollection<PrayerSlotViewModel> PrayerSlots { get; }

    public string DateDisplay => Today?.Date.ToString("dddd, dd MMMM yyyy",
        new System.Globalization.CultureInfo("ms-MY")) ?? "Memuat...";

    public string HijriDisplay => Today?.Hijri ?? "";

    public string CountdownDisplay => Countdown.TotalSeconds <= 0
        ? "—"
        : $"{(int)Countdown.TotalHours:D2}:{Countdown.Minutes:D2}:{Countdown.Seconds:D2}";

    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            Today = await _api.GetTodayAsync();

            if (Today == null)
            {
                ErrorMessage = "Tiada data waktu solat untuk hari ini.";
                return;
            }

            BuildPrayerSlots();
            StartCountdown();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Gagal memuat: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void BuildPrayerSlots()
    {
        if (Today == null) return;

        PrayerSlots.Clear();

        var prayers = new[]
        {
            ("Imsak",   Today.Imsak,   "🌑"),
            ("Subuh",   Today.Subuh,   "🌒"),
            ("Syuruk",  Today.Syuruk,  "🌅"),
            ("Dhuha",   Today.Dhuha,   "☀️"),
            ("Zohor",   Today.Zohor,   "🌞"),
            ("Asar",    Today.Asar,    "🌤️"),
            ("Maghrib", Today.Maghrib, "🌇"),
            ("Isyak",   Today.Isyak,   "🌃"),
        };

        var now = DateTime.UtcNow.AddHours(8);
        string? current = null;
        string? next = null;

        foreach (var (name, timeStr, icon) in prayers)
        {
            var time = ParseTime(timeStr);
            if (time == null) continue;

            PrayerSlotState state;
            if (now.TimeOfDay >= time.Value)
            {
                state = PrayerSlotState.Passed;
                current = name;
            }
            else
            {
                state = next == null ? PrayerSlotState.Next : PrayerSlotState.Upcoming;
                next ??= name;
            }

            PrayerSlots.Add(new PrayerSlotViewModel
            {
                Name = name,
                Time = timeStr,
                Icon = icon,
                State = state
            });
        }

        CurrentPrayerName = current;
        NextPrayerName = next;
    }

    private void StartCountdown()
    {
        // Compute next prayer countdown
        var now = DateTime.UtcNow.AddHours(8);
        var next = PrayerSlots.FirstOrDefault(s => s.State == PrayerSlotState.Next);
        if (next != null)
        {
            var time = ParseTime(next.Time);
            if (time.HasValue)
            {
                Countdown = time.Value - now.TimeOfDay;
                if (Countdown.TotalSeconds < 0) Countdown = TimeSpan.Zero;
            }
        }
    }

    private static TimeSpan? ParseTime(string? timeStr)
    {
        if (string.IsNullOrWhiteSpace(timeStr) || timeStr == "-") return null;
        var parts = timeStr.Contains('.') ? timeStr.Split('.') : timeStr.Split(':');
        if (parts.Length != 2) return null;
        if (!int.TryParse(parts[0], out var h) || !int.TryParse(parts[1], out var m)) return null;
        return new TimeSpan(h, m, 0);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class PrayerSlotViewModel
{
    public string Name { get; set; } = "";
    public string Time { get; set; } = "";
    public string Icon { get; set; } = "";
    public PrayerSlotState State { get; set; }
    public Color Color => State switch
    {
        PrayerSlotState.Passed => Color.FromArgb("#475569"),
        PrayerSlotState.Next => Color.FromArgb("#c9a84c"),
        PrayerSlotState.Upcoming => Color.FromArgb("#e8edf8"),
        _ => Color.FromArgb("#e8edf8"),
    };
}

public enum PrayerSlotState { Passed, Next, Upcoming }
