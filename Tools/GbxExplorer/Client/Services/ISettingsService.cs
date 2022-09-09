namespace GbxExplorer.Client.Services;

public interface ISettingsService
{
    int MaxAllowedFileSize { get; set; }
    int MaxFileCountToUpload { get; set; }
    bool InsiderMode { get; set; }
}