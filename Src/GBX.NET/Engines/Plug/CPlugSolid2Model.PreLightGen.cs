namespace GBX.NET.Engines.Plug;

public partial class CPlugSolid2Model
{
    public class PreLightGen : IVersionable, IReadableWritable
    {
        private int v;
        private int u01;
        private float u02;
        private bool u03;
        private float u04;
        private float u05;
        private float u06;
        private float u07;
        private float u08;
        private float u09;
        private float u10;
        private float u11;
        private Int2 spriteCount;
        private Box[] u14 = Array.Empty<Box>();
        private (int, int, int, int)[]? uvGroups;

        public int Version { get => v; set => v = value; }
        public int U01 { get => u01; set => u01 = value; }
        public float U02 { get => u02; set => u02 = value; }
        public bool U03 { get => u03; set => u03 = value; }
        public float U04 { get => u04; set => u04 = value; }
        public float U05 { get => u05; set => u05 = value; }
        public float U06 { get => u06; set => u06 = value; }
        public float U07 { get => u07; set => u07 = value; }
        public float U08 { get => u08; set => u08 = value; }
        public float U09 { get => u09; set => u09 = value; }
        public float U10 { get => u10; set => u10 = value; }
        public float U11 { get => u11; set => u11 = value; }
        public Int2 SpriteCount { get => spriteCount; set => spriteCount = value; }
        public Box[] U14 { get => u14; set => u14 = value; }
        public (int, int, int, int)[]? UvGroups { get => uvGroups; set => uvGroups = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref v);
            rw.Int32(ref u01);
            rw.Single(ref u02); // uv/m something?
            rw.Boolean(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
            rw.Single(ref u09);
            rw.Single(ref u10);
            rw.Single(ref u11);
            rw.Int2(ref spriteCount);
            rw.Array<Box>(ref u14!);

            if (v >= 1)
            {
                rw.Array(ref uvGroups);
            }
        }
    }
}
