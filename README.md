# PrayerTimeV1.Maui — Android App

Mobile companion for [lakastahsolat.com](https://lakastahsolat.com). Displays Brunei prayer times with countdown, live highlighting, and Hijri date.

## Requirements

- .NET 10 SDK
- MAUI Android workload: `dotnet workload install maui-android`
- Android SDK (API 21+)

## Build & Run

```bash
cd src/PrayerTimeV1.Maui
dotnet build -f net10.0-android
dotnet run -f net10.0-android
```

## Architecture

```
PrayerTimeV1.Maui.sln
├── src/PrayerTimeV1.Maui/          # MAUI app (Android)
│   ├── App.xaml/.cs                # Application root
│   ├── AppShell.xaml/.cs           # Shell navigation
│   ├── MauiProgram.cs              # DI registration
│   ├── Views/MainPage.xaml/.cs     # Prayer time display
│   ├── ViewModels/MainViewModel.cs # MVVM logic
│   └── Platforms/Android/          # Android manifest + activities
└── src/PrayerTimeV1.Maui.Core/     # Shared library
    ├── Models/PrayerTimeDto.cs     # API response model
    ├── Interfaces/IApiClient.cs    # API contract
    └── Services/ApiClient.cs       # REST client
```

## API

Calls `https://lakastahsolat.com/api/PrayerTimeApi` with API key authentication. Returns 730 days of prayer times in a single request (cached for 24h server-side).

## Connection State

- Online: loads fresh data from API
- Offline: shows last cached data (to be implemented via Preferences/JSON)
