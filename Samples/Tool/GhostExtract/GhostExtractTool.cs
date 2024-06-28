using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Tool;

namespace GhostExtract;

public class GhostExtractTool : ITool
{
    private readonly string? fileName;
    private readonly IEnumerable<CGameCtnGhost> ghosts;

    public GhostExtractTool(Gbx<CGameCtnReplayRecord> gbxReplay)
    {
        fileName = Path.GetFileName(gbxReplay.FilePath);
        ghosts = gbxReplay.Node.GetGhosts();
    }

    public GhostExtractTool(Gbx<CGameCtnMediaClip> gbxClip)
    {
        fileName = Path.GetFileName(gbxClip.FilePath);
        ghosts = gbxClip.Node.GetGhosts();
    }

    public IEnumerable<Gbx<CGameCtnGhost>> Produce()
    {
        return ghosts.Select((ghost, i)
            => new Gbx<CGameCtnGhost>(ghost)
            {
                FilePath = ghost.CanBeGameVersion(GameVersion.MP3)
                    ? Path.Combine("Replays", "GbxTools", "GhostExtract", $"{GbxPath.GetFileNameWithoutExtension(fileName ?? "Ghost")}_{i + 1:00}.Gbx.Gbx")
                    : Path.Combine("Tracks", "Replays", "GbxTools", "GhostExtract", $"{GbxPath.GetFileNameWithoutExtension(fileName ?? "Ghost")}_{i + 1:00}.Gbx")
            });
    }
}
