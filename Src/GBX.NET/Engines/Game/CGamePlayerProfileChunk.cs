using System.Text;

namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk
{
    public string ChunkName { get; set; } = string.Empty;
    public string GameName { get; set; } = string.Empty;
    public string Checksum { get; set; } = string.Empty;
    public DateTimeOffset? LastUpdatedAt { get; set; }
    public int ArchiveVersion { get; set; }
    public int? SkipArchiveVersion { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder(GetType().Name);

        sb.Append(" {");

        if (!string.IsNullOrEmpty(ChunkName))
        {
            sb.Append(' ');
            sb.Append(ChunkName);
        }

        if (!string.IsNullOrEmpty(GameName))
        {
            sb.Append(' ');
            sb.Append(GameName);
        }

        sb.Append(' ');
        sb.Append(LastUpdatedAt?.ToString("yyyy-MM-dd'T'HH:mm:ss"));

        if (CreatedAt.HasValue)
        {
            sb.Append(" | ");
            sb.Append(CreatedAt.Value.ToString("yyyy-MM-dd'T'HH:mm:ss"));
        }

        sb.Append(" }");

        return sb.ToString();
    }
}
