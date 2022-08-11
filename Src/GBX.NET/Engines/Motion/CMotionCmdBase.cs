namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08029000</remarks>
[Node(0x08029000)]
public class CMotionCmdBase : CMwCmd
{
    protected CMotionCmdBase()
    {
        
    }
    
    #region 0x002 chunk

    /// <summary>
    /// CMotionCmdBase 0x002 chunk
    /// </summary>
    [Chunk(0x08029002)]
    public class Chunk08029002 : Chunk<CMotionCmdBase>
    {
        public int U01;
        public float U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;

        public override void ReadWrite(CMotionCmdBase n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
        }
    }

    #endregion

}
