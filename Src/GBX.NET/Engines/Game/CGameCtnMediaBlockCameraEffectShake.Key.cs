namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockCameraEffectShake
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float intensity;
        private float speed;

        public float Intensity { get => intensity; set => intensity = value; }
        public float Speed { get => speed; set => speed = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref intensity);
            rw.Single(ref speed);
        }
    }
}
