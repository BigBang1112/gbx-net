namespace GBX.NET.Engines.Game;

[Node(0x03036000)]
public class CGameCtnBlockUnitInfo : CMwNod
{
    private int placePylons;
    private int acceptPylons;
    private Int3 relativeOffset;
    private CGameCtnBlockInfoClip?[]? clips;
    private bool underground;
    private string? terrainModifierId;

    [NodeMember(ExactlyNamed = true)]
    public int PlacePylons { get => placePylons; set => placePylons = value; }

    [NodeMember(ExactlyNamed = true)]
    public int AcceptPylons { get => acceptPylons; set => acceptPylons = value; }

    [NodeMember(ExactlyNamed = true)]
    public Int3 RelativeOffset { get => relativeOffset; set => relativeOffset = value; }

    public CGameCtnBlockInfoClip?[]? Clips { get => clips; set => clips = value; }

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

    public string? TerrainModifierId { get => terrainModifierId; set => terrainModifierId = value; }

    public override string ToString() => $"{base.ToString()} {{ {relativeOffset} }}";

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
            rw.ArrayNode<CGameCtnBlockInfoClip>(ref n.clips);
        }
    }

    [Chunk(0x03036001)]
    public class Chunk03036001 : Chunk<CGameCtnBlockUnitInfo>
    {
        public string? U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01); // Desert, Grass
            rw.Int32(ref U02);
            rw.Int32(ref U03);
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
        public override void ReadWrite(CGameCtnBlockUnitInfo n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.acceptPylons);
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
}
