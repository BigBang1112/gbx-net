namespace GBX.NET.Engines.Plug;

/// <summary>
/// 3D visual (0x0902C000)
/// </summary>
[Node(0x0902C000), WritingNotSupported]
public abstract class CPlugVisual3D : CPlugVisual
{
    protected Vertex[] vertices;

    public Vertex[] Vertices
    {
        get => vertices;
        set => vertices = value;
    }

    protected CPlugVisual3D()
    {
        vertices = null!;
    }

    [Chunk(0x0902C002)]
    public class Chunk0902C002 : Chunk<CPlugVisual3D>
    {
        public CMwNod? U01;

        public override void ReadWrite(CPlugVisual3D n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    [Chunk(0x0902C003)]
    public class Chunk0902C003 : Chunk<CPlugVisual3D>
    {
        public Vec3[]? U01;
        public Vec3[]? U02;

        public override void Read(CPlugVisual3D n, GameBoxReader r)
        {
            n.vertices = new Vertex[n.Count];

            for (int i = 0; i < n.Count; i++)
            {
                n.vertices[i] = new Vertex(
                    position: r.ReadVec3(),
                    u01: r.ReadVec3(),
                    u02: r.ReadVec3(),
                    u03: r.ReadSingle()
                );
            }

            U01 = r.ReadArray<Vec3>();
            U02 = r.ReadArray<Vec3>();
        }
    }

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
                throw new InvalidDataException(String.Format("num tangents is not equal to num vertices ({0} != {1})", numElems, n.Count));
            }
            r.ReadBytes(numElems * numBytesPerTangent);
        }
        
        public override void Read(CPlugVisual3D n, GameBoxReader r)
        {
            var u02 = !n.Flags.HasFlag(VisualFlags.UnknownFlag22) || n.Flags.HasFlag(VisualFlags.HasVertexNormals);
            var u03 = !n.Flags.HasFlag(VisualFlags.UnknownFlag22) || n.Flags.HasFlag(VisualFlags.UnknownFlag8);

            int numBytesPerVertex = 12;
            if (u02)
            {
                numBytesPerVertex += n.Flags.HasFlag(VisualFlags.UnknownFlag20) ? 4 : 12;
            }
            if (u03)
            {
                numBytesPerVertex += n.Flags.HasFlag(VisualFlags.UnknownFlag21) ? 4 : 16;
            }   
            
            // Console.WriteLine("numBytesPerVertex={0}", numBytesPerVertex);
            var verts = r.ReadBytes(numBytesPerVertex * n.Count);

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

    public readonly struct Vertex
    {
        public Vec3 Position { get; }
        public Vec3 U01 { get; }
        public Vec3 U02 { get; }
        public float U03 { get; }

        public Vertex(Vec3 position, Vec3 u01, Vec3 u02, float u03)
        {
            Position = position;
            U01 = u01;
            U02 = u02;
            U03 = u03;
        }

        public override string ToString() => Position.ToString();
    }
}
