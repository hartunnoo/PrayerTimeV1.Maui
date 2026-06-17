namespace PrayerTimeV1.Maui.Core.Models;

/// <summary>
/// Prayer time data from the PrayerTimeV1 API.
/// Matches the PrayerTimeListDto shape from the server.
/// </summary>
public class PrayerTimeDto
{
    public DateTime Date { get; set; }
    public string Hijri { get; set; } = string.Empty;
    public string Imsak { get; set; } = string.Empty;
    public string Subuh { get; set; } = string.Empty;
    public string Syuruk { get; set; } = string.Empty;
    public string Dhuha { get; set; } = string.Empty;
    public string Zohor { get; set; } = string.Empty;
    public string Asar { get; set; } = string.Empty;
    public string Maghrib { get; set; } = string.Empty;
    public string Isyak { get; set; } = string.Empty;
    public string DayName { get; set; } = string.Empty;
    public bool IsToday { get; set; }
    public bool IsTomorrow { get; set; }
}
