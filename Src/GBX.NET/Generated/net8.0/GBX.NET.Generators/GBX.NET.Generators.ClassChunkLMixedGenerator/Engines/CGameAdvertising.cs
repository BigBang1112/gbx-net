namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x0303D000</remarks>
[Class(0x0303D000)]
public partial class CGameAdvertising : CGameNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0303D000;




    private External<CMwNod>[]? files;
    [AppliedWithChunk<Chunk0303D001>]
    public External<CMwNod>[]? Files { get => files; set => files = value; }

    private External<CMwNod>[]? filesOrig;
    [AppliedWithChunk<Chunk0303D002>]
    public External<CMwNod>[]? FilesOrig { get => filesOrig; set => filesOrig = value; }

    private EMode mode;
    [AppliedWithChunk<Chunk0303D003>]
    public EMode Mode { get => mode; set => mode = value; }

    private string? radial_Config;
    [AppliedWithChunk<Chunk0303D004>]
    public string? Radial_Config { get => radial_Config; set => radial_Config = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameAdvertising"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameAdvertising() { }


    /// <summary>
    /// CGameAdvertising 0x001 chunk
    /// </summary>
    [Chunk(0x0303D001)]
    public partial class Chunk0303D001 : Chunk<CGameAdvertising>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303D001;


        public override void ReadWrite(CGameAdvertising n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CMwNod>(ref n.files!);
        }
    }

    /// <summary>
    /// CGameAdvertising 0x002 chunk
    /// </summary>
    [Chunk(0x0303D002)]
    public partial class Chunk0303D002 : Chunk<CGameAdvertising>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303D002;


        public override void ReadWrite(CGameAdvertising n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef<CMwNod>(ref n.filesOrig!);
        }
    }

    /// <summary>
    /// CGameAdvertising 0x003 chunk
    /// </summary>
    [Chunk(0x0303D003)]
    public partial class Chunk0303D003 : Chunk<CGameAdvertising>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303D003;


        public override void ReadWrite(CGameAdvertising n, GbxReaderWriter rw)
        {
            rw.EnumInt32<EMode>(ref n.mode);
        }
    }

    /// <summary>
    /// CGameAdvertising 0x004 chunk
    /// </summary>
    [Chunk(0x0303D004)]
    public partial class Chunk0303D004 : Chunk<CGameAdvertising>
    {
        /// <inheritdoc />
        public override uint Id => 0x0303D004;


        public override void ReadWrite(CGameAdvertising n, GbxReaderWriter rw)
        {
            rw.String(ref n.radial_Config);
        }
    }



    public enum EMode
    {
        Environement,
        Vehicle,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0303D001 => new Chunk0303D001(),
        0x0303D002 => new Chunk0303D002(),
        0x0303D003 => new Chunk0303D003(),
        0x0303D004 => new Chunk0303D004(),
        _ => base.NewChunk(chunkId),
    };
}
