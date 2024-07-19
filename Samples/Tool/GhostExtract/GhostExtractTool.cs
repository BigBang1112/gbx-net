using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Tool;
using Microsoft.Extensions.Logging;

namespace GhostExtract;

public class GhostExtractTool : ITool, IProductive<IEnumerable<Gbx<CGameCtnGhost>>>
{
    private readonly string? fileName;
    private readonly IEnumerable<CGameCtnGhost> ghosts;
    private readonly ILogger logger;

    public GhostExtractTool(Gbx<CGameCtnReplayRecord> gbxReplay, ILogger logger)
    {
        fileName = Path.GetFileName(gbxReplay.FilePath);
        ghosts = gbxReplay.Node.GetGhosts();
        this.logger = logger;
    }

    public GhostExtractTool(Gbx<CGameCtnMediaClip> gbxClip, ILogger logger)
    {
        fileName = Path.GetFileName(gbxClip.FilePath);
        ghosts = gbxClip.Node.GetGhosts();
        this.logger = logger;
    }

    public IEnumerable<Gbx<CGameCtnGhost>> Produce()
    {
        return ghosts.Select((ghost, i) =>
        {
            logger.LogInformation("Extracting ghost {GhostIndex} from {FileName}...", i + 1, fileName);

            return new Gbx<CGameCtnGhost>(ghost)
            {
                FilePath = ghost.CanBeGameVersion(GameVersion.MP3)
                    ? Path.Combine("Replays", "GbxTools", "GhostExtract", $"{GbxPath.GetFileNameWithoutExtension(fileName ?? "Ghost")}_{i + 1:00}.Ghost.Gbx")
                    : Path.Combine("Tracks", "Replays", "GbxTools", "GhostExtract", $"{GbxPath.GetFileNameWithoutExtension(fileName ?? "Ghost")}_{i + 1:00}.Ghost.Gbx")
            };
        });
    }
}
