namespace GBX.NET.Engines.Game;

[Node(0x03121000)]
public class CGameCtnSolidDecals : CMwNod
{
    private string? name;
    private string? typeId;
    private int typeIntensity;
    private int decalFrequency;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03121002>]
    public string? Name { get => name; set => name = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03121003>]
    public string? TypeId { get => typeId; set => typeId = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03121003>]
    public int TypeIntensity { get => typeIntensity; set => typeIntensity = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk03121004>]
    public int DecalFrequency { get => decalFrequency; set => decalFrequency = value; }

    internal CGameCtnSolidDecals()
    {

    }

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnSolidDecals 0x001 chunk
    /// </summary>
    [Chunk(0x03121001)]
    public class Chunk03121001 : Chunk<CGameCtnSolidDecals>
    {
        public int U01;
        public byte[]? U02;

        public override void ReadWrite(CGameCtnSolidDecals n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Bytes(ref U02);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnSolidDecals 0x002 chunk
    /// </summary>
    [Chunk(0x03121002)]
    public class Chunk03121002 : Chunk<CGameCtnSolidDecals>
    {
        public override void ReadWrite(CGameCtnSolidDecals n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.name);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnSolidDecals 0x003 chunk
    /// </summary>
    [Chunk(0x03121003)]
    public class Chunk03121003 : Chunk<CGameCtnSolidDecals>
    {
        public override void ReadWrite(CGameCtnSolidDecals n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.typeId);
            rw.Int32(ref n.typeIntensity);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CGameCtnSolidDecals 0x004 chunk
    /// </summary>
    [Chunk(0x03121004)]
    public class Chunk03121004 : Chunk<CGameCtnSolidDecals>
    {
        public override void ReadWrite(CGameCtnSolidDecals n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.decalFrequency);
        }
    }

    #endregion
}
