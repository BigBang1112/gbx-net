namespace GBX.NET.Engines.Game;

[Node(0x03047000)]
public class CGameHighScore : CMwNod
{
    protected CGameHighScore()
    {

    }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03047002))]
    public TimeInt32? Time { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03047002))]
    public int Rank { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03047002))]
    public int Count { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03047002))]
    public string? Name { get; private set; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk03047002))]
    public string? Score { get; private set; }

    #region 0x002 chunk

    /// <summary>
    /// CGameHighScore 0x002 chunk
    /// </summary>
    [Chunk(0x03047002)]
    public class Chunk03047002 : Chunk<CGameHighScore>
    {
        public string? U01;

        public override void Read(CGameHighScore n, GameBoxReader r)
        {
            n.Time = r.ReadTimeInt32Nullable();
            n.Rank = r.ReadInt32();
            n.Count = r.ReadInt32();
            n.Name = r.ReadString();
            n.Score = r.ReadString();
            U01 = r.ReadString();
        }
    }

    #endregion
}
