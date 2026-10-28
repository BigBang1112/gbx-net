namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleVisGeomModel
{
    internal int SurfVersion { get; set; }

    public partial class DamagePart
    {
        private CPlugDynaModel dyna = new();
        private CPlugSurface.ISurf? surf;

        public CPlugSurface.ISurf? Surf
        {
            get => surf;
            set => surf = value;
        }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.NodeRef(ref meshRef);
            rw.ReadableWritable<CPlugDynaModel>(ref dyna);

            if (rw.Reader is not null)
            {
                surf = CPlugSurface.ReadSurf(rw.Reader, v);
            }
            if (rw.Writer is not null)
            {
                CPlugSurface.WriteSurf(surf, rw.Writer, v);
            }
        }
    }
}
