namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0902C000</remarks>
[Node(0x0902C000), WritingNotSupported]
public abstract class CPlugVisual3D : CPlugVisual
{
    private Vertex[] vertices;
    private Vec3[]? tangents;
    private Vec3[]? biTangents;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0902C003))]
    [AppliedWithChunk(typeof(Chunk0902C004))]
    public Vertex[] Vertices { get => vertices; set => vertices = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0902C003))]
    public Vec3[]? Tangents { get => tangents; set => tangents = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0902C003))]
    public Vec3[]? BiTangents { get => biTangents; set => biTangents = value; }

    internal CPlugVisual3D()
    {
        vertices = Array.Empty<Vertex>();
    }

    /// <summary>
    /// CPlugVisual3D 0x002 chunk
    /// </summary>
    [Chunk(0x0902C002)]
    public class Chunk0902C002 : Chunk<CPlugVisual3D>
    {
        public CMwNod? U01;

        public override void ReadWrite(CPlugVisual3D n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    /// <summary>
    /// CPlugVisual3D 0x003 chunk
    /// </summary>
    [Chunk(0x0902C003)]
    public class Chunk0902C003 : Chunk<CPlugVisual3D>
    {
        public override void Read(CPlugVisual3D n, GameBoxReader r)
        {
            n.vertices = new Vertex[n.Count];

            for (int i = 0; i < n.Count; i++)
            {
                n.vertices[i] = new Vertex
                {
                    Position = r.ReadVec3(),
                    Normal = r.ReadVec3(),
                    U02 = r.ReadVec3(),
                    U03 = r.ReadSingle()
                };
            }

            n.tangents = r.ReadArray<Vec3>();
            n.biTangents = r.ReadArray<Vec3>();
        }

        public override void Write(CPlugVisual3D n, GameBoxWriter w)
        {
            for (int i = 0; i < n.Count; i++)
            {
                var v = n.vertices[i];
                w.Write(v.Position);
                w.Write(v.Normal.GetValueOrDefault());
                w.Write(v.U02.GetValueOrDefault());
                w.Write(v.U03.GetValueOrDefault());
            }

            w.WriteArray<Vec3>(n.tangents);
            w.WriteArray<Vec3>(n.biTangents);
        }
    }

    /// <summary>
    /// CPlugVisual3D 0x004 chunk
    /// </summary>
    [Chunk(0x0902C004)]
    public class Chunk0902C004 : Chunk<CPlugVisual3D>
    {
        void TODOSkipTangents(CPlugVisual3D n, GameBoxReader r)
        {
            // this is some weird bit trickery...
            int numBytesPerTangent = (~(byte)((uint)n.Flags >> 17) & 8) | 4;
            int numElems = r.ReadInt32();
            if (numElems != 0 && numElems != n.Count)
            {
                throw new InvalidDataException(string.Format("num tangents is not equal to num vertices ({0} != {1})", numElems, n.Count));
            }
            r.ReadBytes(numElems * numBytesPerTangent);
        }
        
        public override void Read(CPlugVisual3D n, GameBoxReader r)
        {
            var u02 = !n.IsFlagBitSet(22) || n.HasVertexNormals;
            var u03 = !n.IsFlagBitSet(22) || n.IsFlagBitSet(8);

            // Console.WriteLine("numBytesPerVertex={0}", numBytesPerVertex);
            n.vertices = r.ReadArray(n.Count, r =>
            {
                var pos = r.ReadVec3();
                var vertU01 = default(int?);
                var vertU02 = default(Vec3?);
                var vertU03 = default(int?);
                var vertU04 = default(Vec4?);

                if (u02)
                {
                    if (n.IsFlagBitSet(20))
                    {
                        vertU01 = r.ReadInt32();
                    }
                    else
                    {
                        vertU02 = r.ReadVec3();
                    }
                }

                if (u03)
                {
                    if (n.IsFlagBitSet(21))
                    {
                        vertU03 = r.ReadInt32();
                    }
                    else
                    {
                        vertU04 = r.ReadVec4();
                    }
                }

                return new Vertex
                {
                    Position = pos,
                    U04 = vertU01,
                    U05 = vertU02,
                    U06 = vertU03,
                    U07 = vertU04
                };
            });

            /*var list = new Vertex[n.Count];

            for(var i = 0; i < n.Count; i++)
            {
                list[i] = new Vertex(r.ReadVec3(), default, default, default);
                r.ReadInt32();
                r.ReadInt32();
            }*/

            //n.vertices = verts;

            TODOSkipTangents(n, r);
            TODOSkipTangents(n, r);
        }
    }

    public readonly record struct Vertex(Vec3 Position, Vec3? Normal, Vec3? U02, float? U03, int? U04, Vec3? U05, int? U06, Vec4? U07)
    {
        public override string ToString() => Position.ToString();
    }
}
