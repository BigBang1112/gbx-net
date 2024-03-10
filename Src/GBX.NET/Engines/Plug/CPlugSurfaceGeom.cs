
namespace GBX.NET.Engines.Plug;

public partial class CPlugSurfaceGeom
{
    public CPlugSurface.ISurf Surf { get; set; }

    public partial class Chunk0900F004
    {
        public string? U01;
        public BoxAligned U02;
        public ushort U03;

        public override void ReadWrite(CPlugSurfaceGeom n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
            rw.BoxAligned(ref U02);

            if (rw.Reader is not null)
            {
                n.Surf = CPlugSurface.ReadSurf(rw.Reader, version: 0);
            }
            
            if (rw.Writer is not null)
            {
                CPlugSurface.WriteSurf(n.Surf, rw.Writer);
            }

            rw.UInt16(ref U03);
        }
    }
}
