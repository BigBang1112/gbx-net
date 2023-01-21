namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockSpectators
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float spectators;

        public float Spectators { get => spectators; set => spectators = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);
            rw.Single(ref spectators);
        }
    }
}
