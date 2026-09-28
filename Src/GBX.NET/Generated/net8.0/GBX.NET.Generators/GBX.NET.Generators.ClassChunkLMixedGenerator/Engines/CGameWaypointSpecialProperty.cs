namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E009000</remarks>
[Class(0x2E009000)]
public partial class CGameWaypointSpecialProperty : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x2E009000;




    private int spawn;
    [AppliedWithChunk<Chunk2E009000>]
    public int Spawn { get => spawn; set => spawn = value; }

    private int order;
    [AppliedWithChunk<Chunk2E009000>]
    [AppliedWithChunk<Chunk2E009000>]
    public int Order { get => order; set => order = value; }

    private string? tag;
    [AppliedWithChunk<Chunk2E009000>]
    public string? Tag { get => tag; set => tag = value; }


    /// <summary>
    /// CGameWaypointSpecialProperty 0x000 chunk
    /// </summary>
    [Chunk(0x2E009000)]
    public partial class Chunk2E009000 : Chunk<CGameWaypointSpecialProperty>, IVersionable
    {
        /// <inheritdoc />
        public override uint Id => 0x2E009000;

        public int Version { get; set; }


        public override void ReadWrite(CGameWaypointSpecialProperty n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            if (Version == 1)
            {
                rw.Int32(ref n.spawn);
                rw.Int32(ref n.order);
            }
            if (Version == 2)
            {
                rw.String(ref n.tag);
                rw.Int32(ref n.order);
            }
        }
    }

    /// <summary>
    /// CGameWaypointSpecialProperty 0x001 skippable chunk
    /// </summary>
    [Chunk(0x2E009001)]
    public partial class Chunk2E009001 : SkippableChunk<CGameWaypointSpecialProperty>
    {
        /// <inheritdoc />
        public override uint Id => 0x2E009001;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x2E009000 => new Chunk2E009000(),
        0x2E009001 => new Chunk2E009001(),
        _ => base.NewChunk(chunkId),
    };
}
