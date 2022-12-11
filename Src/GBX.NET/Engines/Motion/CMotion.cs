namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08001000</remarks>
[Node(0x08001000)]
public abstract class CMotion : CMwNod
{
    internal CMotion()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CMotion 0x000 chunk
    /// </summary>
    [Chunk(0x08001000)]
    public class Chunk08001000 : Chunk<CMotion>
    {
        public string? U01;

        public override void ReadWrite(CMotion n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    #endregion
}