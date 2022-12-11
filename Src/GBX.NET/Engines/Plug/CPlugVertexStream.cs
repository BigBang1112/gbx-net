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
        public int U01;
        public uint U02;
        public Node? U03;
        public bool U04;

        public int Version { get; set; }

        public override void ReadWrite(CPlugVertexStream n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            rw.Int32(ref U01);
            rw.UInt32(ref U02); // DoData
            rw.NodeRef(ref U03);

            if (U01 == 0 || U03 is not null)
            {
                return;
            }
            
            if (Version == 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            rw.ArrayArchive<DataDecl>(ref n.dataDecls!, U01);

            rw.Boolean(ref U04);

            rw.UntilFacade(Unknown);
        }
    }

    #endregion

    public class DataDecl : IReadableWritable
    {
        public ulong U01;
        public ushort? U02;
        public ushort? U03;

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0) // version is not really version in this case
        {
            rw.UInt64(ref U01); // DoData

            if (((U01 >> 0x12) & 0x1ff) != 0)
            {
                rw.UInt16(ref U02);
                rw.UInt16(ref U03);
            }
        }
    }
}