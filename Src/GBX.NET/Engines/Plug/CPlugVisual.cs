namespace GBX.NET.Engines.Plug;

public partial class CPlugVisual
{
    private const int isGeometryStaticBit = 3;
    private const int isIndexationStaticBit = 5;
    private const int hasVertexNormalsBit = 7;

    public int Flags { get; set; }

    internal int Count { get; set; }

    public IList<CPlugVertexStream> VertexStreams { get; set; } = [];
    public TexCoordSet[] TexCoords { get; set; } = [];
    public BoxAligned BoundingBox { get; set; }

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

    private static int ConvertChunkFlagsToFlags(int chunkFlags)
    {
        var flags = 0;
        flags |= chunkFlags & 15;
        flags |= (chunkFlags << 1) & 0x20;
        flags |= (chunkFlags << 2) & 0x80;
        flags |= (chunkFlags << 2) & 0x100;
        flags |= (chunkFlags << 13) & 0x100000;
        flags |= (chunkFlags << 13) & 0x200000;
        flags |= (chunkFlags << 13) & 0x400000;
        return flags;
    }

    private static int ConvertFlagsToChunkFlags(int flags)
    {
        var chunkFlags = flags & 15;
        chunkFlags |= (flags >> 1) & 0x10;
        chunkFlags |= (flags >> 2) & 0x20;
        chunkFlags |= (flags >> 2) & 0x40;
        chunkFlags |= (flags >> 13) & 0x80;
        chunkFlags |= (flags >> 13) & 0x100;
        chunkFlags |= (flags >> 13) & 0x200;
        return chunkFlags;
    }

    public bool IsFlagBitSet(int bit) => (Flags & (1 << bit)) != 0;

    public void SetFlagBit(int bit, bool value)
    {
        if (value) Flags |= 1 << bit;
        else Flags &= ~(1 << bit);
    }

    public partial class Chunk09006006
    {
        public override void Read(CPlugVisual n, GbxReader r)
        {
            n.HasVertexNormals = r.ReadBoolean();
        }

        public override void Write(CPlugVisual n, GbxWriter w)
        {
            w.Write(n.HasVertexNormals);
        }
    }

    public partial class Chunk09006008
    {
        public Iso4[]? U01;

        public override void Read(CPlugVisual n, GbxReader r)
        {
            n.IsGeometryStatic = r.ReadBoolean();
            n.IsIndexationStatic = r.ReadBoolean();

            var numTexCoordSets = r.ReadInt32();

            // this might be wrong
            var skinFlags = r.ReadInt32() & 7; // Skin (& 7 added for safety)
            n.Flags |= skinFlags;

            n.Count = r.ReadInt32();

            n.TexCoords = new TexCoordSet[numTexCoordSets];
            for (var i = 0; i < numTexCoordSets; i++)
            {
                n.TexCoords[i] = TexCoordSet.Read(r, n.Count);
            }

            if (skinFlags != 0)
            {
                // DoData
                throw new NotSupportedException("Skin flags are not yet supported");
            }

            n.SetFlagBit(8, r.ReadBoolean());

            U01 = r.ReadArray<Iso4>();
        }

        public override void Write(CPlugVisual n, GbxWriter w)
        {
            w.Write(n.IsGeometryStatic);
            w.Write(n.IsIndexationStatic);

            w.Write(n.TexCoords.Length);

            // this might be wrong
            var skinFlags = n.Flags & 7;
            w.Write(skinFlags);

            w.Write(n.Count);

            for (var i = 0; i < n.TexCoords.Length; i++)
            {
                n.TexCoords[i].Write(w);
            }

            if (skinFlags != 0)
            {
                // DoData
                throw new NotSupportedException("Skin flags are presented");
            }

            w.Write(n.IsFlagBitSet(8));
            w.WriteArray(U01);
        }
    }

    public partial class Chunk0900600A
    {
        public Iso4[]? U01;

        public override void Read(CPlugVisual n, GbxReader r)
        {
            n.IsGeometryStatic = r.ReadBoolean();
            n.IsIndexationStatic = r.ReadBoolean();

            var numTexCoordSets = r.ReadInt32();

            var skinFlags = r.ReadInt32() & 7; // Skin (& 7 added for safety)
            n.Flags |= skinFlags;

            n.Count = r.ReadInt32();

            n.VertexStreams = r.ReadListNodeRef<CPlugVertexStream>()!;

            n.TexCoords = new TexCoordSet[numTexCoordSets];
            for (var i = 0; i < numTexCoordSets; i++)
            {
                n.TexCoords[i] = TexCoordSet.Read(r, n.Count);
            }

            if (skinFlags != 0)
            {
                // DoData
                throw new NotSupportedException("Skin flags are not yet supported");
            }

            n.SetFlagBit(8, r.ReadBoolean());

            U01 = r.ReadArray<Iso4>();
        }
    }

    public partial class Chunk0900600C
    {
        public Iso4[]? U01;

        public override void Read(CPlugVisual n, GbxReader r)
        {
            n.IsGeometryStatic = r.ReadBoolean();
            n.IsIndexationStatic = r.ReadBoolean();

            var numTexCoordSets = r.ReadInt32();

            var skinFlags = r.ReadInt32() & 7; // Skin (& 7 added for safety)
            n.Flags |= skinFlags;

            n.Count = r.ReadInt32();

            n.VertexStreams = r.ReadListNodeRef<CPlugVertexStream>()!;

            n.TexCoords = new TexCoordSet[numTexCoordSets];
            for (var i = 0; i < numTexCoordSets; i++)
            {
                n.TexCoords[i] = TexCoordSet.Read(r, n.Count);
            }

            if (skinFlags != 0)
            {
                // DoData
                throw new NotSupportedException("Skin flags are not yet supported");
            }

            n.SetFlagBit(8, r.ReadBoolean());

            n.BoundingBox = r.ReadBoxAligned();
        }
    }

    public partial class Chunk0900600D
    {
        public override void Read(CPlugVisual n, GbxReader r)
        {
            n.Flags = ConvertChunkFlagsToFlags(r.ReadInt32());
            var numTexCoordSets = r.ReadInt32();
            n.Count = r.ReadInt32();
            n.VertexStreams = r.ReadListNodeRef<CPlugVertexStream>()!;

            n.TexCoords = new TexCoordSet[numTexCoordSets];
            for (var i = 0; i < numTexCoordSets; i++)
            {
                n.TexCoords[i] = TexCoordSet.Read(r, n.Count);
            }

            if ((n.Flags & 7) != 0)
            {
                throw new Exception();
            }

            n.BoundingBox = r.ReadBoxAligned();
        }
    }

    public partial class Chunk0900600F
    {

    }

    public sealed class TexCoordSet : IVersionable
    {
        public int Version { get; set; }
        public TexCoord[] TexCoords { get; set; } = [];
        public int? Flags { get; set; }
        public float[]? U01 { get; set; }

        public static TexCoordSet Read(GbxReader r, int expectedCount)
        {
            var version = r.ReadInt32();
            var flags = default(int?);

            if (version >= 3)
            {
                var actualCount = r.ReadInt32();

                if (actualCount != expectedCount)
                {
                    throw new InvalidDataException("TexCoord actualCount != expectedCount");
                }

                flags = r.ReadInt32();

                if ((byte)flags > 2)
                {
                    throw new InvalidDataException("TexCoord flags kind > 2");
                }
            }

            var texCoords = new TexCoord[expectedCount];

            for (var i = 0; i < expectedCount; i++)
            {
                texCoords[i] = TexCoord.Read(r, version);
            }

            var u01 = default(float[]);

            if (version >= 3 && flags.HasValue)
            {
                u01 = r.ReadArray<float>(expectedCount * flags.Value & 0xFF);
            }

            return new TexCoordSet
            {
                Version = version,
                TexCoords = texCoords,
                Flags = flags,
                U01 = u01
            };
        }

        public void Write(GbxWriter w)
        {
            w.Write(Version);

            if (Version >= 3)
            {
                var flags = Flags.GetValueOrDefault(256);

                w.Write(TexCoords.Length);
                w.Write(flags);

                if ((byte)flags > 2)
                {
                    throw new InvalidDataException("TexCoord flags kind > 2");
                }
            }

            for (int i = 0; i < TexCoords.Length; i++)
            {
                TexCoords[i].Write(w, Version);
            }

            if (Flags.HasValue)
            {
                var expectedLength = TexCoords.Length * Flags.Value & 0xFF;

                var u01 = U01;

                if (u01 is null)
                {
                    u01 = new float[expectedLength];
                }
                else
                {
                    Array.Resize(ref u01, expectedLength);
                }

                w.WriteArray(u01);
            }
        }
    }

    public readonly record struct TexCoord(Vec2 UV, int? U01, int? U02)
    {
        public static TexCoord Read(GbxReader r, int version)
        {
            var uv = r.ReadVec2();
            var u01 = default(int?);
            var u02 = default(int?);

            if (version < 3 && version >= 1)
            {
                u01 = r.ReadInt32();

                if (version >= 2)
                {
                    u02 = r.ReadInt32();
                }
            }

            return new TexCoord(uv, u01, u02);
        }

        public void Write(GbxWriter w, int version)
        {
            w.Write(UV);

            if (version < 3 && version >= 1)
            {
                w.Write(U01.GetValueOrDefault());

                if (version >= 2)
                {
                    w.Write(U02.GetValueOrDefault());
                }
            }
        }
    }
}
