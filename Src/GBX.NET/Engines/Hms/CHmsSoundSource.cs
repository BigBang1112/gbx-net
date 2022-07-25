namespace GBX.NET.Engines.Hms;

/// <remarks>ID: 0x0600D000</remarks>
[Node(0x0600D000)]
public class CHmsSoundSource : CMwNod
{
    protected CHmsSoundSource()
    {

    }

    #region 0x002 chunk

    /// <summary>
    /// CHmsSoundSource 0x002 chunk
    /// </summary>
    [Chunk(0x0600D002)]
    public class Chunk0600D002 : Chunk<CHmsSoundSource>
    {
        public Node? U01;
        public GameBoxRefTable.File? U01file;
        public int U02;
        public bool U03;

        public override void ReadWrite(CHmsSoundSource n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01, ref U01file); // sound
            rw.Int32(ref U02);
            rw.Boolean(ref U03);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CHmsSoundSource 0x003 chunk
    /// </summary>
    [Chunk(0x0600D003)]
    public class Chunk0600D003 : Chunk<CHmsSoundSource>
    {
        public Vec3 U01;

        public override void ReadWrite(CHmsSoundSource n, GameBoxReaderWriter rw)
        {
            rw.Vec3(ref U01);
        }
    }

    #endregion
}