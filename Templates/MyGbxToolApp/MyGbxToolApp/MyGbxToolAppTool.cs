using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Tool;
using Microsoft.Extensions.Logging;

namespace MyGbxToolApp;

public class MyGbxToolAppTool : ITool, IProductive<Gbx<CGameCtnMediaClip>>
{
    private readonly Gbx gbx;
    private readonly CGameCtnMediaClip? clip;
    private readonly string? fileName;
    private readonly ILogger logger;

    public MyGbxToolAppTool(Gbx<CGameCtnChallenge> gbxMap, ILogger logger)
    {
        gbx = gbxMap;
        clip = gbxMap.Node.ClipIntro;
        fileName = Path.GetFileName(gbxMap.FilePath);
        this.logger = logger;
    }

    public Gbx<CGameCtnMediaClip> Produce()
    {
        logger.LogInformation("Extracting intro clip from {FileName}...", fileName);

        if (clip is null)
        {
            throw new InvalidOperationException("The map does not have an intro clip.");
        }

        var clipFileName = GbxPath.GetValidFileName($"{GbxPath.GetFileNameWithoutExtension(fileName ?? "Map")}_Intro.Clip.Gbx");

        return new Gbx<CGameCtnMediaClip>(clip)
        {
            FilePath = clip.CanBeGameVersion(GameVersion.MP3)
                ? Path.Combine("Replays", "Clips", "GbxTools", "MyGbxToolApp", clipFileName)
                : Path.Combine("Tracks", "GbxTools", "MyGbxToolApp", clipFileName),
            ClassIdRemapMode = gbx.ClassIdRemapMode,
            PackDescVersion = gbx.PackDescVersion,
        };
    }
}
