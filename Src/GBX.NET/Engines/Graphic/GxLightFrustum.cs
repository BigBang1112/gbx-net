namespace GBX.NET.Engines.Graphic;

/// <remarks>ID: 0x0400A000</remarks>
[Node(0x0400A000)]
public class GxLightFrustum : GxLightBall
{
    internal GxLightFrustum()
    {

    }

    /// <summary>
    /// GxLightFrustum 0x004 chunk
    /// </summary>
    [Chunk(0x0400A004)]
    public class Chunk0400A004 : Chunk<GxLightFrustum>
    {
        private int U01;
        private int U02;
        private int U03;
        private float U04;
        private float U05;
        private float U06;
        private float U07;
        private int U08;

        public override void ReadWrite(GxLightFrustum n, GameBoxReaderWriter rw)
        {
            // May be incorrect

            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
            rw.Int32(ref U08);
        }
    }

    #region 0x006 chunk

    /// <summary>
    /// GxLightFrustum 0x006 chunk
    /// </summary>
    [Chunk(0x0400A006)]
    public class Chunk0400A006 : Chunk<GxLightFrustum>
    {
        public bool U01;
        public Box? U02;
        public uint U03;

        public override void ReadWrite(GxLightFrustum n, GameBoxReaderWriter rw)
        {
            // GmFrustum::ArchiveFrustum
            rw.Boolean(ref U01);
            rw.Box(ref U02); // 24 regular bytes if U01 is false
            //
            
            rw.UInt32(ref U03); // DoData
        }
    }

    #endregion
}
