using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using PrayerTimeV1.Maui.Interfaces;

namespace PrayerTimeV1.Maui.Services;

public class AzanSchedulerService : IDisposable
{
    private readonly IApiClient _api;
    private Timer? _timer;
    private readonly HashSet<string> _notified = [];
    private string _lastDate = "";
    private int _notifyId;
    public bool Enabled { get; set; } = true;

    public AzanSchedulerService(IApiClient api)
    {
        _api = api;
    }

    public void Start()
    {
        _timer = new Timer(async _ => await CheckAsync(), null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30));
    }

    private async Task CheckAsync()
    {
        if (!Enabled) return;
        try
        {
            var today = await _api.GetTodayAsync();
            if (today == null) return;

            var dateKey = today.Date.ToString("yyyy-MM-dd");
            if (dateKey != _lastDate) { _lastDate = dateKey; _notified.Clear(); }

            var now = DateTime.UtcNow.AddHours(8);
            var prayers = new[] {
                ("Subuh", today.Subuh), ("Zohor", today.Zohor), ("Asar", today.Asar),
                ("Maghrib", today.Maghrib), ("Isyak", today.Isyak)
            };

            foreach (var (name, timeStr) in prayers)
            {
                if (_notified.Contains(name)) continue;
                var t = ParseTime(timeStr);
                if (t == null) continue;
                var diff = (now.TimeOfDay - t.Value).TotalSeconds;
                if (diff >= 0 && diff < 60)
                {
                    _notified.Add(name);
                    SendNotification(name);
                }
            }
        }
        catch { }
    }

    private void SendNotification(string prayerName)
    {
        var context = global::Android.App.Application.Context;
        var manager = NotificationManagerCompat.From(context);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel(
                "azan_channel", "Azan",
                NotificationImportance.High)
            {
                Description = "Azan notification"
            };
            var nm = (NotificationManager?)context.GetSystemService(Context.NotificationService);
            nm?.CreateNotificationChannel(channel);
        }

        var builder = new NotificationCompat.Builder(context, "azan_channel")
            .SetContentTitle($"Waktu {prayerName}")
            .SetContentText($"Sudah masuk waktu solat {prayerName}. Segera dirikan solat.")
            .SetSmallIcon(global::Android.Resource.Drawable.IcDialogAlert)
            .SetPriority(NotificationCompat.PriorityHigh)
            .SetAutoCancel(true);

        manager.Notify(Interlocked.Increment(ref _notifyId), builder.Build());
    }

    private static TimeSpan? ParseTime(string? t)
    {
        if (string.IsNullOrWhiteSpace(t)) return null;
        var p = t.Contains('.') ? t.Split('.') : t.Split(':');
        return p.Length == 2 && int.TryParse(p[0], out var h) && int.TryParse(p[1], out var m)
            ? new TimeSpan(h, m, 0) : null;
    }

    public void Dispose() => _timer?.Dispose();
}
