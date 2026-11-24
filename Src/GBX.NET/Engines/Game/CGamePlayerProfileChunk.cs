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

    /// <summary>
    /// Selects the buffer memory payload of an old archive, which embeds referenced nodes as Gbx files instead of node references.
    /// This context is supplied by the caller and is not stored in the payload.
    /// </summary>
    public bool IsBufferMemoryArchivation { get; set; }

    /// <summary>
    /// Selects the native online-save payload, which omits local data.
    /// This context is supplied by the caller and is not stored in the payload.
    /// </summary>
    public bool IsOnlineSaveArchivation { get; set; }

    /// <summary>
    /// Reads or writes a node of an old archive. Buffer memory payloads store a presence byte and an uncompressed Gbx, other payloads store a node reference.
    /// </summary>
    private protected void ReadWriteArchivedNode<T>(GbxReaderWriter rw, ref NodeRef<T>? nodeRef) where T : CMwNod
    {
        if (!IsBufferMemoryArchivation)
        {
            rw.NodeRef(ref nodeRef);
            return;
        }

        var node = rw.Writer is null ? null : nodeRef?.Resolve();
        var hasNode = rw.Boolean(node is not null, asByte: true);

        if (rw.Reader is GbxReader r)
        {
            node = null;

            if (hasNode)
            {
                var embedded = Gbx.ParseNode(r.BaseStream, r.Settings with { CloseStream = false });
                node = embedded as T ?? throw new InvalidDataException($"Embedded node {embedded?.GetType().Name ?? "null"} is not {typeof(T).Name}.");
            }

            NodeRef.SetEmbedded(ref nodeRef, node);
        }

        if (rw.Writer is GbxWriter w && node is not null)
        {
            var gbx = node.ToGbx();
            gbx.BodyCompression = GbxCompression.Uncompressed;
            gbx.Save(w.BaseStream, w.Settings with { CloseStream = false });
        }
    }

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
