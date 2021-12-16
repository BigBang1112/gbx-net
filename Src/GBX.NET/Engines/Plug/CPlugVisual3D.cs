using System.Globalization;

namespace GBX.NET.Engines.Plug;

[Node(0x0902C000), WritingNotSupported]
public class CPlugVisual3D : CPlugVisual
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
        public int U01;

        public override void ReadWrite(CPlugVisual3D n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x0902C003)]
    public class Chunk0902C003 : Chunk<CPlugVisual3D>
    {
        public int U01;
        public int U02;

        public override void Read(CPlugVisual3D n, GameBoxReader r)
        {
            n.vertices = new Vertex[n.count];

            for (int i = 0; i < n.count; i++)
            {
                n.vertices[i] = new Vertex(
                    position: r.ReadVec3(),
                    u01: r.ReadVec3(),
                    u02: r.ReadVec3(),
                    u03: r.ReadSingle()
                );
            }

            U01 = r.ReadInt32();
            U02 = r.ReadInt32();
        }
    }

    [Chunk(0x0902C004)]
    public class Chunk0902C004 : Chunk<CPlugVisual3D>
    {
        public override void ReadWrite(CPlugVisual3D n, GameBoxReaderWriter rw)
        {
            if ((char)n.flags < '\0') // wtf
            {

            }

            if ((n.flags & 0x200000U) == 0)
            {

            }

            var wat = ~(n.flags >> 0x11) & 8 | 4;

            //var verts = rw.Reader.ReadArray(n.count-4, r => r.ReadVec4());
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
