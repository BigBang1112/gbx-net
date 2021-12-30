namespace GBX.NET.Engines.Game;

[Node(0x03036000)]
public class CGameCtnBlockUnitInfo : CMwNod
{
    public Int3 OffsetE { get; set; }
    public CGameCtnBlockInfoClip[]? Clips { get; set; }
    public string? TerrainModifierId { get; set; }

    public override string ToString() => OffsetE.ToString();

    protected CGameCtnBlockUnitInfo()
    {

    }

    [Chunk(0x03036000)]
    public class Chunk03036000 : Chunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
            rw.Int32();
            n.OffsetE = rw.Int3(n.OffsetE);
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
        }
    }

    [Chunk(0x03036001)]
    public class Chunk03036001 : Chunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Id(); // Desert, Grass
            rw.Single();
            rw.Single();
        }
    }

    [Chunk(0x03036003)]
    public class Chunk03036003 : Chunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
        }
    }

    [Chunk(0x03036004)]
    public class Chunk03036004 : Chunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
        }
    }

    [Chunk(0x03036005)]
    public class Chunk03036005 : Chunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            n.TerrainModifierId = rw.Id(n.TerrainModifierId);
        }
    }

    [Chunk(0x03036007)]
    public class Chunk03036007 : Chunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
        }
    }

    [Chunk(0x0303600B)]
    public class Chunk0303600B : Chunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
            rw.Int32();
        }
    }

    [Chunk(0x0303600C), AutoReadWriteChunk]
    public class Chunk0303600C : Chunk<CGameCtnBlockUnitInfo>
    {

    }
}
