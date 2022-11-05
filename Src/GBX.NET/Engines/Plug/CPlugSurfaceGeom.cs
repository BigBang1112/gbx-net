namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x0900F000</remarks>
[Node(0x0900F000)]
public class CPlugSurfaceGeom : CPlugSurface
{
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
            n.Surf = rw.Archive(n.Surf as Mesh);
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