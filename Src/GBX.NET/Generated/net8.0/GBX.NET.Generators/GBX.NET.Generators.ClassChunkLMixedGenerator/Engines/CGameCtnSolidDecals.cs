namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03121000</remarks>
[Class(0x03121000)]
public partial class CGameCtnSolidDecals : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03121000;




    private string? name;
    [AppliedWithChunk<Chunk03121002>]
    public string? Name { get => name; set => name = value; }

    private string? typeId;
    [AppliedWithChunk<Chunk03121003>]
    public string? TypeId { get => typeId; set => typeId = value; }

    private int typeIntensity;
    [AppliedWithChunk<Chunk03121003>]
    public int TypeIntensity { get => typeIntensity; set => typeIntensity = value; }

    private int decalFrequency;
    [AppliedWithChunk<Chunk03121004>]
    public int DecalFrequency { get => decalFrequency; set => decalFrequency = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameCtnSolidDecals"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameCtnSolidDecals() { }


    /// <summary>
    /// CGameCtnSolidDecals 0x001 chunk
    /// </summary>
    [Chunk(0x03121001)]
    public partial class Chunk03121001 : Chunk<CGameCtnSolidDecals>
    {
        /// <inheritdoc />
        public override uint Id => 0x03121001;

        public int U01;
        public byte[]? U02;

        public override void ReadWrite(CGameCtnSolidDecals n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Data(ref U02);
        }
    }

    /// <summary>
    /// CGameCtnSolidDecals 0x002 chunk
    /// </summary>
    [Chunk(0x03121002)]
    public partial class Chunk03121002 : Chunk<CGameCtnSolidDecals>
    {
        /// <inheritdoc />
        public override uint Id => 0x03121002;


        public override void ReadWrite(CGameCtnSolidDecals n, GbxReaderWriter rw)
        {
            rw.String(ref n.name);
        }
    }

    /// <summary>
    /// CGameCtnSolidDecals 0x003 chunk
    /// </summary>
    [Chunk(0x03121003)]
    public partial class Chunk03121003 : Chunk<CGameCtnSolidDecals>
    {
        /// <inheritdoc />
        public override uint Id => 0x03121003;


        public override void ReadWrite(CGameCtnSolidDecals n, GbxReaderWriter rw)
        {
            rw.Id(ref n.typeId);
            rw.Int32(ref n.typeIntensity);
        }
    }

    /// <summary>
    /// CGameCtnSolidDecals 0x004 chunk
    /// </summary>
    [Chunk(0x03121004)]
    public partial class Chunk03121004 : Chunk<CGameCtnSolidDecals>
    {
        /// <inheritdoc />
        public override uint Id => 0x03121004;


        public override void ReadWrite(CGameCtnSolidDecals n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.decalFrequency);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03121001 => new Chunk03121001(),
        0x03121002 => new Chunk03121002(),
        0x03121003 => new Chunk03121003(),
        0x03121004 => new Chunk03121004(),
        _ => base.NewChunk(chunkId),
    };
}
