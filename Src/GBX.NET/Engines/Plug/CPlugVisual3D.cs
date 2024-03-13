namespace GBX.NET.Engines.Plug;

public partial class CPlugVisual3D
{
    public Vertex[] Vertices { get; set; }
    public Vec3[]? Tangents { get; set; }
    public Vec3[]? BiTangents { get; set; }

    public partial class Chunk0902C003
    {
        public override void Read(CPlugVisual3D n, GbxReader r)
        {
            n.Vertices = new Vertex[n.Count];

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

            n.Tangents = r.ReadArray<Vec3>();
            n.BiTangents = r.ReadArray<Vec3>();
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

    public readonly record struct Vertex(Vec3 Position, Vec3? Normal, Vec3? U02, float? U03, int? U04, Vec3? U05, int? U06, Vec4? U07, float? U08, int? U09)
    {
        public static Vertex Read(GbxReader r, bool u01, bool u02, bool u03, bool u04, bool isSprite)
        {
            var pos = r.ReadVec3();
            var vertU01 = default(int?);
            var vertU02 = default(Vec3?);
            var vertU03 = default(int?);
            var vertU04 = default(Vec4?);

            if (u01)
            {
                if (u03)
                {
                    vertU01 = r.ReadInt32();
                }
                else
                {
                    vertU02 = r.ReadVec3();
                }
            }

            if (u02)
            {
                if (u04)
                {
                    vertU03 = r.ReadInt32();
                }
                else
                {
                    vertU04 = r.ReadVec4();
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
                U04 = vertU01,
                U05 = vertU02,
                U06 = vertU03,
                U07 = vertU04,
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
                    w.Write(U04.GetValueOrDefault());
                }
                else
                {
                    w.Write(U05.GetValueOrDefault());
                }
            }

            if (u02)
            {
                if (u04)
                {
                    w.Write(U06.GetValueOrDefault());
                }
                else
                {
                    w.Write(U07.GetValueOrDefault());
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
