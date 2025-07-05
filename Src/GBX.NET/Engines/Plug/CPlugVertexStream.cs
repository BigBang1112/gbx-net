using GBX.NET.Components;

namespace GBX.NET.Engines.Plug;

public partial class CPlugVertexStream
{
    private int count;
    private uint flags;
    private CPlugVertexStream? streamModel;
    private GbxRefTableFile? streamModelFile;
    private DataDecl[]? dataDecls;
    private SortedDictionary<int, Vec2[]> uvs = [];
    private Vec2[]? blendWeight;
    private Vec3[]? positions;
    private Vec3[]? positions2;
    private Vec3[]? normals;
    private Vec3[]? normals2;
    private SortedDictionary<int, int[]> colors = [];
    private int[]? blendIndices;
    private Vec3[]? tangentUs;
    private Vec3[]? tangentVs;

    public Vec3[]? Positions { get => positions; set => positions = value; }
    public SortedDictionary<int, Vec2[]> UVs { get => uvs; set => uvs = value; }
    public Vec3[]? Normals { get => normals; set => normals = value; }
    public SortedDictionary<int, int[]> Colors { get => colors; set => colors = value; }

    public partial class Chunk09056000 : IVersionable
    {
        public int Version { get; set; }

        public bool U01;

        public override void ReadWrite(CPlugVertexStream n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            rw.Int32(ref n.count);
            rw.UInt32(ref n.flags); // DoData
            rw.NodeRef<CPlugVertexStream>(ref n.streamModel, ref n.streamModelFile);

            if (n.count == 0 || n.streamModel is not null || n.streamModelFile is not null)
            {
                return;
            }

            // version and count are stored in the same int, as more than 1 argument is not possible
            rw.ArrayReadableWritable<DataDecl>(ref n.dataDecls!, version: Version | (n.count << 3));

            if (Version == 0)
            {
                return;
            }

            rw.Boolean(ref U01);

            foreach (var decl in n.dataDecls ?? [])
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
                                if (rw.Reader is not null) n.uvs ??= [];
                                _ = n.uvs.TryGetValue(uvsIndex, out var uvs);
                                rw.Array<Vec2>(ref uvs, n.count);
                                n.uvs[uvsIndex] = uvs ?? throw new Exception("Null when it shouldn't be");
                                break;
                            case EPlugVDcl.BlendWeight:
                                rw.Array<Vec2>(ref n.blendWeight!, n.count);
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
                            case EPlugVDcl.Position1:
                                rw.Array<Vec3>(ref n.positions2!, n.count);
                                break;
                            case EPlugVDcl.Normal:
                                rw.ArrayVec3_10b(ref n.normals!, n.count);
                                break;
                            case EPlugVDcl.Normal1:
                                rw.ArrayVec3_10b(ref n.normals2!, n.count);
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
                                if (rw.Reader is not null) n.colors ??= [];
                                _ = n.colors.TryGetValue(colorsIndex, out var colors);
                                rw.Array<int>(ref colors, n.count);
                                n.colors[colorsIndex] = colors ?? throw new Exception("Null when it shoudlnt be");
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

    public partial class DataDecl
    {
        private uint flags1;
        public uint Flags1 { get => flags1; set => flags1 = value; }

        private uint flags2;
        public uint Flags2 { get => flags2; set => flags2 = value; }

        private byte[]? u01;
        public byte[]? U01 { get => u01; set => u01 = value; }

        private ushort u02;
        public ushort U02 { get => u02; set => u02 = value; }

        private ushort offset;
        public ushort Offset { get => offset; set => offset = value; }

        public EPlugVDcl WeightCount => (EPlugVDcl)(flags1 & 0x1FF);
        public EPlugVDclType Type => (EPlugVDclType)((flags1 >> 9) & 0x1FF);
        public EPlugVDclSpace Space => (EPlugVDclSpace)((flags1 >> 28) & 0xF);

        public override string ToString()
        {
            return $"{Type} {Space} {WeightCount}";
        }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            // unpacking that integer
            var version = v & 7;
            var count = v >> 3;

            rw.UInt32(ref flags1);
            rw.UInt32(ref flags2);

            if ((Flags2 & 0xFFC) == 0)
            {
                if (version == 0)
                {
                    rw.Data(ref u01, (int)(flags1 >> 0x12 & 0x3FF) * count);
                }
            }
            else
            {
                rw.UInt16(ref u02);
                rw.UInt16(ref offset);
                if (((ushort)(Flags2 >> 2) & 0x3FF) != Offset)
                {
                    throw new Exception("Offset mismatch");
                }
            }
        }
    }
}
