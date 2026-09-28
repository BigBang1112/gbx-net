namespace GBX.NET.Engines.Game;

/// <summary>
/// Block placed on a map.
/// </summary>
/// <remarks>ID: 0x03057000</remarks>
[Class(0x03057000)]
public partial class CGameCtnBlock : CMwNod, IClass, IReadable, IWritable
{
    [Hexadecimal] public static new uint Id => 0x03057000;



    public string? Author { get; set; }
    public CPlugCharPhySpecialProperty? PhyCharSpecialProperty { get; set; }
    public SSquareCardEventIds[]? SquareCardEventIds { get; set; }
    public int DecalIntensity { get; set; } = 1;
    public int DecalVariant { get; set; } = -1;

    public void Read(GbxReader r, int v = 0)
    {
        Name = r.ReadId();
        Direction = (Direction)r.ReadByte();
        Coord = r.ReadByte3();
        if (v == 0)
        {
            Flags = r.ReadInt16();
        }
        if (v >= 1)
        {
            Flags = r.ReadInt32();
        }
        if ((Flags&(1<<15))!=0)
        {
            Author = r.ReadId();
            Skin = r.ReadNodeRef<CGameCtnBlockSkin>();
        }
        if (v >= 2)
        {
            if ((Flags&(1<<19))!=0)
            {
                PhyCharSpecialProperty = r.ReadNodeRef<CPlugCharPhySpecialProperty>();
            }
            if ((Flags&(1<<20))!=0)
            {
                WaypointSpecialProperty = r.ReadNodeRef<CGameWaypointSpecialProperty>();
            }
            if ((Flags&(1<<18))!=0)
            {
                SquareCardEventIds = r.ReadArrayReadable<SSquareCardEventIds>();
            }
            if ((Flags&(1<<17))!=0)
            {
                DecalId = r.ReadId();
                DecalIntensity = r.ReadInt32();
                DecalVariant = r.ReadInt32();
            }
        }
    }

    public void Write(GbxWriter w, int v = 0)
    {
        w.WriteIdAsString(Name);
        w.Write((byte)Direction);
        w.Write((Byte3)Coord);
        if (v == 0)
        {
            w.Write((short)Flags);
        }
        if (v >= 1)
        {
            w.Write(Flags);
        }
        if ((Flags&(1<<15))!=0)
        {
            w.WriteIdAsString(Author);
            w.WriteNodeRef<CGameCtnBlockSkin>(Skin);
        }
        if (v >= 2)
        {
            if ((Flags&(1<<19))!=0)
            {
                w.WriteNodeRef<CPlugCharPhySpecialProperty>(PhyCharSpecialProperty);
            }
            if ((Flags&(1<<20))!=0)
            {
                w.WriteNodeRef<CGameWaypointSpecialProperty>(WaypointSpecialProperty);
            }
            if ((Flags&(1<<18))!=0)
            {
                w.WriteArrayWritable<SSquareCardEventIds>(SquareCardEventIds);
            }
            if ((Flags&(1<<17))!=0)
            {
                w.WriteIdAsString(DecalId);
                w.Write(DecalIntensity);
                w.Write(DecalVariant);
            }
        }
    }


    /// <summary>
    /// CGameCtnBlock 0x002 chunk
    /// </summary>
    [Chunk(0x03057002)]
    [ChunkGameVersion(GameVersion.TM10)]
    public partial class Chunk03057002 : Chunk<CGameCtnBlock>
    {
        /// <inheritdoc />
        public override uint Id => 0x03057002;

        /// <inheritdoc />
        public override GameVersion GameVersion => GameVersion.TM10;

    }


    public sealed partial class SSquareCardEventIds : IReadable, IWritable
    {
        public int U01 { get; set; }
        public int U02 { get; set; }
        public Ident[]? U03 { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            U01 = r.ReadInt32();
            U02 = r.ReadInt32();
            U03 = r.ReadArrayIdent();
        }

        public void Write(GbxWriter w, int v = 0)
        {
            w.Write(U01);
            w.Write(U02);
            w.WriteArray(U03);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03057002 => new Chunk03057002(),
        _ => base.NewChunk(chunkId),
    };
}
