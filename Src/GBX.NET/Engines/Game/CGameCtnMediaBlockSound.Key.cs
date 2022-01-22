namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockSound
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float volume = 1;
        private float pan;
        private Vec3 position;

        public float Volume { get => volume; set => volume = value; }
        public float Pan { get => pan; set => pan = value; }
        public Vec3 Position { get => position; set => position = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref volume);
            rw.Single(ref pan);

            if (version >= 1)
            {
                rw.Vec3(ref position);
            }
        }
    }
}
