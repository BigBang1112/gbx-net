namespace GBX.NET.Engines.Plug;

public partial class CPlugSolid
{
    [Obsolete("Use PlugIdCounter instead.")]
    public int TypeAndIndex { get => PlugIdCounter; set => PlugIdCounter = value; }

    public partial class Chunk0900500D
    {
        [Obsolete("Use HasModel instead.")]
        public bool U02 { get => HasModel; set => HasModel = value; }
    }

    public partial class Chunk09005011
    {
        [Obsolete("Use HasModel instead.")]
        public bool U02 { get => HasModel; set => HasModel = value; }

        [Obsolete("Use ModelIsExternal instead.")]
        public bool? U03 { get => ModelIsExternal; set => ModelIsExternal = value; }
    }

    public partial class LocatedInstance
    {
        [Obsolete("Use ModelIndex instead.")]
        public int U01 { get => ModelIndex; set => ModelIndex = value; }

        [Obsolete("Use Loc instead.")]
        public Iso4 U02 { get => Loc; set => Loc = value; }
    }
}

