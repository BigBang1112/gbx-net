namespace GbxExplorer.Client.Services;

public class SettingsService : ISettingsService
{
    public int MaxAllowedFileSize { get; set; } = 65_536_000;
    public int MaxFileCountToUpload { get; set; } = 100;
    public bool InsiderMode { get; set; } = false;
}
