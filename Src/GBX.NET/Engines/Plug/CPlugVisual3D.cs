namespace GBX.NET.Engines.Plug;

public partial class CPlugVisual3D
{
    public Vertex[] Vertices { get; set; } = [];
    public Vec3[]? Tangents { get; set; } = [];
    public Vec3[]? BiTangents { get; set; } = [];

    private static void ReadWriteVertices(CPlugVisual3D n, GbxReaderWriter rw, bool legacy)
    {
        if (n.Count < 0) throw new InvalidDataException("Vertex count cannot be negative.");
        if (rw.Reader is not null) n.Vertices = new Vertex[n.Count];
        if (n.Vertices.Length != n.Count) throw new InvalidDataException("Vertex count does not match the geometry header.");

        var hasNormals = legacy || !n.IsFlagBitSet(22) || n.HasVertexNormals;
        var hasColors = legacy || !n.IsFlagBitSet(22) || n.HasVertexColors;
        // Sprites use the normal slot for size and rotation data.
        var compressNormals = !legacy && n.IsFlagBitSet(20) && n is not CPlugVisualSprite;
        var compressColors = !legacy && n.IsFlagBitSet(21);
        using var binaryReader = rw.Reader?.ForceBinary();
        for (var i = 0; i < n.Count; i++)
        {
            n.Vertices[i] = n.Vertices[i].ReadWrite(rw, hasNormals, hasColors, compressNormals, compressColors);
        }
    }

    public partial class Chunk0902C001
    {
        public override GameVersion GameVersion => GameVersion.Unspecified;

        public override void ReadWrite(CPlugVisual3D n, GbxReaderWriter rw)
        {
            ReadWriteVertices(n, rw, legacy: true);
            n.Tangents = rw.Array(n.Tangents);
        }
    }

    public partial class Chunk0902C002
    {
        private CMwNod? LegacyBlendShapes { get; set; }
        private bool LegacyBlendShapesSet { get; set; }

        [Obsolete("Use CPlugVisual3D.BlendShapes instead.")]
        public CMwNod? U01
        {
            get => LegacyBlendShapes;
            set { LegacyBlendShapes = value; LegacyBlendShapesSet = true; }
        }

        public override void ReadWrite(CPlugVisual3D n, GbxReaderWriter rw)
        {
            if (rw.Reader is null && LegacyBlendShapesSet) n.BlendShapes = LegacyBlendShapes;
            n.BlendShapes = rw.NodeRef(n.BlendShapes);
            LegacyBlendShapes = n.BlendShapes;
            LegacyBlendShapesSet = false;
        }
    }

    public partial class Chunk0902C004
    {
        public int TangentCount { get; set; }
        public byte[]? TangentData { get; set; }
        public int BitangentCount { get; set; }
        public byte[]? BitangentData { get; set; }

        public override void ReadWrite(CPlugVisual3D n, GbxReaderWriter rw)
        {
            if (n.VertexStreams.Count == 0) ReadWriteVertices(n, rw, legacy: false);
            else if (rw.Reader is not null) n.Vertices = [];

            (TangentCount, TangentData) = ReadWriteTangents(n, rw, TangentCount, TangentData);
            (BitangentCount, BitangentData) = ReadWriteTangents(n, rw, BitangentCount, BitangentData);
        }

        private static (int, byte[]?) ReadWriteTangents(CPlugVisual3D n, GbxReaderWriter rw, int count, byte[]? data)
        {
            count = rw.Int32(count);
            if (count < 0 || (count != 0 && count != n.Count))
            {
                throw new InvalidDataException($"Tangent count must be zero or the vertex count ({count} != {n.Count}).");
            }

            var length = checked(count * (n.IsFlagBitSet(20) ? 4 : 12));
            if (rw.Writer is not null && (data?.Length ?? 0) != length)
            {
                throw new InvalidDataException("Tangent data length does not match its count and compression flags.");
            }
            data = rw.Data(data, length);
            if ((data?.Length ?? 0) != length) throw new EndOfStreamException("Tangent data is truncated.");
            return (count, data);
        }

        [Obsolete("Use TangentCount instead.")]
        public int Tangents1Count { get => TangentCount; set => TangentCount = value; }

        [Obsolete("Use TangentData instead.")]
        public byte[]? Tangents1 { get => TangentData; set => TangentData = value; }

        [Obsolete("Use BitangentCount instead.")]
        public int Tangents2Count { get => BitangentCount; set => BitangentCount = value; }

        [Obsolete("Use BitangentData instead.")]
        public byte[]? Tangents2 { get => BitangentData; set => BitangentData = value; }
    }
}
