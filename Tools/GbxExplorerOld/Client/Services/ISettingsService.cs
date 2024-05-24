namespace GbxExplorerOld.Client.Services;

public interface ISettingsService
{
    int MaxAllowedFileSize { get; set; }
    int MaxFileCountToUpload { get; set; }
    bool OpenPlanetHookExtractMode { get; set; }
    bool InsiderMode { get; set; }
}