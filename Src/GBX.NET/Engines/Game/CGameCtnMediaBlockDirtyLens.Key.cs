namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockDirtyLens
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float intensity;

        public float Intensity { get => intensity; set => intensity = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref intensity);
        }
    }
}
