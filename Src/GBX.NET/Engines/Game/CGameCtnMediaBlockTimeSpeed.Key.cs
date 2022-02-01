namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockTimeSpeed
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float speed;

        public float Speed { get => speed; set => speed = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref speed);
        }
    }
}
