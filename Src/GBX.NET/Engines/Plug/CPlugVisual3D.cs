namespace GBX.NET.Engines.Plug;

public partial class CPlugVisual3D
{
    public Vertex[] Vertices { get; set; } = [];
    public Vec3[]? Tangents { get; set; }
    public Vec3[]? BiTangents { get; set; }

    public partial class Chunk0902C003
    {
        public override void Read(CPlugVisual3D n, GbxReader r)
        {
            n.Vertices = new Vertex[n.Count];

            using (var _ = r.ForceBinary())
            {
                for (int i = 0; i < n.Count; i++)
                {
                    n.Vertices[i] = new Vertex
                    {
                        Position = r.ReadVec3(),
                        Normal = r.ReadVec3(),
                        U02 = r.ReadVec3(),
                        U03 = r.ReadSingle()
                    };
                }
            }

            n.Tangents = r.ReadArray<Vec3>();
            n.BiTangents = r.ReadArray<Vec3>();
        }

        public override void Write(CPlugVisual3D n, GbxWriter w)
        {
            foreach (var vertex in n.Vertices)
            {
                w.Write(vertex.Position);
                w.Write(vertex.Normal.GetValueOrDefault());
                w.Write(vertex.U02.GetValueOrDefault());
                w.Write(vertex.U03.GetValueOrDefault());
            }

            w.WriteArray(n.Tangents);
            w.WriteArray(n.BiTangents);
        }
    }

    public partial class Chunk0902C004
    {
        public int Tangents1Count { get; set; }
        public byte[]? Tangents1 { get; set; }
        public int Tangents2Count { get; set; }
        public byte[]? Tangents2 { get; set; }

        public override void Read(CPlugVisual3D n, GbxReader r)
        {
            var u01 = !n.IsFlagBitSet(22) || n.HasVertexNormals;
            var u02 = !n.IsFlagBitSet(22) || n.IsFlagBitSet(8);
            var u03 = n.IsFlagBitSet(20);
            var u04 = n.IsFlagBitSet(21);
            var isSprite = n is CPlugVisualSprite;

            if (n.VertexStreams.Count == 0)
            {
                n.Vertices = new Vertex[n.Count];

                for (var i = 0; i < n.Count; i++)
                {
                    n.Vertices[i] = Vertex.Read(r, u01, u02, u03, u04, isSprite);
                }
            }

            (Tangents1Count, Tangents1) = ReadTangents(n, r);
            (Tangents2Count, Tangents2) = ReadTangents(n, r);
        }

        public override void Write(CPlugVisual3D n, GbxWriter w)
        {
            var u01 = !n.IsFlagBitSet(22) || n.HasVertexNormals;
            var u02 = !n.IsFlagBitSet(22) || n.IsFlagBitSet(8);
            var u03 = n.IsFlagBitSet(20);
            var u04 = n.IsFlagBitSet(21);
            var isSprite = n is CPlugVisualSprite;

            if (n.VertexStreams.Count == 0)
            {
                for (var i = 0; i < n.Count; i++)
                {
                    n.Vertices[i].Write(w, u01, u02, u03, u04, isSprite);
                }
            }

            WriteTangents(w, Tangents1Count, Tangents1);
            WriteTangents(w, Tangents2Count, Tangents2);
        }

        private static (int, byte[]) ReadTangents(CPlugVisual3D n, GbxReader r)
        {
            // this is some weird bit trickery...
            var numBytesPerTangent = (~(byte)((uint)n.Flags >> 17) & 8) | 4;
            var numElems = r.ReadInt32();

            if (numElems != 0 && numElems != n.Count)
            {
                throw new InvalidDataException($"num tangents is not equal to num vertices ({numElems} != {n.Count})");
            }

            return (numElems, r.ReadBytes(numElems * numBytesPerTangent));
        }

        private static void WriteTangents(GbxWriter w, int tangentsCount, byte[]? tangents)
        {
            w.Write(tangentsCount);
            w.Write(tangents);
        }
    }

    public readonly record struct Vertex(Vec3 Position, Vec3? Normal, Vec3? U02, float? U03, Vec4? Color, float? U08, int? U09)
    {
        public static Vertex Read(GbxReader r, bool u01, bool u02, bool u03, bool u04, bool isSprite)
        {
            var pos = r.ReadVec3();
            var normal = default(Vec3?);
            var color = default(Vec4?);

            if (u01)
            {
                normal = u03 ? r.ReadVec3_10b() : r.ReadVec3();
            }

            if (u02)
            {
                if (u04)
                {
                    var colorInt = r.ReadInt32();
                    color = new Vec4(
                        (colorInt >> 0x10 & 0xFF) / 255f,
                        (colorInt >> 8 & 0xFF) / 255f,
                        (colorInt & 0xFF) / 255f,
                        (colorInt >> 0x18 & 0xFF) / 255f
                    );
                }
                else
                {
                    color = r.ReadVec4();
                }
            }

            var vertU05 = default(float?);
            var vertU06 = default(int?);

            if (isSprite)
            {
                vertU05 = r.ReadSingle();
                vertU06 = r.ReadInt32();
            }

            return new Vertex
            {
                Position = pos,
                Normal = normal,
                Color = color,
                U08 = vertU05,
                U09 = vertU06,
            };
        }

        public void Write(GbxWriter w, bool u01, bool u02, bool u03, bool u04, bool isSprite)
        {
            w.Write(Position);

            if (u01)
            {
                if (u03)
                {
                    w.WriteVec3_10b(Normal.GetValueOrDefault());
                }
                else
                {
                    w.Write(Normal.GetValueOrDefault());
                }
            }

            if (u02)
            {
                if (u04)
                {
                    w.Write((int)(Color.GetValueOrDefault().X * 255) << 0x10
                        | (int)(Color.GetValueOrDefault().Y * 255) << 8
                        | (int)(Color.GetValueOrDefault().Z * 255)
                        | (int)(Color.GetValueOrDefault().W * 255) << 0x18);
                }
                else
                {
                    w.Write(Color.GetValueOrDefault());
                }
            }

            if (isSprite)
            {
                w.Write(U08.GetValueOrDefault());
                w.Write(U09.GetValueOrDefault());
            }
        }

        public override string ToString() => Position.ToString();
    }
}
