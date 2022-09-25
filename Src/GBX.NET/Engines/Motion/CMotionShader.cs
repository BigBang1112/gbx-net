namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x0802B000</remarks>
[Node(0x0802B000)]
public class CMotionShader : CMotionTrack
{
    internal CMotionShader()
    {

    }
    
    #region 0x000 chunk

    /// <summary>
    /// CMotionShader 0x000 chunk
    /// </summary>
    [Chunk(0x0802B000)]
    public class Chunk0802B000 : Chunk<CMotionShader>
    {
        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CMotionShader n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    #endregion
}
