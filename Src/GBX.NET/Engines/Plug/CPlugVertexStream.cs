namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09056000</remarks>
[Node(0x09056000)]
public class CPlugVertexStream : CPlug
{
    public enum EPlugVDclType
    {
        U01,
        Float2,
        Float3,
        Float4,
        Color, // 4 bytes RGBA
        Int32,
        Dec3N = 14
    }

    public enum EPlugVDcl
    {
        Position,
        Position1,
        TgtRotation,
        BlendWeight,
        BlendIndices,
        Normal,
        Normal1,
        PointSize,
        Color0,
        Color1,
        TexCoord0,
        TexCoord1,
        TexCoord2,
        TexCoord3,
        TexCoord4,
        TexCoord5,
        TexCoord6,
        TexCoord7,
        TangentU,
        TangentU1,
        TangentV,
        TangentV1,
        Color2
    }

    public enum EPlugVDclSpace
    {
        Global3D,
        Local3D,
        Global2D
    }
    
    private int count;
    private uint flags;
    private DataDecl[]? dataDecls;
    private CPlugVertexStream? streamModel;
    private GameBoxRefTable.File? streamModelFile;
    private Vec3[] positions = Array.Empty<Vec3>();
    private Vec3[] normals = Array.Empty<Vec3>();
    private SortedDictionary<int, Vec2[]> uvs = new();
    private SortedDictionary<int, int[]> colors = new();
    private int[]? blendIndices;
    private Vec3[]? tangentUs;
    private Vec3[]? tangentVs;

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

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09056000>]
    public DataDecl[]? DataDecls { get => dataDecls; set => dataDecls = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public Vec3[] Positions { get => positions; set => positions = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public Vec3[] Normals { get => normals; set => normals = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public SortedDictionary<int, Vec2[]> UVs { get => uvs; set => uvs = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public SortedDictionary<int, int[]> Colors { get => colors; set => colors = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public int[]? BlendIndices { get => blendIndices; set => blendIndices = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public Vec3[]? TangentUs { get => tangentUs; set => tangentUs = value; }
    
    [NodeMember]
    [AppliedWithChunk<Chunk09056000>]
    public Vec3[]? TangentVs { get => tangentVs; set => tangentVs = value; }

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
                switch (decl.Type)
                {
                    case EPlugVDclType.Float2:
                        switch (decl.WeightCount)
                        {
                            case EPlugVDcl.TexCoord0:
                            case EPlugVDcl.TexCoord1:
                            case EPlugVDcl.TexCoord2:
                            case EPlugVDcl.TexCoord3:
                            case EPlugVDcl.TexCoord4:
                            case EPlugVDcl.TexCoord5:
                            case EPlugVDcl.TexCoord6:
                            case EPlugVDcl.TexCoord7:
                                var uvsIndex = (int)decl.WeightCount - (int)EPlugVDcl.TexCoord0;
                                if (rw.Reader is not null) n.UVs = new();
                                _ = n.UVs.TryGetValue(uvsIndex, out var uvs);
                                rw.Array<Vec2>(ref uvs, n.count);
                                n.UVs[uvsIndex] = uvs ?? throw new Exception("Null when it shouldn't be");
                                break;
                            default:
                                throw new Exception($"Unknown Float2 ({decl.WeightCount})");
                        }
                        break;
                    case EPlugVDclType.Float3:
                        switch (decl.WeightCount)
                        {
                            case EPlugVDcl.Position:
                                rw.Array<Vec3>(ref n.positions!, n.count);
                                break;
                            default:
                                throw new Exception($"Unknown Float3 ({decl.WeightCount})");
                        }
                        break;
                    case EPlugVDclType.Float4:
                        throw new Exception($"Unknown Float4 ({decl.WeightCount})");
                    case EPlugVDclType.Color:
                        switch (decl.WeightCount)
                        {
                            case EPlugVDcl.Color0:
                            case EPlugVDcl.Color1:
                            case EPlugVDcl.Color2:
                                var colorsIndex = (int)decl.WeightCount - (int)EPlugVDcl.Color0;
                                if (rw.Reader is not null) n.Colors = new();
                                _ = n.Colors.TryGetValue(colorsIndex, out var colors);
                                rw.Array<int>(ref colors, n.count);
                                n.Colors[colorsIndex] = colors ?? throw new Exception("Null when it shoudlnt be");
                                break;
                            default:
                                throw new Exception($"Unknown Color ({decl.WeightCount})");
                        }
                        break;
                    case EPlugVDclType.Int32:
                        switch (decl.WeightCount)
                        {
                            case EPlugVDcl.BlendIndices:
                                rw.Array<int>(ref n.blendIndices, n.count);
                                break;
                            default:
                                throw new Exception($"Unknown Int32 ({decl.WeightCount})");
                        }
                        break;
                    case EPlugVDclType.Dec3N:
                        switch (decl.WeightCount)
                        {
                            case EPlugVDcl.Normal:
                                rw.ArrayVec3_10b(ref n.normals!, n.count);
                                break;
                            case EPlugVDcl.TangentU:
                                rw.ArrayVec3_10b(ref n.tangentUs!, n.count);
                                break;
                            case EPlugVDcl.TangentV:
                                rw.ArrayVec3_10b(ref n.tangentVs!, n.count);
                                break;
                            default:
                                throw new Exception($"Unknown Dec3N ({decl.WeightCount})");
                        }
                        break;
                    default:
                        throw new NotSupportedException($"Unknown data decl type ({decl.WeightCount})");
                }
            }
        }
    }

    #endregion

    public class DataDecl : IReadableWritable
    {
        // DDDDXXXX XXXXXXBB BBBBBBBA AAAAAAAA
        // A - WeightCount
        // B - Type (Float2?, 2 = Float3, Float4?, 14 = Dec3N)
        // D - Space
        private uint flags1;

        // AAAAAAAAXX
        // A - Offset?
        private uint flags2;
        
        private ushort offset;

        public uint Flags1 { get => flags1; set => flags1 = value; }
        public uint Flags2 { get => flags2; set => flags2 = value; }
        public ushort Offset { get => offset; set => offset = value; }

        public EPlugVDcl WeightCount => (EPlugVDcl)(flags1 & 0x1FF);
        public EPlugVDclType Type => (EPlugVDclType)((flags1 >> 9) & 0x1FF);
        public EPlugVDclSpace Space => (EPlugVDclSpace)((flags1 >> 28) & 0xF);
        
        public ushort? U03;

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0) // version is not really version in this case
        {
            rw.UInt32(ref flags1);
            rw.UInt32(ref flags2);

            if ((flags2 & 0xFFC) != 0)
            {
                rw.UInt16(ref U03);
                rw.UInt16(ref offset);

                if (((ushort)(flags2 >> 2) & 0x3FF) != Offset)
                {
                    throw new Exception("Invalid offset");
                }
            }
        }

        public override string ToString()
        {
            return $"{Type} {Space} {WeightCount}";
        }
    }
}