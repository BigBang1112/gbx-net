namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockTriangles : CGameCtnMediaBlock.IHasKeys
{
    private Vec4[] vertices = [];
    private Int3[] triangles = [];

    [AppliedWithChunk<Chunk03029001>]
    public IList<Key> Keys { get; set; } = new List<Key>();

    IEnumerable<IKey> IHasKeys.Keys => (IEnumerable<IKey>)Keys;

    [AppliedWithChunk<Chunk03029001>]
    public Vec4[] Vertices
    {
        get => vertices;
        set
        {
            if (vertices is null || value.Length != vertices.Length)
            {
                vertices ??= value;

                foreach (var key in Keys)
                {
                    var positions = key.Positions;
                    Array.Resize(ref positions, value.Length);
                    key.Positions = positions;
                }

                RemoveTrianglesOutOfRange();
            }

            vertices = value;
        }
    }

    [AppliedWithChunk<Chunk03029001>]
    public Int3[] Triangles
    {
        get => triangles;
        set
        {
            if (vertices is null)
            {
                return;
            }

            foreach (var int3 in value)
            {
                if (int3.X >= vertices.Length
                 || int3.Y >= vertices.Length
                 || int3.Z >= vertices.Length)
                    throw new Exception($"Index in {int3} is not available in vertices.");
            }

            triangles = value;
        }
    }

    private void RemoveTrianglesOutOfRange()
    {
        if (triangles == null) return;

        var trianglesToRemove = new List<Int3>();

        foreach (var triangle in triangles)
        {
            if (triangle.X >= vertices.Length
            || triangle.Y >= vertices.Length
            || triangle.Z >= vertices.Length)
            {
                trianglesToRemove.Add(triangle);
            }
        }

        triangles = triangles.Where(x => !trianglesToRemove.Contains(x)).ToArray();
    }

    public partial class Chunk03029001
    {
        public int U01;
        public int U02;
        public int U03;
        public float U04;
        public int U05;
        public long U06;

        public override void Read(CGameCtnMediaBlockTriangles n, GbxReader r)
        {
            var numKeys = r.ReadInt32();
            n.Keys = new List<Key>();
            for (var i = 0; i < numKeys; i++)
            {
                n.Keys.Add(new Key(n)
                {
                    Time = r.ReadTimeSingle()
                });
            }

            numKeys = r.ReadInt32();
            var numVerts = r.ReadInt32();

            for (var i = 0; i < numKeys; i++)
            {
                n.Keys[i].Positions = new Vec3[numVerts];

                for (var j = 0; j < numVerts; j++)
                {
                    n.Keys[i].Positions[j] = r.ReadVec3();
                }
            }

            n.vertices = r.ReadArray<Vec4>();
            n.triangles = r.ReadArray<Int3>();

            U01 = r.ReadInt32();
            U02 = r.ReadInt32();
            U03 = r.ReadInt32();
            U04 = r.ReadSingle();
            U05 = r.ReadInt32();
            U06 = r.ReadInt64();
        }

        public override void Write(CGameCtnMediaBlockTriangles n, GbxWriter w)
        {
            w.Write(n.Keys.Count);
            foreach (var key in n.Keys)
            {
                w.Write(key.Time);
            }

            w.Write(n.Keys.Count);
            w.Write(n.vertices.Length);

            foreach (var key in n.Keys)
            {
                foreach (var pos in key.Positions)
                {
                    w.Write(pos);
                }
            }

            w.WriteArray(n.vertices);
            w.WriteArray(n.triangles);

            w.Write(U01);
            w.Write(U02);
            w.Write(U03);
            w.Write(U04);
            w.Write(U05);
            w.Write(U06);
        }
    }

    public partial class Key
    {
        private readonly CGameCtnMediaBlockTriangles node;

        private Vec3[] positions;

        public Vec3[] Positions
        {
            get => positions;
            set
            {
                if (value.Length != positions.Length)
                {
                    Array.Resize(ref node.vertices, value.Length);

                    foreach (var k in node.Keys)
                        if (k != this)
                            Array.Resize(ref k.positions, value.Length);

                    node.RemoveTrianglesOutOfRange();
                }

                positions = value;
            }
        }

        public Key(CGameCtnMediaBlockTriangles node)
        {
            this.node = node;
            positions = new Vec3[node.vertices.Length];
        }
    }
}
