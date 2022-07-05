namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockFxBloom
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float intensity;
        private float sensitivity;

        public float Intensity { get => intensity; set => intensity = value; }
        public float Sensitivity { get => sensitivity; set => sensitivity = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref intensity);
            rw.Single(ref sensitivity);
        }
    }
}
