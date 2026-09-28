namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03062000</remarks>
[Class(0x03062000)]
public partial class CGameSkillScoreComputer : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03062000;




    /// <summary>
    /// Creates a new instance of <see cref="CGameSkillScoreComputer"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CGameSkillScoreComputer() { }


    /// <summary>
    /// CGameSkillScoreComputer 0x000 chunk
    /// </summary>
    [Chunk(0x03062000)]
    public partial class Chunk03062000 : Chunk<CGameSkillScoreComputer>
    {
        /// <inheritdoc />
        public override uint Id => 0x03062000;

        public DateTime? U01;

        public override void ReadWrite(CGameSkillScoreComputer n, GbxReaderWriter rw)
        {
            rw.SystemTime(ref U01);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03062000 => new Chunk03062000(),
        _ => base.NewChunk(chunkId),
    };
}
