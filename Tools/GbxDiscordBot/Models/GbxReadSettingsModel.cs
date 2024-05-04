using GBX.NET;

namespace GbxDiscordBot.Models;

public sealed class GbxReadSettingsModel
{
    public GbxReadSettings ToReadSettings()
    {
        return new GbxReadSettings();
    }
}
