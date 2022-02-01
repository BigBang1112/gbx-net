namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockScenery
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        public float U01;
        public float U02;
        public float U03;

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
        }
    }
}
