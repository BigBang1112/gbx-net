
namespace GBX.NET.Engines.Plug;

public partial class CPlugSurfaceGeom
{
    public CPlugSurface.ISurf? Surf { get; set; }

    public partial class Chunk0900D002
    {
        public override void Read(CPlugSurfaceGeom n, GbxReader r)
        {
            n.Surf = new CPlugSurface.Mesh();
            n.Surf.Read(r);
        }

        public override void Write(CPlugSurfaceGeom n, GbxWriter w)
        {
            if (n.Surf is null)
            {
                throw new InvalidOperationException("Cannot write default (null) surf.");
            }

            n.Surf.Write(w);
        }
    }

    public partial class Chunk0900F002
    {
        public ushort U01;

        public override void ReadWrite(CPlugSurfaceGeom n, GbxReaderWriter rw)
        {
            if (rw.Reader is not null)
            {
                n.Surf = CPlugSurface.ReadSurf(rw.Reader, version: 0);
            }

            if (rw.Writer is not null)
            {
                CPlugSurface.WriteSurf(n.Surf, rw.Writer, version: 0);
            }

            rw.UInt16(ref U01);
        }
    }

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
                rw.Reader.Settings.EncryptionInitializer?.Initialize(BitConverter.GetBytes(U02.X - U02.X2), 0, 4);

                n.Surf = CPlugSurface.ReadSurf(rw.Reader, version: 0);
            }
            
            if (rw.Writer is not null)
            {
                CPlugSurface.WriteSurf(n.Surf, rw.Writer, version: 0);
            }

            // I have a suspicion Nadeo broke backwards compatibility here
            if (n.Surf is CPlugSurface.Mesh)
            {
                rw.UInt16(ref U03);
            }
        }
    }
}
