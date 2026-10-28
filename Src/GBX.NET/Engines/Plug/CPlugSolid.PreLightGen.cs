namespace GBX.NET.Engines.Plug;

public partial class CPlugSolid
{
    public partial class PreLightGen
    {
        [Obsolete("Use TileCountU instead.")]
        public int U01 { get => TileCountU; set => TileCountU = value; }

        [Obsolete("Use TileCountU instead.")]
        public int UvTileCount { get => TileCountU; set => TileCountU = value; }

        [Obsolete("Use MeterByUv instead.")]
        public float U02 { get => MeterByUv; set => MeterByUv = value; }

        [Obsolete("Use IsComputed instead.")]
        public bool U03 { get => IsComputed; set => IsComputed = value; }

        [Obsolete("Use UvBBox.X instead.")]
        public float U04 { get => UvBBox.X; set => UvBBox = UvBBox with { X = value }; }

        [Obsolete("Use UvBBox.Y instead.")]
        public float U05 { get => UvBBox.Y; set => UvBBox = UvBBox with { Y = value }; }

        [Obsolete("Use UvBBox.X2 instead.")]
        public float U06 { get => UvBBox.X2; set => UvBBox = UvBBox with { X2 = value }; }

        [Obsolete("Use UvBBox.Y2 instead.")]
        public float U07 { get => UvBBox.Y2; set => UvBBox = UvBBox with { Y2 = value }; }

        [Obsolete("Use UvBBoxSH.X instead.")]
        public float U08 { get => UvBBoxSH.X; set => UvBBoxSH = UvBBoxSH with { X = value }; }

        [Obsolete("Use UvBBoxSH.Y instead.")]
        public float U09 { get => UvBBoxSH.Y; set => UvBBoxSH = UvBBoxSH with { Y = value }; }

        [Obsolete("Use UvBBoxSH.X2 instead.")]
        public float U10 { get => UvBBoxSH.X2; set => UvBBoxSH = UvBBoxSH with { X2 = value }; }

        [Obsolete("Use UvBBoxSH.Y2 instead.")]
        public float U11 { get => UvBBoxSH.Y2; set => UvBBoxSH = UvBBoxSH with { Y2 = value }; }

        [Obsolete("Use SpriteCount.X instead.")]
        public int U12 { get => SpriteCount.X; set => SpriteCount = SpriteCount with { X = value }; }

        [Obsolete("Use SpriteCount.Y instead.")]
        public int U13 { get => SpriteCount.Y; set => SpriteCount = SpriteCount with { Y = value }; }

        [Obsolete("Use ProbeBoxes instead.")]
        public BoxAligned[]? U14 { get => ProbeBoxes; set => ProbeBoxes = value; }

        [Obsolete("Use UvGroups instead.")]
        public UvGroup[]? U15 { get => UvGroups; set => UvGroups = value; }
    }

    public partial class UvGroup
    {
        [Obsolete("Use MeterByUv instead.")]
        public float U01 { get => MeterByUv; set => MeterByUv = value; }

        [Obsolete("Use UvBBox.X instead.")]
        public float U02 { get => UvBBox.X; set => UvBBox = UvBBox with { X = value }; }

        [Obsolete("Use UvBBox.Y instead.")]
        public float U03 { get => UvBBox.Y; set => UvBBox = UvBBox with { Y = value }; }

        [Obsolete("Use UvBBox.X2 instead.")]
        public float U04 { get => UvBBox.X2; set => UvBBox = UvBBox with { X2 = value }; }

        [Obsolete("Use UvBBox.Y2 instead.")]
        public float U05 { get => UvBBox.Y2; set => UvBBox = UvBBox with { Y2 = value }; }
    }
}
