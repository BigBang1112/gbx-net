namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockGhost
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float unknown;

        public float Unknown { get => unknown; set => unknown = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);
            rw.Single(ref unknown);
        }
    }
}
