namespace GBX.NET.Engines.Game;

[Node(0x03036000)]
public class CGameCtnBlockUnitInfo : CMwNod
{
    private int placePylons;
    private Int3 relativeOffset;
    private CMwNod?[]? clips;
    private bool underground;
    private string? terrainModifierId;

    public int PlacePylons
    {
        get => placePylons;
        set => placePylons = value;
    }

    public Int3 RelativeOffset
    {
        get => relativeOffset;
        set => relativeOffset = value;
    }

    public CMwNod?[]? Clips
    {
        get => clips;
        set => clips = value;
    }

    public bool Underground
    {
        get
        {
            DiscoverChunk<Chunk03036002>();
            return underground;
        }
        set
        {
            DiscoverChunk<Chunk03036002>();
            underground = value;
        }
    }

    public string? TerrainModifierId
    {
        get => terrainModifierId;
        set => terrainModifierId = value;
    }

    public override string ToString() => "Block unit at " + relativeOffset.ToString();

    protected CGameCtnBlockUnitInfo()
    {

    }

    [Chunk(0x03036000)]
    public class Chunk03036000 : Chunk<CGameCtnBlockUnitInfo>
    {
        public bool U01;
        public bool U02;

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.placePylons);
            rw.Boolean(ref U01); // AcceptPylons?
            rw.Boolean(ref U02);
            rw.Int3(ref n.relativeOffset);
            rw.ArrayNode(ref n.clips);
        }
    }

    [Chunk(0x03036001)]
    public class Chunk03036001 : Chunk<CGameCtnBlockUnitInfo>
    {
        public string? U01;
        public float U02;
        public float U03;

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01); // Desert, Grass
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }

    [Chunk(0x03036002)]
    public class Chunk03036002 : SkippableChunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.underground);
        }
    }

    [Chunk(0x03036003)]
    public class Chunk03036003 : Chunk<CGameCtnBlockUnitInfo>
    {
        public CMwNod? U01;
        public string? U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
            rw.Id(ref U02); // TerrainModifierId?
            rw.Int32(ref U03);
            rw.Int32(ref U04);
        }
    }

    [Chunk(0x03036004)]
    public class Chunk03036004 : Chunk<CGameCtnBlockUnitInfo>
    {
        public int U01;

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x03036005)]
    public class Chunk03036005 : Chunk<CGameCtnBlockUnitInfo>
    {
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.terrainModifierId);
        }
    }

    // incomplete

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
