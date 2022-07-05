namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockDOF
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float zFocus;
        private float lensSize;
        private int? target;
        private Vec3? targetPosition;

        public float ZFocus { get => zFocus; set => zFocus = value; }
        public float LensSize { get => lensSize; set => lensSize = value; }
        public int? Target { get => target; set => target = value; }
        public Vec3? TargetPosition { get => targetPosition; set => targetPosition = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref zFocus);
            rw.Single(ref lensSize);

            if (version >= 1)
            {
                rw.Int32(ref target);

                if (version >= 2)
                {
                    rw.Vec3(ref targetPosition);
                }
            }
        }
    }
}
