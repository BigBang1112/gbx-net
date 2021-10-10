namespace GBX.NET.Engines.Plug
{
    [Node(0x09079000)]
    public sealed class CPlugMaterial : CPlug
    {
        private CPlugMaterial()
        {

        }

        [Chunk(0x09079001)]
        public class Chunk09079001 : Chunk<CPlugMaterial>
        {
            public int U01;

            public override void ReadWrite(CPlugMaterial n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref U01);
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
    }
}
