namespace GBX.NET.Engines.Motion;

[Node(0x804C000)]
public class CMotionEmitterLeaves : CMotion
{
    protected CMotionEmitterLeaves()
    {

    }

    #region 0x001 chunk

    /// <summary>
    /// CMotionEmitterLeaves 0x001 chunk
    /// </summary>
    [Chunk(0x0804C001)]
    public class Chunk0804C001 : Chunk<CMotionEmitterLeaves>
    {
        public Node? U01;
        public GameBoxRefTable.File? U01File;
        public float U02; 
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public float U07;

        public override void ReadWrite(CMotionEmitterLeaves n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01, ref U01File); // ManagerModel? CMotionManagerLeaves with MobilLeaves?
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Single(ref U07);
        }
    }

    #endregion
}
