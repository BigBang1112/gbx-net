namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03047000</remarks>
[Class(0x03047000)]
public partial class CGameHighScore : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03047000;




    private int time;
    [AppliedWithChunk<Chunk03047002>]
    public int Time { get => time; set => time = value; }

    private int rank;
    [AppliedWithChunk<Chunk03047002>]
    public int Rank { get => rank; set => rank = value; }

    private int count;
    [AppliedWithChunk<Chunk03047002>]
    public int Count { get => count; set => count = value; }

    private string? name;
    [AppliedWithChunk<Chunk03047002>]
    public string? Name { get => name; set => name = value; }

    private string? score;
    [AppliedWithChunk<Chunk03047002>]
    public string? Score { get => score; set => score = value; }

    private string? ghostUrl;
    [AppliedWithChunk<Chunk03047004>]
    public string? GhostUrl { get => ghostUrl; set => ghostUrl = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CGameHighScore"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameHighScore() { }


    /// <summary>
    /// CGameHighScore 0x002 chunk
    /// </summary>
    [Chunk(0x03047002)]
    public partial class Chunk03047002 : Chunk<CGameHighScore>
    {
        /// <inheritdoc />
        public override uint Id => 0x03047002;

        public string? U01;

        public override void ReadWrite(CGameHighScore n, GbxReaderWriter rw)
        {
            rw.Int32(ref n.time);
            rw.Int32(ref n.rank);
            rw.Int32(ref n.count);
            rw.String(ref n.name);
            rw.String(ref n.score);
            rw.String(ref U01);
        }
    }

    /// <summary>
    /// CGameHighScore 0x004 skippable chunk
    /// </summary>
    [Chunk(0x03047004)]
    public partial class Chunk03047004 : SkippableChunk<CGameHighScore>
    {
        /// <inheritdoc />
        public override uint Id => 0x03047004;

        public string? U01;

        public override void ReadWrite(CGameHighScore n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.String(ref n.ghostUrl);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03047002 => new Chunk03047002(),
        0x03047004 => new Chunk03047004(),
        _ => base.NewChunk(chunkId),
    };
}
