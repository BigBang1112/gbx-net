using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Tool;
using Microsoft.Extensions.Logging;

namespace GhostExtract;

public class GhostExtractTool : ITool, IProductive<IEnumerable<Gbx<CGameCtnGhost>>>
{
    private readonly Gbx gbx;
    private readonly string? fileName;
    private readonly IEnumerable<CGameCtnGhost> ghosts;
    private readonly ILogger logger;

    public GhostExtractTool(Gbx<CGameCtnReplayRecord> gbxReplay, ILogger logger)
    {
        gbx = gbxReplay;
        fileName = Path.GetFileName(gbxReplay.FilePath);
        ghosts = gbxReplay.Node.GetGhosts();
        this.logger = logger;
    }

    public GhostExtractTool(Gbx<CGameCtnMediaClip> gbxClip, ILogger logger)
    {
        gbx = gbxClip;
        fileName = Path.GetFileName(gbxClip.FilePath);
        ghosts = gbxClip.Node.GetGhosts();
        this.logger = logger;
    }

    public IEnumerable<Gbx<CGameCtnGhost>> Produce()
    {
        return ghosts.Select((ghost, i) =>
        {
            logger.LogInformation("Extracting ghost {GhostIndex} from {FileName}...", i + 1, fileName);

            var ghostFileName = GbxPath.GetValidFileName($"{GbxPath.GetFileNameWithoutExtension(fileName ?? "Ghost")}_{i + 1:00}.Ghost.Gbx");

            return new Gbx<CGameCtnGhost>(ghost, gbx.Header.Basic)
            {
                FilePath = ghost.CanBeGameVersion(GameVersion.MP3)
                    ? Path.Combine("Replays", "GbxTools", "GhostExtract", ghostFileName)
                    : Path.Combine("Tracks", "Replays", "GbxTools", "GhostExtract", ghostFileName),
                ClassIdRemapMode = gbx.ClassIdRemapMode,
                PackDescVersion = gbx.PackDescVersion,
            };
        });
    }
}
