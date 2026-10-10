namespace GBX.NET.Engines.Plug;

public partial class CPlugVisual
{
    private const int isGeometryStaticBit = 3;
    private const int isIndexationStaticBit = 5;
    private const int hasVertexNormalsBit = 7;
    private const int serializedFlagsMask = 0x7001AF;

    public int Flags { get; set; } = 0x804F0;
    internal int Count { get; set; }

    public int VertexCount
    {
        get => Count;
        set => Count = value;
    }

    public List<CPlugVertexStream> VertexStreams { get; set; } = [];
    public TexCoordSet[] TexCoords { get; set; } = [];
    public BoxAligned BoundingBox { get; set; } = new(0, 0, 0, -1, -1, -1);
    public SSkinData? SkinData { get; set; }
    public ushort[]? UvGroups { get; set; } = [];

    public bool IsGeometryStatic
    {
        get => IsFlagBitSet(isGeometryStaticBit);
        set => SetFlagBit(isGeometryStaticBit, value);
    }

    public bool IsIndexationStatic
    {
        get => IsFlagBitSet(isIndexationStaticBit);
        set => SetFlagBit(isIndexationStaticBit, value);
    }

    public bool HasVertexNormals
    {
        get => IsFlagBitSet(hasVertexNormalsBit);
        set => SetFlagBit(hasVertexNormalsBit, value);
    }

    public bool HasVertexColors
    {
        get => IsFlagBitSet(8);
        set => SetFlagBit(8, value);
    }

    public int SkinIndexCount
    {
        get => Flags & 7;
        set => Flags = (Flags & ~7) | (value & 7);
    }

    private static int ConvertChunkFlagsToFlags(int chunkFlags) => (chunkFlags & 15)
        | ((chunkFlags << 1) & 0x20) | ((chunkFlags << 2) & 0x180) | ((chunkFlags << 13) & 0x700000);

    private static int ConvertFlagsToChunkFlags(int flags) => (flags & 15)
        | ((flags >> 1) & 0x10) | ((flags >> 2) & 0x60) | ((flags >> 13) & 0x380);

    public bool IsFlagBitSet(int bit) => (Flags & (1 << bit)) != 0;

    public void SetFlagBit(int bit, bool value)
    {
        if (value) Flags |= 1 << bit;
        else Flags &= ~(1 << bit);
    }

    private static void ReadWriteGeometryHeader(CPlugVisual n, GbxReaderWriter rw, bool packedFlags, bool vertexStreams, bool rawTexCoords = false)
    {
        if (packedFlags)
        {
            var chunkFlags = rw.Int32(ConvertFlagsToChunkFlags(n.Flags));
            n.Flags = (n.Flags & ~serializedFlagsMask) | ConvertChunkFlagsToFlags(chunkFlags);
        }
        else
        {
            n.IsGeometryStatic = rw.Boolean(n.IsGeometryStatic);
            n.IsIndexationStatic = rw.Boolean(n.IsIndexationStatic);
        }

        var texCoordSetCount = rw.Int32(n.TexCoords.Length);
        if (!packedFlags) n.SkinIndexCount = rw.Int32(n.SkinIndexCount);
        n.Count = rw.Int32(n.Count);
        if (vertexStreams) n.VertexStreams = rw.ListNodeRef(n.VertexStreams!)!;

        if (rw.Reader is not null) n.TexCoords = new TexCoordSet[texCoordSetCount];
        for (var i = 0; i < texCoordSetCount; i++)
        {
            n.TexCoords[i] ??= new TexCoordSet();
            if (rawTexCoords) n.TexCoords[i].ReadWriteComponents(rw, n.Count, 0);
            else n.TexCoords[i].ReadWrite(rw, n.Count);
        }
    }

    private static void ReadWriteSkin(CPlugVisual n, GbxReaderWriter rw, uint chunkId, int version = 0)
    {
        if (n.SkinIndexCount == 0) return;
        n.SkinData ??= new SSkinData();
        n.SkinData.ReadWrite(rw, chunkId, version, n.Count, n.SkinIndexCount);
    }

    public partial class Chunk09006003
    {
        public override GameVersion GameVersion => GameVersion.Unspecified;
    }

    public partial class Chunk09006006
    {
        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            n.HasVertexNormals = rw.Boolean(n.HasVertexNormals);
        }
    }

    public partial class Chunk09006008
    {
        public Iso4[]? LegacyBoneTransforms;

        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            ReadWriteGeometryHeader(n, rw, packedFlags: false, vertexStreams: Id == 0x0900600A, rawTexCoords: Id == 0x09006003);
            if (n.SkinIndexCount != 0)
            {
                n.SkinData ??= new SSkinData();
                n.SkinData.ReadWriteVertexWeights(rw, n.Count, n.SkinIndexCount);
            }
            n.HasVertexColors = rw.Boolean(n.HasVertexColors);
            rw.Array(ref LegacyBoneTransforms);
        }
    }

    public partial class Chunk0900600C
    {
        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            ReadWriteGeometryHeader(n, rw, packedFlags: false, vertexStreams: true);
            ReadWriteSkin(n, rw, Id);
            n.HasVertexColors = rw.Boolean(n.HasVertexColors);
            n.BoundingBox = rw.BoxAligned(n.BoundingBox);
        }
    }

    public partial class Chunk0900600D
    {
        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            ReadWriteGeometryHeader(n, rw, packedFlags: true, vertexStreams: true);
            ReadWriteSkin(n, rw, Id);
            n.BoundingBox = rw.BoxAligned(n.BoundingBox);
        }
    }

    public partial class Chunk0900600F
    {
        public int PackedDataVersion;
        public byte[]? PackedData;

        public override void ReadWrite(CPlugVisual n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            ReadWriteGeometryHeader(n, rw, packedFlags: true, vertexStreams: true);
            ReadWriteSkin(n, rw, Id, Version);
            n.BoundingBox = rw.BoxAligned(n.BoundingBox);
            n.BitmapElemToPacks = rw.ArrayReadableWritable(n.BitmapElemToPacks)!;
            if (Version >= 5) n.UvGroups = rw.Array(n.UvGroups);
            if (Version >= 6)
            {
                rw.Int32(ref PackedDataVersion);
                var size = rw.Int32(PackedData is null ? 0 : checked(PackedData.Length + 4));
                if (size != 0 && size < 4) throw new InvalidDataException("Packed visual data size is smaller than its header.");
                PackedData = size == 0 ? null : rw.Data(PackedData, size - 4);
            }
        }
    }

    public partial class Morph
    {
        public virtual void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            Indices = rw.Array(Indices)!;
            BoneCount = rw.Int32(Bones.Length);
            VertexStream = rw.NodeRef(VertexStream);
            Bones = rw.ArrayId(Bones, BoneCount)!;
        }
    }
}
