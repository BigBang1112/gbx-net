using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game;

[Node(0x0311D000)]
public sealed class CGameCtnZoneGenealogy : CMwNod
{
    #region Fields

    private string[]? zoneIds;
    private string? currentZoneId;
    private Direction dir;
    private int currentIndex;

    #endregion

    #region Properties

    [NodeMember]
    public string[]? ZoneIds
    {
        get => zoneIds;
        set => zoneIds = value;
    }

    [NodeMember]
    public string? CurrentZoneId
    {
        get => currentZoneId;
        set => currentZoneId = value;
    }

    [NodeMember]
    public Direction Dir
    {
        get => dir;
        set => dir = value;
    }

    [NodeMember]
    public int CurrentIndex
    {
        get => currentIndex;
        set => currentIndex = value;
    }

    #endregion

    #region Methods

    public override string ToString() => ZoneIds is null ? string.Empty : string.Join(" ", ZoneIds);

    #endregion

    #region Constructors

    private CGameCtnZoneGenealogy()
    {

    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    [Chunk(0x0311D001)]
    public class Chunk0311D001 : Chunk<CGameCtnZoneGenealogy>
    {
        public override void ReadWrite(CGameCtnZoneGenealogy n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.currentIndex);
            rw.EnumInt32<Direction>(ref n.dir);
        }
    }

    #endregion

    #region 0x002 chunk

    [Chunk(0x0311D002)]
    public class Chunk0311D002 : Chunk<CGameCtnZoneGenealogy>
    {
        public override void ReadWrite(CGameCtnZoneGenealogy n, GameBoxReaderWriter rw)
        {
            rw.Array(ref n.zoneIds,
                r => r.ReadId(),
                (x, w) => w.WriteId(x));
            rw.Int32(ref n.currentIndex); // 9
            rw.EnumInt32<Direction>(ref n.dir);
            rw.Id(ref n.currentZoneId);
        }
    }

    #endregion

    #endregion
}
