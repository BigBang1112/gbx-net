namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockFxBlurDepth
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float lensSize;
        private bool forceFocus;
        private float focusZ;

        public float LensSize { get => lensSize; set => lensSize = value; }
        public bool ForceFocus { get => forceFocus; set => forceFocus = value; }
        public float FocusZ { get => focusZ; set => focusZ = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            rw.Single(ref lensSize);
            rw.Boolean(ref forceFocus);
            rw.Single(ref focusZ);
        }
    }
}
