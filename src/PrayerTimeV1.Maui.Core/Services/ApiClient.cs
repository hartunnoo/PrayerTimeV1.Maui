using System.Net.Http.Json;
using PrayerTimeV1.Maui.Core.Interfaces;
using PrayerTimeV1.Maui.Core.Models;

namespace PrayerTimeV1.Maui.Core.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _http;
    private const string BaseUrl = "https://lakastahsolat.com";
    private const string ApiKey = "lakastahsolat_SvTjo9w0wfct8uaWduUpMfih5gcvL1CNOPPBjXpXgwEItcUGAwOCekP4S2NUIMZb";

    public ApiClient(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri(BaseUrl);
        _http.DefaultRequestHeaders.Add("X-API-Key", ApiKey);
        _http.DefaultRequestHeaders.Add("Accept", "application/json");
        _http.Timeout = TimeSpan.FromSeconds(15);
    }

    public async Task<List<PrayerTimeDto>> GetAllPrayerTimesAsync(CancellationToken ct = default)
    {
        var result = await _http.GetFromJsonAsync<List<PrayerTimeDto>>("/api/PrayerTimeApi", ct);
        return result ?? [];
    }

    public async Task<PrayerTimeDto?> GetTodayAsync(CancellationToken ct = default)
    {
        var all = await GetAllPrayerTimesAsync(ct);
        var today = DateTime.UtcNow.AddHours(8).Date;
        return all.FirstOrDefault(p => p.Date.Date == today);
    }

    public async Task<PrayerTimeDto?> GetTomorrowAsync(CancellationToken ct = default)
    {
        var all = await GetAllPrayerTimesAsync(ct);
        var tomorrow = DateTime.UtcNow.AddHours(8).Date.AddDays(1);
        return all.FirstOrDefault(p => p.Date.Date == tomorrow);
    }

    public async Task<bool> IsAvailableAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await _http.GetAsync("/api/PrayerTimeApi", ct);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
