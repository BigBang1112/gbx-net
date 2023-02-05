using System.Collections;

namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09006000</remarks>
[Node(0x09006000)]
[NodeExtension("Visual")]
public abstract class CPlugVisual : CPlug
{
    private const int isGeometryStaticBit = 3;
    private const int isIndexationStaticBit = 5;
    private const int hasVertexNormalsBit = 7;

    private Split[] splits = Array.Empty<Split>();
    private Int3[]? subVisuals;

    [NodeMember]
    [AppliedWithChunk<Chunk09006005>]
    public Int3[]? SubVisuals { get => subVisuals; set => subVisuals = value; }

    protected int Count { get; private set; }

    [NodeMember]
    [AppliedWithChunk<Chunk09006008>]
    [AppliedWithChunk<Chunk0900600A>]
    [AppliedWithChunk<Chunk0900600C>]
    [AppliedWithChunk<Chunk0900600D>]
    [AppliedWithChunk<Chunk0900600E>]
    [AppliedWithChunk<Chunk0900600F>]
    public int Flags { get; set; }

    [NodeMember]
    public bool IsGeometryStatic
    {
        get => IsFlagBitSet(isGeometryStaticBit);
        set => SetFlagBit(isGeometryStaticBit, value);
    }

    [NodeMember]
    public bool IsIndexationStatic
    {
        get => IsFlagBitSet(isIndexationStaticBit);
        set => SetFlagBit(isIndexationStaticBit, value);
    }

    [NodeMember]
    public bool HasVertexNormals
    {
        get => IsFlagBitSet(hasVertexNormalsBit);
        set => SetFlagBit(hasVertexNormalsBit, value);
    }

    [NodeMember]
    [AppliedWithChunk<Chunk09006008>]
    [AppliedWithChunk<Chunk0900600A>]
    [AppliedWithChunk<Chunk0900600C>]
    [AppliedWithChunk<Chunk0900600D>]
    [AppliedWithChunk<Chunk0900600E>]
    [AppliedWithChunk<Chunk0900600F>]
    public TexCoordSet[] TexCoords { get; set; } = Array.Empty<TexCoordSet>();

    [NodeMember]
    [AppliedWithChunk<Chunk0900600A>]
    [AppliedWithChunk<Chunk0900600C>]
    [AppliedWithChunk<Chunk0900600D>]
    [AppliedWithChunk<Chunk0900600E>]
    [AppliedWithChunk<Chunk0900600F>]
    public CPlugVertexStream[] VertexStreams { get; set; } = Array.Empty<CPlugVertexStream>();

    [NodeMember]
    [AppliedWithChunk<Chunk0900600B>]
    public Split[] Splits { get => splits; set => splits = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk0900600E>]
    [AppliedWithChunk<Chunk0900600F>]
    public BitmapElemToPack[] BitmapElemToPacks { get; set; } = Array.Empty<BitmapElemToPack>();

    [NodeMember]
    [AppliedWithChunk<Chunk0900600C>]
    [AppliedWithChunk<Chunk0900600D>]
    [AppliedWithChunk<Chunk0900600E>]
    [AppliedWithChunk<Chunk0900600F>]
    public Box BoundingBox { get; set; }

    internal CPlugVisual()
    {
        
    }

    public bool IsFlagBitSet(int bit) => (Flags & (1 << bit)) != 0;

    public void SetFlagBit(int bit, bool value)
    {
        if (value) Flags |= 1 << bit;
        else Flags &= ~(1 << bit);
    }
    
    private void ConvertChunkFlagsToFlags(int chunkFlags)
    {
        Flags = 0;
        Flags |= chunkFlags & 15;
        Flags |= (chunkFlags << 1) & 0x20;
        Flags |= (chunkFlags << 2) & 0x80;
        Flags |= (chunkFlags << 2) & 0x100;
        Flags |= (chunkFlags << 13) & 0x100000;
        Flags |= (chunkFlags << 13) & 0x200000;
        Flags |= (chunkFlags << 13) & 0x400000;
    }

    private int ConvertFlagsToChunkFlags()
    {        
        var chunkFlags = Flags & 15;
        chunkFlags |= (Flags >> 1) & 0x10;
        chunkFlags |= (Flags >> 2) & 0x20;
        chunkFlags |= (Flags >> 2) & 0x40;
        chunkFlags |= (Flags >> 13) & 0x80;
        chunkFlags |= (Flags >> 13) & 0x100;
        chunkFlags |= (Flags >> 13) & 0x200;
        return chunkFlags;
    }

    /// <summary>
    /// CPlugVisual 0x001 chunk
    /// </summary>
    [Chunk(0x09006001)]
    public class Chunk09006001 : Chunk<CPlugVisual>
    {
        public string U01 = "";

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01!);
        }
    }

    /// <summary>
    /// CPlugVisual 0x004 chunk
    /// </summary>
    [Chunk(0x09006004)]
    public class Chunk09006004 : Chunk<CPlugVisual>
    {
        public CMwNod? U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01); // sometimes not present
        }
    }

    /// <summary>
    /// CPlugVisual 0x005 chunk
    /// </summary>
    [Chunk(0x09006005)]
    public class Chunk09006005 : Chunk<CPlugVisual>
    {
        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Array<Int3>(ref n.subVisuals); // SSubVisual?
        }
    }

    /// <summary>
    /// CPlugVisual 0x006 chunk
    /// </summary>
    [Chunk(0x09006006)]
    public class Chunk09006006 : Chunk<CPlugVisual>
    {
        public override void Read(CPlugVisual n, GameBoxReader r)
        {
            n.HasVertexNormals = r.ReadBoolean();
        }

        public override void Write(CPlugVisual n, GameBoxWriter w)
        {
            w.Write(n.HasVertexNormals);
        }
    }

    /// <summary>
    /// CPlugVisual 0x007 chunk
    /// </summary>
    [Chunk(0x09006007)]
    public class Chunk09006007 : Chunk<CPlugVisual>
    {
        public bool U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CPlugVisual 0x008 chunk
    /// </summary>
    [Chunk(0x09006008)]
    public class Chunk09006008 : Chunk<CPlugVisual>
    {
        public Iso4[]? U01;

        public override void Read(CPlugVisual n, GameBoxReader r)
        {
            n.IsGeometryStatic = r.ReadBoolean();
            n.IsIndexationStatic = r.ReadBoolean();
            
            var numTexCoordSets = r.ReadInt32();

            var skinFlags = r.ReadInt32() & 7; // Skin (& 7 added for safety)
            n.Flags |= skinFlags;

            n.Count = r.ReadInt32();
            
            var type = GetType();

            if (type != typeof(Chunk09006008))
            {
                n.VertexStreams = r.ReadArray(r => r.ReadNodeRef<CPlugVertexStream>() ?? throw new Exception("Null VertexStream"));
            }

            n.TexCoords = r.ReadArray(numTexCoordSets, r => TexCoordSet.Read(r, n));

            if (skinFlags != 0)
            {
                // DoData
                throw new NotSupportedException("Skin flags are not yet supported");
            }

            n.SetFlagBit(8, r.ReadBoolean());
            
            if (type != typeof(Chunk0900600C))
            {
                U01 = r.ReadArray<Iso4>();
            }
        }

        public override void Write(CPlugVisual n, GameBoxWriter w)
        {
            w.Write(n.IsGeometryStatic);
            w.Write(n.IsIndexationStatic);

            w.Write(n.TexCoords.Length);

            var skinFlags = n.Flags & 7;
            w.Write(skinFlags);

            w.Write(n.Count);

            var type = GetType();
            
            if (type != typeof(Chunk09006008))
            {
                w.WriteArray(n.VertexStreams, (x, w) => w.Write(x));
            }

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

            if (type != typeof(Chunk0900600C))
            {
                w.WriteArray(U01);
            }
        }
    }

    /// <summary>
    /// CPlugVisual 0x009 chunk
    /// </summary>
    [Chunk(0x09006009)]
    public class Chunk09006009 : Chunk<CPlugVisual>
    {
        public float U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVisual 0x00A chunk
    /// </summary>
    [Chunk(0x0900600A)]
    public class Chunk0900600A : Chunk09006008
    {
        
    }

    /// <summary>
    /// CPlugVisual 0x00B chunk
    /// </summary>
    [Chunk(0x0900600B)]
    public class Chunk0900600B : Chunk<CPlugVisual>
    {
        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.ArrayArchive<Split>(ref n.splits!); // SSplit array
        }
    }

    [Chunk(0x0900600C)]
    public class Chunk0900600C : Chunk09006008
    {
        public override void Read(CPlugVisual n, GameBoxReader r)
        {
            base.Read(n, r);
            n.BoundingBox = r.ReadBox(); // ArchiveABox
        }

        public override void Write(CPlugVisual n, GameBoxWriter w)
        {
            base.Write(n, w);
            w.Write(n.BoundingBox);
        }
    }

    /// <summary>
    /// CPlugVisual 0x00D chunk
    /// </summary>
    [Chunk(0x0900600D)]
    public class Chunk0900600D : Chunk<CPlugVisual>
    {
        public override void Read(CPlugVisual n, GameBoxReader r)
        {
            n.ConvertChunkFlagsToFlags(r.ReadInt32());

            var numTexCoordSets = r.ReadInt32();
            
            n.Count = r.ReadInt32();
            n.VertexStreams = r.ReadArray(r => r.ReadNodeRef<CPlugVertexStream>() ?? throw new Exception("Null VertexStream"));
            n.TexCoords = r.ReadArray(numTexCoordSets, r => TexCoordSet.Read(r, n));
            
            if ((n.Flags & 7) != 0)
            {
                if (GetType() == typeof(Chunk0900600F))
                {
                    // CPlugVisual::ArchiveSkinData
                    throw new NotSupportedException("CPlugVisual::ArchiveSkinData");
                }
                else
                {
                    // DoData
                    throw new NotSupportedException("Skin flags are presented");
                }
            }
            
            n.BoundingBox = r.ReadBox(); // ArchiveABox
        }

        public override void Write(CPlugVisual n, GameBoxWriter w)
        {
            w.Write(n.ConvertFlagsToChunkFlags());
            
            w.Write(n.TexCoords.Length);

            w.Write(n.Count);
            w.WriteArray(n.VertexStreams, (x, w) => w.Write(x));

            for (var i = 0; i < n.TexCoords.Length; i++)
            {
                n.TexCoords[i].Write(w);
            }

            if ((n.Flags & 7) != 0)
            {
                if (GetType() == typeof(Chunk0900600F))
                {
                    // CPlugVisual::ArchiveSkinData
                    throw new NotSupportedException("CPlugVisual::ArchiveSkinData");
                }
                else
                {
                    // DoData
                    throw new NotSupportedException("Skin flags are presented");
                }
            }

            w.Write(n.BoundingBox);
        }
    }

    /// <summary>
    /// CPlugVisual 0x00E chunk
    /// </summary>
    [Chunk(0x0900600E)]
    public class Chunk0900600E : Chunk0900600D
    {
        public override void Read(CPlugVisual n, GameBoxReader r)
        {
            base.Read(n, r);
            n.BitmapElemToPacks = r.ReadArray(BitmapElemToPack.Read);
        }

        public override void Write(CPlugVisual n, GameBoxWriter w)
        {
            base.Write(n, w);
            w.WriteArray(n.BitmapElemToPacks, (x, w) => x.Write(w));
        }
    }

    #region 0x00F chunk

    /// <summary>
    /// CPlugVisual 0x00F chunk
    /// </summary>
    [Chunk(0x0900600F)]
    public class Chunk0900600F : Chunk0900600E, IVersionable
    {
        public ushort[]? U02;
        public int? U03;
        public int? U04;

        public int Version { get; set; }

        public override void Read(CPlugVisual n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            base.Read(n, r);

            if (Version >= 5)
            {
                U02 = r.ReadArray<ushort>();

                if (Version >= 6)
                {
                    U03 = r.ReadInt32();
                    U04 = r.ReadInt32();

                    n.Count = 0; // Extremely odd

                    if (U04 > 0)
                    {
                        throw new NotSupportedException("U04 > 0");
                    }
                }
            }
        }

        public override void Write(CPlugVisual n, GameBoxWriter w)
        {
            w.Write(Version);

            base.Write(n, w);

            if (Version >= 5)
            {
                w.WriteArray(U02);

                if (Version >= 6)
                {
                    w.Write(U03.GetValueOrDefault());
                    w.Write(U04.GetValueOrDefault());

                    if (U04 > 0)
                    {
                        throw new NotSupportedException("U04 > 0");
                    }
                }
            }
        }
    }

    #endregion

    #region 0x010 chunk

    /// <summary>
    /// CPlugVisual 0x010 chunk
    /// </summary>
    [Chunk(0x09006010)]
    public class Chunk09006010 : Chunk<CPlugVisual>, IVersionable
    {
        public int MorphCount;

        public int Version { get; set; }

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Int32(ref MorphCount); // Morph count
            
            if (MorphCount > 0)
            {
                throw new NotSupportedException("MorphCount > 0");
            }
        }
    }

    #endregion

    public readonly record struct TexCoord(Vec2 UV, int? U01, int? U02)
    {
        public static TexCoord Read(GameBoxReader r, int version)
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

        public void Write(GameBoxWriter w, int version)
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

    public class TexCoordSet : IVersionable, IReadOnlyCollection<TexCoord>
    {
        private readonly TexCoord[] texCoords;

        public int Version { get; set; }
        public int? Flags { get; set; }
        public float[]? U01 { get; set; }

        public int Count => texCoords.Length;
        
        public TexCoordSet(int version, TexCoord[] texCoords, int? flags, float[]? u01)
        {
            this.texCoords = texCoords;

            Version = version;
            Flags = flags;
            U01 = u01;
        }

        public static TexCoordSet Read(GameBoxReader r, CPlugVisual n)
        {
            var version = r.ReadInt32();
            var flags = default(int?);
            
            var count = n.Count;

            if (version >= 3)
            {
                var actualCount = r.ReadInt32();

                if (actualCount != count)
                {
                    throw new InvalidDataException("TexCoord actualCount != count");
                }

                flags = r.ReadInt32();

                if ((byte)flags > 2)
                {
                    throw new InvalidDataException("TexCoord flags kind > 2");
                }
            }
            
            var texCoords = new TexCoord[count];

            for (var i = 0; i < count; i++)
            {
                texCoords[i] = TexCoord.Read(r, version);
            }

            var u01 = default(float[]);

            if (flags.HasValue)
            {
                u01 = r.ReadArray<float>(count * flags.Value & 0xFF);
            }

            return new TexCoordSet(version, texCoords, flags, u01);
        }

        public void Write(GameBoxWriter w)
        {
            w.Write(Version);

            if (Version >= 3)
            {
                var flags = Flags.GetValueOrDefault(256);

                w.Write(Count);
                w.Write(flags);

                if ((byte)flags > 2)
                {
                    throw new InvalidDataException("TexCoord flags kind > 2");
                }
            }

            for (int i = 0; i < texCoords.Length; i++)
            {
                texCoords[i].Write(w, Version);
            }

            if (Flags.HasValue)
            {
                var expectedLength = Count * Flags.Value & 0xFF;

                var u01 = U01;

                if (u01 is null)
                {
                    u01 = new float[expectedLength];
                }
                else
                {
                    Array.Resize(ref u01, expectedLength);
                }

                w.WriteArray_NoPrefix(u01);
            }
        }

        public IEnumerator<TexCoord> GetEnumerator()
        {
            return ((IEnumerable<TexCoord>)texCoords).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return texCoords.GetEnumerator();
        }
    }

    public class Split : IReadableWritable
    {
        private int u01;
        private int u02;
        private Box boundingBox;

        public int U01 { get => u01; set => u01 = value; }
        public int U02 { get => u02; set => u02 = value; }
        public Box BoundingBox { get => boundingBox; set => boundingBox = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Box(ref boundingBox);
        }
    }

    public readonly record struct BitmapElemToPack(int U01, int U02, int U03, int U04, int U05)
    {
        public static BitmapElemToPack Read(GameBoxReader r)
        {
            return new(r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32());
        }

        public void Write(GameBoxWriter w)
        {
            w.Write(U01);
            w.Write(U02);
            w.Write(U03);
            w.Write(U04);
            w.Write(U05);
        }
    }
}