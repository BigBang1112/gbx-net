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
        public int U01;

        public override void ReadWrite(CPlugVisual3D n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
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

            U01 = r.ReadArray(r => r.ReadVec3());
            U02 = r.ReadArray(r => r.ReadVec3());
        }
    }

    [Chunk(0x0902C004)]
    public class Chunk0902C004 : Chunk<CPlugVisual3D>
    {
        public override void Read(CPlugVisual3D n, GameBoxReader r)
        {
            var u01 = n.Flags >> 0x16 & 1;
            var u02 = u01 == 0 || (char)n.Flags < '\0';
            var u03 = u01 == 0 || (n.Flags & 0x100) != 0;

            var u04 = (n.Flags & 0x100000) != 0;

            if (!u04)
            {
                if (u02 && (n.Flags & 0x200000U) == 0 && u03)
                {
                    // DoData
                }
            }

            var ara = new Vec3[n.Count];

            for (var i = 0; i < n.Count; i++)
            {
                ara[i] = r.ReadVec3();
                if (u02)
                {
                    if (u04)
                    {
                        var ok = r.ReadInt32();
                    }
                    else
                    {
                        var ok2 = r.ReadVec3();
                    }
                }

                if (u03)
                {
                    if ((n.Flags & 0x200000U) == 0)
                    {
                        var wtff = r.ReadBytes(0x10);
                    }
                    else
                    {
                        var color = r.ReadInt32();
                    }
                }
            }

            var wtf = r.ReadArray<int>(800);
            var bruh = r.ReadArray(n.Count, r => (r.ReadVec4(), r.ReadInt32()));

            var strst = r.ReadSpan<Vec4>(50);
        }

        /*public override void ReadWrite(CPlugVisual3D n, GameBoxReaderWriter rw)
        {
            

            var nice = rw.Array<int>(count: 30);

            if ((char)n.Flags < '\0') // wtf
            {

            }

            if ((n.Flags & 0x200000U) == 0)
            {

            }

            var wat = ~(n.Flags >> 0x11) & 8 | 4;

            //var verts = rw.Reader.ReadArray(n.count-4, r => r.ReadVec4());
        }*/
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
