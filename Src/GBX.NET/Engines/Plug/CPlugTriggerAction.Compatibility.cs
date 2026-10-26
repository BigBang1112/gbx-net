namespace GBX.NET.Engines.Plug;

public partial class CPlugTriggerAction
{
    [Obsolete("Use EndYaw instead.")]
    public float U02 { get => EndYaw; set => EndYaw = value; }

    [Obsolete("Use EndPos.X instead.")]
    public float U03 { get => EndPos.X; set => EndPos = EndPos with { X = value }; }

    [Obsolete("Use EndPos.Y instead.")]
    public float U04 { get => EndPos.Y; set => EndPos = EndPos with { Y = value }; }

    [Obsolete("Use EndPos.Z instead.")]
    public float U05 { get => EndPos.Z; set => EndPos = EndPos with { Z = value }; }

    [Obsolete("Use ActionIndex instead.")]
    public int U07 { get => ActionIndex; set => ActionIndex = value; }

    [Obsolete("Use Type instead.")]
    public int U08 { get => (int)Type; set => Type = (EType)value; }

    [Obsolete("Use StartYaw instead.")]
    public float U10 { get => StartYaw; set => StartYaw = value; }

    [Obsolete("Use StartPos.X instead.")]
    public float U11 { get => StartPos.X; set => StartPos = StartPos with { X = value }; }

    [Obsolete("Use StartPos.Y instead.")]
    public float U12 { get => StartPos.Y; set => StartPos = StartPos with { Y = value }; }

    [Obsolete("Use StartPos.Z instead.")]
    public float U13 { get => StartPos.Z; set => StartPos = StartPos with { Z = value }; }

    [Obsolete("Use Radius instead.")]
    public float U14 { get => Radius; set => Radius = value; }

    [Obsolete("Use Height instead.")]
    public float U15 { get => Height; set => Height = value; }
}
