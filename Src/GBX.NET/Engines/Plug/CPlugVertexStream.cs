namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09056000</remarks>
[Node(0x09056000)]
public class CPlugVertexStream : CPlug
{
    private DataDecl[]? dataDecls;

    internal CPlugVertexStream()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CPlugVertexStream 0x000 chunk
    /// </summary>
    [Chunk(0x09056000)]
    public class Chunk09056000 : Chunk<CPlugVertexStream>, IVersionable
    {
        private int version;
        
        public int U01;
        public uint U02;
        public Node? U03;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugVertexStream n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref U01);
            rw.UInt32(ref U02); // DoData
            rw.NodeRef(ref U03);

            if (U01 != 0 && U03 is null)
            {
                rw.ArrayArchive<DataDecl>(ref n.dataDecls);
            }
        }
    }

    #endregion

    public class DataDecl : IReadableWritable
    {
        public ulong U01;

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.UInt64(ref U01); // DoData
            // ...
        }
    }
}