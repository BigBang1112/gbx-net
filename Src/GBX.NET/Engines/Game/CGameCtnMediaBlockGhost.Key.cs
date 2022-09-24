namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockGhost
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float lightIntensity;

        public float LightIntensity { get => lightIntensity; set => lightIntensity = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);
            rw.Single(ref lightIntensity);
        }
    }
}
