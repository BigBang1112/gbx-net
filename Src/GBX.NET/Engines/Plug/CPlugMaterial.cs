namespace GBX.NET.Engines.Plug;

[Node(0x09079000), WritingNotSupported]
public sealed class CPlugMaterial : CPlug
{
    private CPlugMaterial()
    {

    }

    [Chunk(0x09079001)]
    public class Chunk09079001 : Chunk<CPlugMaterial>
    {
        public CMwNod? U01;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    [Chunk(0x09079007)]
    public class Chunk09079007 : Chunk<CPlugMaterial>
    {
        public override void Read(CPlugMaterial n, GameBoxReader r)
        {
            var customMaterial = r.ReadNodeRef<CPlugMaterialCustom>();
        }
    }

    [Chunk(0x09079009)]
    public class Chunk09079009 : Chunk<CPlugMaterial>
    {
        public CMwNod? U01;
        public object? U02;
        public CMwNod? U03;
        public CMwNod? U04;

        public override void Read(CPlugMaterial n, GameBoxReader r)
        {
            U01 = r.ReadNodeRef();

            U02 = r.ReadArray(r =>
            {
                return new
                {
                    u01 = r.ReadInt16(),
                    u02 = r.ReadInt16(),
                    u03 = r.ReadBoolean(),
                    u04 = r.ReadNodeRef()
                };
            });

            U03 = r.ReadNodeRef();
            U04 = r.ReadNodeRef();
        }
    }

    [Chunk(0x0907900A)]
    public class Chunk0907900A : Chunk<CPlugMaterial>
    {
        public int U01;

        public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }
}
