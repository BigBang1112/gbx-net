namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockEntity
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private int u01;
        private float u02;
        private int u03;
        private int u04;
        private float u05;
        private float? u06;

        public int U01 { get => u01; set => u01 = value; }
        public float U02 { get => u02; set => u02 = value; }
        public int U03 { get => u03; set => u03 = value; }
        public int U04 { get => u04; set => u04 = value; }
        public float U05 { get => u05; set => u05 = value; }
        public float? U06 { get => u06; set => u06 = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Int32(ref u01);

            if (version >= 6)
            {
                rw.Single(ref u02);
                rw.Int32(ref u03);
                rw.Int32(ref u04);
                rw.Single(ref u05);

                if (version >= 7)
                {
                    rw.Single(ref u06);
                }
            }
        }
    }
}
