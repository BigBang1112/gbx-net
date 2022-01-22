namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockTransitionFade
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float opacity;

        public float Opacity { get => opacity; set => opacity = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref opacity);
        }
    }
}
