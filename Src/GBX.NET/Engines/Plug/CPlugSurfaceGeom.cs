namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0900F000</remarks>
[Node(0x0900F000)]
public class CPlugSurfaceGeom : CPlugSurface
{
    private int type;
    private Vec3[] vertices = Array.Empty<Vec3>();
    private (Vec4, Int3, ushort, byte, byte)[]? triangles;
    private int meshOctreeCellVersion;
    private (int, Vec3, Vec3, int)[]? meshOctreeCells;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0900F002))]
    public int Type { get => type; set => type = value; }
    
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0900F002))]
    public Vec3[] Vertices { get => vertices; set => vertices = value; }
    
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0900F002))]
    public (Vec4, Int3, ushort, byte, byte)[]? Triangles { get => triangles; set => triangles = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0900F002))]
    public int MeshOctreeCellVersion { get => meshOctreeCellVersion; set => meshOctreeCellVersion = value; }
    
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0900F002))]
    public (int, Vec3, Vec3, int)[]? MeshOctreeCells { get => meshOctreeCells; set => meshOctreeCells = value; }

    internal CPlugSurfaceGeom()
    {

    }

    #region 0x002 chunk

    /// <summary>
    /// CPlugSurfaceGeom 0x002 chunk
    /// </summary>
    [Chunk(0x0900F002)]
    public class Chunk0900F002 : Chunk<CPlugSurfaceGeom>
    {
        public override void ReadWrite(CPlugSurfaceGeom n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.type); // Only 3 is known
            rw.Array<Vec3>(ref n.vertices!);

            rw.Array(ref n.triangles, r => // STriangle array?
            {
                return (r.ReadVec4(),
                    r.ReadInt3(),
                    r.ReadUInt16(),
                    r.ReadByte(),
                    r.ReadByte());
            }, (x, w) =>
            {
                w.Write(x.Item1);
                w.Write(x.Item2);
                w.Write(x.Item3);
                w.Write(x.Item4);
                w.Write(x.Item5);
            });

            rw.Int32(ref n.meshOctreeCellVersion);
            rw.Array(ref n.meshOctreeCells, r =>
            {
                return (r.ReadInt32(),
                    r.ReadVec3(),
                    r.ReadVec3(),
                    r.ReadInt32());
            }, (x, w) =>
            {
                w.Write(x.Item1);
                w.Write(x.Item2);
                w.Write(x.Item3);
                w.Write(x.Item4);
            });
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CPlugSurfaceGeom 0x004 chunk
    /// </summary>
    [Chunk(0x0900F004)]
    public class Chunk0900F004 : Chunk<CPlugSurfaceGeom>
    {
        public string U01 = "";
        public NET.Box U02;
        public int U03;
        public ushort U04;

        public override void ReadWrite(CPlugSurfaceGeom n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01!);
            rw.Box(ref U02);

            if (rw.Reader is not null && rw.Reader.BaseStream is IXorTrickStream cryptedStream)
            {
                cryptedStream.InitializeXorTrick(BitConverter.GetBytes(U02.X - U02.X2), 0, 4);
            }

            var surf = n.Surf;
            ArchiveSurf(ref surf, rw);
            n.Surf = surf;

            rw.UInt16(ref U04);
        }
    }

    #endregion
}