using PrayerTimeV1.Maui.Models;

namespace PrayerTimeV1.Maui.Interfaces;

public interface IApiClient
{
    Task<List<PrayerTimeDto>> GetAllPrayerTimesAsync(CancellationToken ct = default);
    Task<PrayerTimeDto?> GetTodayAsync(CancellationToken ct = default);
    Task<PrayerTimeDto?> GetTomorrowAsync(CancellationToken ct = default);
    Task<bool> IsAvailableAsync(CancellationToken ct = default);
}
