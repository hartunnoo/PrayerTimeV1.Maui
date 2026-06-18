using Plugin.Maui.Audio;

namespace PrayerTimeV1.Maui.Services;

public class AzanPlayerService
{
    private readonly IAudioManager _audioManager;
    private IAudioPlayer? _currentPlayer;

    public AzanPlayerService(IAudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    public async Task PlayAzanAsync(string prayerName)
    {
        try
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("azan.mp3");
            _currentPlayer = _audioManager.CreatePlayer(stream);
            _currentPlayer.Volume = 0.7;
            _currentPlayer.Play();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Azan audio error: {ex.Message}");
        }
    }

    public void Stop()
    {
        _currentPlayer?.Stop();
        _currentPlayer?.Dispose();
    }
}
