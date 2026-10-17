namespace GBX.NET.Engines.Plug;

public partial class CPlugDynaPointModel
{
    [Obsolete("Use Version instead.")]
    public int U01
    {
        get => Version;
        set => Version = value;
    }

    [Obsolete("Use Center.X instead. This member stores the float's bit representation.")]
    public int U02
    {
        get => BitConverter.ToInt32(BitConverter.GetBytes(Center.X), 0);
        set => Center = Center with { X = BitConverter.ToSingle(BitConverter.GetBytes(value), 0) };
    }

    [Obsolete("Use Center.Y instead. This member stores the float's bit representation.")]
    public int U03
    {
        get => BitConverter.ToInt32(BitConverter.GetBytes(Center.Y), 0);
        set => Center = Center with { Y = BitConverter.ToSingle(BitConverter.GetBytes(value), 0) };
    }

    [Obsolete("Use Center.Z instead. This member stores the float's bit representation.")]
    public int U04
    {
        get => BitConverter.ToInt32(BitConverter.GetBytes(Center.Z), 0);
        set => Center = Center with { Z = BitConverter.ToSingle(BitConverter.GetBytes(value), 0) };
    }

    [Obsolete("Use Radius instead. This member stores the float's bit representation.")]
    public int U05
    {
        get => BitConverter.ToInt32(BitConverter.GetBytes(Radius), 0);
        set => Radius = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
    }

    [Obsolete("Use Friction instead. This member stores the float's bit representation.")]
    public int U06
    {
        get => BitConverter.ToInt32(BitConverter.GetBytes(Friction), 0);
        set => Friction = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
    }

    [Obsolete("Use Restitution instead. This member stores the float's bit representation.")]
    public int U07
    {
        get => BitConverter.ToInt32(BitConverter.GetBytes(Restitution), 0);
        set => Restitution = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
    }

    [Obsolete("Use GravityCoef instead. This member stores the float's bit representation.")]
    public int U08
    {
        get => BitConverter.ToInt32(BitConverter.GetBytes(GravityCoef), 0);
        set => GravityCoef = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
    }
}
