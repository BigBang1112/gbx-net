namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08055000</remarks>
[Node(0x08055000)]
public class CMotionDayTime : CMotion
{
    protected CMotionDayTime()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CMotionDayTime 0x000 chunk
    /// </summary>
    [Chunk(0x08055000)]
    public class Chunk08055000 : Chunk<CMotionDayTime>
    {
        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CMotionDayTime n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    #endregion
}