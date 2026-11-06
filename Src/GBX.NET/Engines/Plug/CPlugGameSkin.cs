namespace GBX.NET.Engines.Plug;

public partial class CPlugGameSkin
{
    private Fid[]? customizableFids;
    private string? dirName;

    [AppliedWithChunk<Chunk090F4000>]
    [AppliedWithChunk<Chunk090F4002>]
    public partial Fid[]? CustomizableFids { get => customizableFids; set => customizableFids = value; }

    [AppliedWithChunk<Chunk090F4002>]
    public partial string? DirName { get => dirName; set => dirName = value; }

    /// <remarks>Applied with: 0x001 body chunk</remarks>
    [AppliedWithChunk<Chunk090F4001>]
    public Fid[]? CustomizableSounds { get; set; }

    /// <summary>Compatibility alias for <see cref="CustomizableSounds"/>.</summary>
    [AppliedWithChunk<Chunk090F4001>]
    public Fid[]? CustomizableFids2 { get => CustomizableSounds; set => CustomizableSounds = value; }

    public partial class Chunk090F4000
    {
        public override void ReadWrite(CPlugGameSkin n, GbxReaderWriter rw)
        {
            n.CustomizableFids = ReadWriteLegacyFids(rw, n.CustomizableFids, 0x09025000);
        }
    }

    public partial class Chunk090F4001
    {
        public override void ReadWrite(CPlugGameSkin n, GbxReaderWriter rw)
        {
            n.CustomizableSounds = ReadWriteLegacyFids(rw, n.CustomizableSounds, 0x09030000);
        }
    }

    public partial class Chunk090F4002
    {
        public override void ReadWrite(CPlugGameSkin n, GbxReaderWriter rw)
        {
            rw.String(ref n.dirName!);
            n.CustomizableFids = ReadWriteLegacyFids(rw, n.CustomizableFids, 0, classIdsOnWire: true);
        }
    }

    private static Fid[]? ReadWriteLegacyFids(GbxReaderWriter rw, Fid[]? fids, uint classId, bool classIdsOnWire = false)
    {
        var count = rw.Int32(fids?.Length ?? 0);
        var nameCount = rw.Int32(count);
        var classIdCount = classIdsOnWire ? rw.Int32(count) : count;
        if (count < 0 || nameCount != count || classIdCount != count)
        {
            throw new InvalidDataException("Skin remapping counts must be equal and nonnegative.");
        }

        if (rw.Reader is not null)
        {
            fids = new Fid[count];
        }

        for (var i = 0; i < count; i++)
        {
            var fid = rw.Reader is not null ? fids![i] = new Fid { ClassId = classId } : fids![i];
            rw.NodeRef(fid.References.Node);
            fid.Name = rw.String(fid.Name);
            if (classIdsOnWire) fid.ClassId = rw.UInt32(fid.ClassId);
        }

        return fids;
    }

    public partial class Fid : IReadableWritable
    {
        private uint classId;
        private string name = "";

        private string path = "";
        private bool mipMaps;

        public uint ClassId { get => classId; set => classId = value; }
        public string Name { get => name; set => name = value; }

        public CMwNod? Node { get => nodeRef?.Resolve(); set => NodeRef.SetEmbedded(ref nodeRef, value); }

        /// <summary>A file or folder path relative to game data; v3 and later use this for folders.</summary>
        public string Path { get => path; set => path = value; }

        [Obsolete("Use Path instead.")]
        public string Directory { get => Path; set => Path = value; }
        public bool MipMaps { get => mipMaps; set => mipMaps = value; }

        public void ReadWrite(GbxReaderWriter rw, int version = 0)
        {
            rw.UInt32(ref classId);
            rw.String(ref name!);

            if (version >= 3)
            {
                if (rw.Boolean((nodeRef?.Kind ?? NodeRefKind.None) != NodeRefKind.None))
                {
                    rw.NodeRef(ref nodeRef);
                }
                else
                {
                    rw.String(ref path!);
                }
            }
            else
            {
                rw.String(ref path!);
            }

            if (version >= 2)
            {
                rw.Boolean(ref mipMaps);
            }
        }

        public override string ToString()
        {
            return $"[0x{ClassId:X8}] {Name} ({Node?.ToString() ?? Path})";
        }
    }

    public partial class HeaderFid
    {
        [Obsolete("Use Path instead.")]
        public string? Directory { get => Path; set => Path = value; }

        public override string ToString()
        {
            return $"[0x{ClassId:X8}] {Name} ({Path})";
        }
    }
}
