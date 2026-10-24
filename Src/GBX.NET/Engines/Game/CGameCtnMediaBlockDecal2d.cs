namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockDecal2d
{
    public partial class Decal
    {
        [Obsolete("Use Transform instead.")]
        public Iso4 U01 { get => Transform; set => Transform = value; }

        [Obsolete("Use Opacity instead.")]
        public float U02 { get => Opacity; set => Opacity = value; }

        [Obsolete("Use FlipU instead.")]
        public bool U03 { get => FlipU; set => FlipU = value; }

        [Obsolete("Use ImageIndex instead.")]
        public int U04 { get => ImageIndex; set => ImageIndex = value; }
    }
}
