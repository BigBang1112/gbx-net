namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockCameraEffectScript
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float a;
        private float b;
        private float c;

        public float A { get => a; set => a = value; }
        public float B { get => b; set => b = value; }
        public float C { get => c; set => c = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref a);
            rw.Single(ref b);
            rw.Single(ref c);
        }
    }
}
