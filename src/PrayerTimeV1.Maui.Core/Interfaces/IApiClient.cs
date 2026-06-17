using PrayerTimeV1.Maui.Core.Models;

namespace PrayerTimeV1.Maui.Core.Interfaces;

public interface IApiClient
{
    Task<List<PrayerTimeDto>> GetAllPrayerTimesAsync(CancellationToken ct = default);
    Task<PrayerTimeDto?> GetTodayAsync(CancellationToken ct = default);
    Task<PrayerTimeDto?> GetTomorrowAsync(CancellationToken ct = default);
    Task<bool> IsAvailableAsync(CancellationToken ct = default);
}
