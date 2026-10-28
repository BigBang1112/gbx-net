namespace GBX.NET.Engines.Plug;

public partial class CPlugSolid
{
    [Obsolete("Use PlugIdCounter instead.")]
    public int TypeAndIndex { get => PlugIdCounter; set => PlugIdCounter = value; }

    public partial class LocatedInstance
    {
        [Obsolete("Use ModelIndex instead.")]
        public int U01 { get => ModelIndex; set => ModelIndex = value; }

        [Obsolete("Use Loc instead.")]
        public Iso4 U02 { get => Loc; set => Loc = value; }
    }
}

