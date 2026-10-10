namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockTriangles : CGameCtnMediaBlock.IHasKeys
{
    private Vec4[] vertices = [];
    private Int3[] triangles = [];

    [AppliedWithChunk<Chunk03029000>]
    [AppliedWithChunk<Chunk03029001>]
    public List<Key> Keys { get; set; } = [];

    IEnumerable<IKey> IHasKeys.Keys => Keys;

    [AppliedWithChunk<Chunk03029000>]
    [AppliedWithChunk<Chunk03029001>]
    public Vec4[] Vertices
    {
        get => vertices;
        set
        {
            var sizeChanged = vertices is null || value.Length != vertices.Length;
            vertices = value;

            if (sizeChanged)
            {
                foreach (var key in Keys)
                {
                    var positions = key.Positions;
                    Array.Resize(ref positions, value.Length);
                    key.Positions = positions;
                }

                RemoveTrianglesOutOfRange();
            }
        }
    }

    [AppliedWithChunk<Chunk03029000>]
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

    public partial class Chunk03029000
    {
        public override void ReadWrite(CGameCtnMediaBlockTriangles n, GbxReaderWriter rw)
        {
            var numKeys = rw.Int32(n.Keys.Count);
            if (rw.Reader is not null)
            {
                n.Keys = new List<Key>(numKeys);
                for (var i = 0; i < numKeys; i++)
                {
                    n.Keys.Add(new Key(n));
                }
            }

            foreach (var key in n.Keys)
            {
                key.Time = rw.TimeSingle(key.Time);
            }

            var numPositionKeys = rw.Int32(n.Keys.Count);
            var numVerts = rw.Int32(n.vertices.Length);
            if (numPositionKeys != n.Keys.Count)
            {
                throw new InvalidDataException("The position matrix must have one row per key.");
            }

            foreach (var key in n.Keys)
            {
                if (rw.Writer is not null && key.Positions.Length != numVerts)
                {
                    throw new InvalidDataException("The position matrix must have one position per vertex.");
                }

                key.Positions = rw.Array(key.Positions, numVerts)!;
            }

            rw.Array(ref n.vertices!);
            rw.Array(ref n.triangles!);
        }
    }

    public partial class Key : IDeepCloneable
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

        object IDeepCloneable.DeepClone(DeepCloneContext context)
        {
            var clone = new Key(context.Clone(node)!) { Time = Time };
            context.Register(this, clone);
            clone.positions = context.CloneArray(positions)!;
            return clone;
        }
    }
}
