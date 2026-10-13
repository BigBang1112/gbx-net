namespace GBX.NET.Engines.Plug;

public partial class CPlugVertexStream
{
    private SortedDictionary<int, Vec2[]> uvs = [];
    private SortedDictionary<int, int[]> colors = [];

    public Vec3[]? Positions { get; set; }
    public Vec3[]? Positions2 { get; set; }
    public Vec3[]? Normals { get; set; }
    public Vec3[]? Normals2 { get; set; }
    public Vec2[]? BlendWeight { get; set; }
    public int[]? BlendIndices { get; set; }
    public Vec3[]? TangentUs { get; set; }
    public Vec3[]? TangentUs2 { get; set; }
    public Vec3[]? TangentVs { get; set; }
    public Vec3[]? TangentVs2 { get; set; }
    public SortedDictionary<int, Vec2[]> UVs { get => uvs; set => uvs = value; }
    public SortedDictionary<int, int[]> Colors { get => colors; set => colors = value; }

    public bool IsStatic { get => (Flags & 1) != 0; set => SetFlag(1, value); }
    public bool SkipVision { get => (Flags & 8) != 0; set => SetFlag(8, value); }
    public bool DirtyVision { get => (Flags & 2) != 0; set => Flags = value ? Flags | 2 : Flags & ~2u; }
    public byte CompressTexCoords
    {
        get => (byte)(Flags >> 9);
        set
        {
            if (CompressTexCoords != value) Flags = (Flags & 0xFFFE01FF) | ((uint)value << 9) | 2;
        }
    }

    private void SetFlag(uint mask, bool value)
    {
        if (((Flags & mask) != 0) != value)
        {
            Flags = (value ? Flags | mask : Flags & ~mask) | 2;
        }
    }

    public partial class Chunk09056000
    {
        public Chunk09056000() : this(GameVersion.Unspecified) { }

        public Chunk09056000(GameVersion gameVersion)
        {
            Version = IsLegacyGame(gameVersion) ? -1
                : gameVersion is GameVersion.MP3 or GameVersion.MP4 or GameVersion.TM2020 ? 1 : 0;
        }

        private static bool IsLegacyGame(GameVersion game) => game is GameVersion.TMSX or GameVersion.TMNESWC or GameVersion.VSK5 or GameVersion.TMF;

        public override void ReadWrite(CPlugVertexStream n, GbxReaderWriter rw)
        {
            // Version -1 represents the unversioned format used before Maniaplanet.
            if (rw.Reader is { } reader && (IsLegacyGame(reader.Settings.GameVersion)
                || (reader.Settings.GameVersion == GameVersion.Unspecified && reader.ClassIdRemapMode != ClassIdRemapMode.Latest)))
            {
                Version = -1;
            }
            if (Version >= 0) rw.VersionInt32(this);
            if (Version is < -1 or > 1) throw new NotSupportedException($"Unsupported vertex stream version {Version}.");

            rw.Int32(ref n.vertexCount);
            if (n.VertexCount < 0) throw new InvalidDataException("Vertex count cannot be negative.");
            rw.DataUInt32(ref n.flags);
            rw.NodeRef(ref n.streamModelRef);
            if ((n.streamModelRef?.Kind ?? NodeRefKind.None) != NodeRefKind.None || (Version >= 0 && n.VertexCount == 0)) return;

            var declCount = rw.Int32(n.dataDecls?.Length ?? 0);
            if (declCount < 0 || declCount > (rw.Reader?.Settings.MaxDataSize ?? GbxReader.MaxDataSize) / 8)
            {
                throw new InvalidDataException("Invalid vertex declaration count.");
            }
            if (rw.Reader is not null) n.dataDecls = new DataDecl[declCount];
            if (rw.Writer is not null && rw.Reader is null)
            {
                for (var i = 0; i < declCount; i++)
                {
                    n.dataDecls![i].Legacy = Version < 0;
                    ValidateShared(n.dataDecls, i);
                }
                n.WriteGeometry(Version);
            }
            for (var i = 0; i < declCount; i++)
            {
                var decl = n.dataDecls![i] ??= new DataDecl();
                decl.VertexCount = n.VertexCount;
                decl.ReadWrite(rw, Version);
                ValidateShared(n.dataDecls, i);
            }
            if (Version >= 1)
            {
                rw.Boolean(ref n.compressLocalFloat3);
                foreach (var decl in n.dataDecls!)
                {
                    var size = decl.GetValueSize(n.CompressLocalFloat3);
                    decl.Data = rw.Data(decl.Data, checked(n.VertexCount * size));
                }
            }
            if (rw.Reader is not null) n.ReadGeometry(Version);
        }

        private static void ValidateShared(DataDecl[] declarations, int index)
        {
            var decl = declarations[index];
            if (decl.IsShared && (decl.SharedDeclIndex >= index || declarations[decl.SharedDeclIndex].IsShared
                || declarations[decl.SharedDeclIndex].Stride != decl.Stride))
            {
                throw new InvalidDataException("Shared declaration must refer to an earlier buffer with the same stride.");
            }
        }
    }

    private void ReadGeometry(int version)
    {
        foreach (var decl in dataDecls ?? [])
        {
            var buffer = version <= 0 && decl.IsShared ? dataDecls![decl.SharedDeclIndex].Data : decl.Data;
            if (buffer is null) continue;
            var packed = decl.Type == EPlugVDclType.Dec3N || (version >= 1 && CompressLocalFloat3
                && decl.Type == EPlugVDclType.Float3 && decl.Space == EPlugVDclSpace.Local3D);
            var offset = version <= 0 && decl.IsShared ? decl.Offset : 0;
            var stride = version <= 0 ? decl.Stride : decl.GetValueSize(CompressLocalFloat3);
            using var stream = new MemoryStream(buffer, writable: false);
            stream.Position = offset;
            using var reader = new GbxReader(stream);
            if (decl.Type is EPlugVDclType.Float3 or EPlugVDclType.Dec3N)
            {
                Vec3[] values;
                if (packed && stride == 4)
                {
                    values = Array.ConvertAll(reader.ReadArray<int>(VertexCount), static value =>
                        new Vec3((value << 22) >> 22, (value << 12) >> 22, (value << 2) >> 22) / 511f);
                }
                else if (!packed && stride == 12)
                {
                    values = reader.ReadArray<Vec3>(VertexCount);
                }
                else
                {
                    values = new Vec3[VertexCount];
                    for (var i = 0; i < values.Length; i++)
                    {
                        stream.Position = offset + i * stride;
                        values[i] = packed ? reader.ReadVec3_10b() : reader.ReadVec3();
                    }
                }
                SetVec3(decl.Semantic, values);
            }
            else if (decl.Type == EPlugVDclType.Float2)
            {
                var values = stride == 8 ? reader.ReadArray<Vec2>(VertexCount) : new Vec2[VertexCount];
                if (stride != 8)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        stream.Position = offset + i * stride;
                        values[i] = reader.ReadVec2();
                    }
                }
                if (decl.Semantic == EPlugVDcl.BlendWeight) BlendWeight = values;
                else if (decl.Semantic is >= EPlugVDcl.TexCoord0 and <= EPlugVDcl.TexCoord7) uvs[(int)decl.Semantic - (int)EPlugVDcl.TexCoord0] = values;
            }
            else if (decl.Type is EPlugVDclType.ColorD3D or EPlugVDclType.UByte4)
            {
                var values = stride == 4 ? reader.ReadArray<int>(VertexCount) : new int[VertexCount];
                if (stride != 4)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        stream.Position = offset + i * stride;
                        values[i] = reader.ReadInt32();
                    }
                }
                if (decl.Semantic == EPlugVDcl.BlendIndices) BlendIndices = values;
                else if (GetColorIndex(decl.Semantic) is { } index) colors[index] = values;
            }
        }
    }

    private void WriteGeometry(int version)
    {
        foreach (var decl in dataDecls ?? [])
        {
            decl.Legacy = version < 0;
            var owner = version <= 0 && decl.IsShared ? dataDecls![decl.SharedDeclIndex] : decl;
            var stride = version <= 0 ? decl.Stride : decl.GetValueSize(CompressLocalFloat3);
            var offset = version <= 0 && decl.IsShared ? decl.Offset : 0;
            owner.Data ??= new byte[checked(VertexCount * stride)];
            if (owner.Data.Length != checked(VertexCount * stride)) throw new InvalidDataException("Vertex data length does not match its declaration.");
            using var stream = new MemoryStream(owner.Data, writable: true);
            using var writer = new GbxWriter(stream);
            var vectors = GetVec3(decl.Semantic);
            uvs.TryGetValue((int)decl.Semantic - (int)EPlugVDcl.TexCoord0, out var texCoords);
            var pairs = decl.Semantic == EPlugVDcl.BlendWeight ? BlendWeight : texCoords;
            var integers = decl.Semantic == EPlugVDcl.BlendIndices ? BlendIndices
                : GetColorIndex(decl.Semantic) is { } index && colors.TryGetValue(index, out var colorValues) ? colorValues : null;
            for (var i = 0; i < VertexCount; i++)
            {
                stream.Position = offset + i * stride;
                if (decl.Type is EPlugVDclType.Float3 or EPlugVDclType.Dec3N && vectors is not null)
                {
                    if (vectors.Length != VertexCount) throw new InvalidDataException("Vector count does not match the vertex count.");
                    var packed = decl.Type == EPlugVDclType.Dec3N || (version >= 1 && CompressLocalFloat3 && decl.Space == EPlugVDclSpace.Local3D);
                    if (packed)
                    {
                        var previous = BitConverter.ToInt32(owner.Data, (int)stream.Position);
                        var original = new Vec3((previous << 22) >> 22, (previous << 12) >> 22, (previous << 2) >> 22) / 511f;
                        if (original != vectors[i])
                        {
                            writer.Write(((int)(vectors[i].X * 511) & 0x3FF) | (((int)(vectors[i].Y * 511) & 0x3FF) << 10) | (((int)(vectors[i].Z * 511) & 0x3FF) << 20));
                        }
                    }
                    else writer.Write(vectors[i]);
                }
                else if (decl.Type == EPlugVDclType.Float2 && pairs is not null)
                {
                    if (pairs.Length != VertexCount) throw new InvalidDataException("Vector count does not match the vertex count.");
                    writer.Write(pairs[i]);
                }
                else if (decl.Type is EPlugVDclType.ColorD3D or EPlugVDclType.UByte4 && integers is not null)
                {
                    if (integers.Length != VertexCount) throw new InvalidDataException("Color or blend-index count does not match the vertex count.");
                    writer.Write(integers[i]);
                }
            }
        }
    }

    private Vec3[]? GetVec3(EPlugVDcl semantic) => semantic switch
    {
        EPlugVDcl.Position => Positions,
        EPlugVDcl.Position1 => Positions2,
        EPlugVDcl.Normal => Normals,
        EPlugVDcl.Normal1 => Normals2,
        EPlugVDcl.TangentU => TangentUs,
        EPlugVDcl.TangentU1 => TangentUs2,
        EPlugVDcl.TangentV => TangentVs,
        EPlugVDcl.TangentV1 => TangentVs2,
        _ => null
    };

    private void SetVec3(EPlugVDcl semantic, Vec3[] values)
    {
        switch (semantic)
        {
            case EPlugVDcl.Position: Positions = values; break;
            case EPlugVDcl.Position1: Positions2 = values; break;
            case EPlugVDcl.Normal: Normals = values; break;
            case EPlugVDcl.Normal1: Normals2 = values; break;
            case EPlugVDcl.TangentU: TangentUs = values; break;
            case EPlugVDcl.TangentU1: TangentUs2 = values; break;
            case EPlugVDcl.TangentV: TangentVs = values; break;
            case EPlugVDcl.TangentV1: TangentVs2 = values; break;
        }
    }

    private static int? GetColorIndex(EPlugVDcl semantic) => semantic switch
    {
        EPlugVDcl.Color0 => 0,
        EPlugVDcl.Color1 => 1,
        EPlugVDcl.Color2 => 2,
        _ => null
    };

    public partial class DataDecl
    {
        private byte[]? data;
        /// <summary>Raw channel bytes. Older shared declarations use the referenced declaration's buffer.</summary>
        public byte[]? Data { get => data; set => data = value; }
        /// <summary>Vertex count supplied by the enclosing stream when reading or writing this declaration.</summary>
        public int VertexCount { get; set; }
        /// <summary>Whether flags use the unversioned format's nine-bit stride and buffer-ownership bit.</summary>
        public bool Legacy { get; set; }

        public EPlugVDcl Semantic { get => (EPlugVDcl)(Flags1 & 0x1FF); set => Flags1 = (Flags1 & ~0x1FFu) | ((uint)value & 0x1FF); }
        public EPlugVDcl WeightCount => Semantic;
        public EPlugVDclType Type { get => (EPlugVDclType)((Flags1 >> 9) & 0x1FF); set => Flags1 = (Flags1 & ~0x3FE00u) | (((uint)value & 0x1FF) << 9); }
        public EPlugVDclSpace Space => (EPlugVDclSpace)((Flags1 >> (Legacy ? 27 : 28)) & (Legacy ? 7u : 15u));
        public int Stride => (int)((Flags1 >> 18) & (Legacy ? 0x1FFu : 0x3FFu));
        public bool IsShared => Legacy ? (Flags1 & 0x40000000) == 0 : (Flags2 & 0xFFC) != 0;

        public override string ToString() => $"{Type} {Space} {Semantic}";

        internal int GetValueSize(bool compressed) => compressed && Type == EPlugVDclType.Float3 && Space == EPlugVDclSpace.Local3D ? 4 : Type switch
        {
            EPlugVDclType.Float1 or EPlugVDclType.ColorD3D or EPlugVDclType.UByte4 or EPlugVDclType.Short2
                or EPlugVDclType.UByte4N or EPlugVDclType.Short2N or EPlugVDclType.UShort2N
                or EPlugVDclType.UDec3 or EPlugVDclType.Dec3N or EPlugVDclType.Half2 => 4,
            EPlugVDclType.Float2 or EPlugVDclType.Short4 or EPlugVDclType.Short4N or EPlugVDclType.UShort4N or EPlugVDclType.Half4 => 8,
            EPlugVDclType.Float3 => 12,
            EPlugVDclType.Float4 => 16,
            _ => throw new NotSupportedException($"Unknown vertex declaration type {Type}.")
        };

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            Legacy = v < 0;
            rw.DataUInt32(ref flags1);
            if (!Legacy) rw.DataUInt32(ref flags2);
            if (IsShared)
            {
                rw.UInt16(ref sharedDeclIndex);
                rw.UInt16(ref offset);
                if (!Legacy && ((Flags2 >> 2) & 0x3FF) != Offset) throw new InvalidDataException("Shared declaration offset does not match its flags.");
            }
            else if (v <= 0)
            {
                if (Stride == 0 && VertexCount != 0) throw new InvalidDataException("Vertex stride cannot be zero.");
                rw.Data(ref data, checked(Stride * VertexCount));
            }
        }
    }
}
