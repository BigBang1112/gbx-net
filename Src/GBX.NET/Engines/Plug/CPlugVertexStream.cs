namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09056000</remarks>
[Node(0x09056000)]
public class CPlugVertexStream : CPlug
{
    private int count;
    private uint flags;
    private DataDecl[]? dataDecls;
    private CPlugVertexStream? streamModel;
    private GameBoxRefTable.File? streamModelFile;
    private Vec3[] vertices = Array.Empty<Vec3>();
    private Vec3[] normals = Array.Empty<Vec3>();
    private Vec2[]? uvs1;
    private Vec2[]? uvs2;
    private Vec2[]? uvs3;
    private Vec3[]? tangents1;
    private Vec3[]? tangents2;

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public int Count { get => count; set => count = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public uint Flags { get => flags; set => flags = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09056000>]
    public CPlugVertexStream? StreamModel
    {
        get => streamModel = GetNodeFromRefTable(streamModel, streamModelFile) as CPlugVertexStream;
        set => streamModel = value;
    }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public Vec3[] Vertices { get => vertices; set => vertices = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public Vec3[] Normals { get => normals; set => normals = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public Vec2[]? Uvs1 { get => uvs1; set => uvs1 = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public Vec2[]? Uvs2 { get => uvs2; set => uvs2 = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public Vec2[]? Uvs3 { get => uvs3; set => uvs3 = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public Vec3[]? Tangents1 { get => tangents1; set => tangents1 = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public Vec3[]? Tangents2 { get => tangents2; set => tangents2 = value; }

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
        public bool U04;
        private Vec3[]? U01;

        public int Version { get; set; }

        public override void ReadWrite(CPlugVertexStream n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            rw.Int32(ref n.count);
            rw.UInt32(ref n.flags); // DoData
            rw.NodeRef(ref n.streamModel, ref n.streamModelFile);

            if (n.count == 0 || n.streamModel is not null || n.streamModelFile is not null)
            {
                return;
            }

            if (Version == 0)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            rw.ArrayArchive<DataDecl>(ref n.dataDecls!, n.count);

            rw.Boolean(ref U04);

            foreach (var decl in n.dataDecls)
            {
                switch (decl.U01)
                {
                    case 4195328:
                    case 5243904:
                    case 6292480:
                    case 8389632:
                    case 10486784:
                    case 12583936:
                        rw.Array<Vec3>(ref n.vertices!, n.count);
                        break;
                    case 272636933:
                    case 273685509:
                    case 274734085:
                    case 276831237:
                    case 278928389:
                    case 281025541:
                        rw.ArrayVec3_10b(ref n.normals!, n.count);
                        break;
                    case 542115848:
                        rw.ArrayVec3_10b(ref U01, n.count);
                        break;
                    case 542116356:
                        rw.ArrayVec3_10b(ref U01, n.count);
                        break;
                    case 542114314: // this one has dupes?
                    case 543162890:
                    case 545260042: // uv match1
                    case 547357194: // uv match2
                    case 549454346: // uv match3
                        rw.Array<Vec2>(ref n.uvs1!, count: n.count);
                        break;
                    case 545260043: // uv match1
                    case 547357195: // uv match2
                    case 549454347: // uv match3
                        rw.Array<Vec2>(ref n.uvs2!, count: n.count);
                        break;
                    case 549454348:
                        rw.Array<Vec2>(ref n.uvs3!, count: n.count);
                        break;
                    case 278928402: // t match1
                    case 281025554: // t match2
                        rw.ArrayVec3_10b(ref n.tangents1!, n.count);
                        break;
                    case 278928404: // t match1
                    case 281025556: // t match2
                        rw.ArrayVec3_10b(ref n.tangents2!, n.count);
                        break;
                }
            }
        }
    }

    #endregion

    public class DataDecl : IReadableWritable
    {
        public uint U01;
        public uint U02;
        public ushort? U03;
        public ushort Offset;
        
        public void ReadWrite(GameBoxReaderWriter rw, int version = 0) // version is not really version in this case
        {
            rw.UInt32(ref U01);
            rw.UInt32(ref U02);

            if ((U02 & 0xffc) != 0)
            {
                rw.UInt16(ref U03);
                rw.UInt16(ref Offset);

                if (((ushort)(U02 >> 2) & 0x3ff) != Offset)
                {
                    throw new Exception("Invalid offset");
                }
            }
        }
    }
}