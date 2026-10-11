namespace GBX.NET.Engines.Plug;

public partial class CPlugDynaWaterModel
{
    public CPlugDynaWaterModel()
    {
        U04 = new PlugCurve { Values = [new(0, 1), new(1, 500)] };
        U05 = new PlugCurve { Values = [new(0, 0.85f)] };
        U06 = new PlugCurve { Values = [new(0, 0), new(1, 6000)] };
        U08 = new PlugCurve { Values = [new(0, 0), new(1, 1200)] };
    }

    public void ReadWrite(GbxReaderWriter rw, int v)
    {
        rw.Single(ref gravity);
        rw.Single(ref u02);
        rw.Single(ref u03);
        ReadWriteCurve(rw, ref u04, v);
        ReadWriteCurve(rw, ref u05, v);
        ReadWriteCurve(rw, ref u06, v);
        rw.Single(ref u07);
        ReadWriteCurve(rw, ref u08, v);
        rw.Single(ref u09);

        if (v == 2)
        {
            if (rw.Reader is not null)
            {
                var surf = CPlugSurface.ReadSurf(rw.Reader, surfVersion: 2);
                surface = surf is null ? null : new CPlugSurface { Surf = surf, SurfVersion = 2 };
                surfaceFile = null;
                if (surface is not null)
                {
                    surface.CreateChunk<CPlugSurface.Chunk0900C003>().Version = 2;
                }
            }
            if (rw.Writer is not null)
            {
                CPlugSurface.WriteSurf(Surface?.Surf, rw.Writer, surfVersion: 2);
            }
        }
        else if (v >= 3)
        {
            rw.NodeRef<CPlugSurface>(ref surface, ref surfaceFile);
        }
    }

    private static void ReadWriteCurve(GbxReaderWriter rw, ref PlugCurve? curve, int version)
    {
        if (rw.Reader is not null)
        {
            curve ??= new PlugCurve();
        }
        (curve ?? new PlugCurve()).ReadWrite(rw, version - 1, useSimpleNode: true);
    }

    public partial class Chunk0915F000
    {
        public override void ReadWrite(CPlugDynaWaterModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            n.ReadWrite(rw, Version);
        }
    }
}
