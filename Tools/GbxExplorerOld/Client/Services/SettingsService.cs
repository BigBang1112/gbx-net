using GBX.NET;

namespace GbxExplorerOld.Client.Services;

public class SettingsService : ISettingsService
{
    public bool OpenPlanetHookExtractMode { get; set; }
    public bool InsiderMode { get; set; } = false;
}
