namespace GBX.NET.Engines.Game;

[Node(0x03095000)]
public partial class CGamePlayerOfficialScores : CMwNod
{
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03095000))]
    [AppliedWithChunk(typeof(Chunk03095001))]
    public FilteredPlayerRank[]? FilteredPlayerRanks { get; private set; }

    protected CGamePlayerOfficialScores()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CGamePlayerOfficialScores 0x000 chunk
    /// </summary>
    [Chunk(0x03095000)]
    public class Chunk03095000 : Chunk<CGamePlayerOfficialScores>
    {
        public bool U01;
        public int U02;
        public int U03;
        public int U04;
        public bool U05;

        public override void Read(CGamePlayerOfficialScores n, GameBoxReader r)
        {
            // CTransactionalNatural::Archive
            U01 = r.ReadBoolean(asByte: true);
            if (U01)
            {
                U02 = r.ReadInt32();
                U03 = r.ReadInt32();
                U04 = r.ReadInt32();
                U05 = r.ReadBoolean();
            }
            //

            n.FilteredPlayerRanks = r.ReadArray<FilteredPlayerRank>(r => new(
                zone: r.ReadString(),
                u01: r.ReadBoolean(),
                score: r.ReadNodeRef<CGameHighScore>()));
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CGamePlayerOfficialScores 0x001 chunk
    /// </summary>
    [Chunk(0x03095001)]
    public class Chunk03095001 : Chunk<CGamePlayerOfficialScores>
    {
        public bool U01;
        public int U02;
        public int U03;
        public int U04;
        public bool U05;
        public bool U06;
        public ulong U07;
        public byte U08;
        public ulong U09;

        public override void Read(CGamePlayerOfficialScores n, GameBoxReader r)
        {
            // CTransactionalNatural::Archive
            U01 = r.ReadBoolean(asByte: true);
            if (U01)
            {
                U02 = r.ReadInt32();
                U03 = r.ReadInt32();
                U04 = r.ReadInt32();
                U05 = r.ReadBoolean();
            }
            //

            // CGameScoresVersion::ArchiveScoresVersion
            U06 = r.ReadBoolean(asByte: true);
            if (U06)
            {
                U07 = r.ReadUInt64(); // SSystemTime
                U08 = r.ReadByte();
            }
            //

            U09 = r.ReadUInt64(); // SSystemTime

            n.FilteredPlayerRanks = r.ReadArray<FilteredPlayerRank>(r => new(
                zone: r.ReadString(),
                u01: r.ReadBoolean(),
                score: r.ReadNodeRef<CGameHighScore>()));
        }
    }

    #endregion
}
