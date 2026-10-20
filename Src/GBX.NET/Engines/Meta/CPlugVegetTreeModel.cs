namespace GBX.NET.Engines.Meta;

public partial class CPlugVegetTreeModel
{
    public override void ReadWrite(GbxReaderWriter rw) => ReadWrite(rw, v: 0);

    public partial class CollisionSurface
    {
        private int surfaceVersion = 1;

        public int SurfaceVersion { get => surfaceVersion; set => surfaceVersion = value; }
        public CPlugSurface.ISurf? Surf { get; set; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            if (v >= 13)
            {
                rw.Int32(ref surfaceVersion);
            }

            if (rw.Reader is not null)
            {
                Surf = CPlugSurface.ReadSurf(rw.Reader, v >= 13 ? surfaceVersion : 1);
            }
            if (rw.Writer is not null)
            {
                CPlugSurface.WriteSurf(Surf, rw.Writer, v >= 13 ? surfaceVersion : 1);
            }
        }
    }
}
