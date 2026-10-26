namespace GBX.NET.Engines.Function;

public partial class CFuncKeysSkel
{
    public partial class Loc
    {
        [Obsolete("Use Rotation instead.")]
        public Quat U01 { get => Rotation; set => Rotation = value; }

        [Obsolete("Use Position instead.")]
        public Vec3 U02 { get => Position; set => Position = value; }
    }
}
