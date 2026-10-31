namespace GBX.NET.Engines.Plug;

public partial class CPlugMaterialUserInst
{
    [Obsolete("Use Name instead.")]
    public string? MaterialName { get => Name; set => Name = value; }

    [Obsolete("Use CstValues instead.")]
    public int[]? Color { get => CstValues; set => CstValues = value; }

    public partial class Cst
    {
        [Obsolete("Use Name instead.")]
        public string? U01 { get => Name; set => Name = value; }

        [Obsolete("Use Type instead.")]
        public string? U02 { get => Type; set => Type = value; }

        [Obsolete("Use ValueCount instead.")]
        public int U03 { get => ValueCount; set => ValueCount = value; }
    }

    public partial class UvAnim
    {
        [Obsolete("Use Layer instead.")]
        public string? U01 { get => Layer; set => Layer = value; }

        [Obsolete("Use Type instead.")]
        public string? U02 { get => Type; set => Type = value; }

        [Obsolete("Use Period instead.")]
        public float U03 { get => Period; set => Period = value; }

        [Obsolete("Use Data instead.")]
        public ulong U04 { get => Data; set => Data = value; }

        [Obsolete("Use InputVal instead.")]
        public string? U05 { get => InputVal; set => InputVal = value; }
    }

    public partial class UserTexture
    {
        [Obsolete("Use Layer instead.")]
        public int U01 { get => Layer; set => Layer = value; }
    }
}
