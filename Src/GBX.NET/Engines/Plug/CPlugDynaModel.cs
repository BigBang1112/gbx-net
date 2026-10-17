namespace GBX.NET.Engines.Plug;

public partial class CPlugDynaModel
{
    [Obsolete("Use LinearMass instead.")]
    public float U01
    {
        get => LinearMass;
        set => LinearMass = value;
    }

    [Obsolete("Use MaxDistPerStep instead.")]
    public float U02
    {
        get => MaxDistPerStep;
        set => MaxDistPerStep = value;
    }

    [Obsolete("Use CenterOfMass.X instead.")]
    public float U03
    {
        get => CenterOfMass.X;
        set => CenterOfMass = CenterOfMass with { X = value };
    }

    [Obsolete("Use CenterOfMass.Y instead.")]
    public float U04
    {
        get => CenterOfMass.Y;
        set => CenterOfMass = CenterOfMass with { Y = value };
    }

    [Obsolete("Use CenterOfMass.Z instead.")]
    public float U05
    {
        get => CenterOfMass.Z;
        set => CenterOfMass = CenterOfMass with { Z = value };
    }

    [Obsolete("Use InverseInertiaMatrix.XX instead.")]
    public float U06
    {
        get => InverseInertiaMatrix.XX;
        set => InverseInertiaMatrix = InverseInertiaMatrix with { XX = value };
    }

    [Obsolete("Use InverseInertiaMatrix.XY instead.")]
    public float U07
    {
        get => InverseInertiaMatrix.XY;
        set => InverseInertiaMatrix = InverseInertiaMatrix with { XY = value };
    }

    [Obsolete("Use InverseInertiaMatrix.XZ instead.")]
    public float U08
    {
        get => InverseInertiaMatrix.XZ;
        set => InverseInertiaMatrix = InverseInertiaMatrix with { XZ = value };
    }

    [Obsolete("Use InverseInertiaMatrix.YX instead.")]
    public float U09
    {
        get => InverseInertiaMatrix.YX;
        set => InverseInertiaMatrix = InverseInertiaMatrix with { YX = value };
    }

    [Obsolete("Use InverseInertiaMatrix.YY instead.")]
    public float U10
    {
        get => InverseInertiaMatrix.YY;
        set => InverseInertiaMatrix = InverseInertiaMatrix with { YY = value };
    }

    [Obsolete("Use InverseInertiaMatrix.YZ instead.")]
    public float U11
    {
        get => InverseInertiaMatrix.YZ;
        set => InverseInertiaMatrix = InverseInertiaMatrix with { YZ = value };
    }

    [Obsolete("Use InverseInertiaMatrix.ZX instead.")]
    public float U12
    {
        get => InverseInertiaMatrix.ZX;
        set => InverseInertiaMatrix = InverseInertiaMatrix with { ZX = value };
    }

    [Obsolete("Use InverseInertiaMatrix.ZY instead.")]
    public float U13
    {
        get => InverseInertiaMatrix.ZY;
        set => InverseInertiaMatrix = InverseInertiaMatrix with { ZY = value };
    }

    [Obsolete("Use InverseInertiaMatrix.ZZ instead.")]
    public float U14
    {
        get => InverseInertiaMatrix.ZZ;
        set => InverseInertiaMatrix = InverseInertiaMatrix with { ZZ = value };
    }

    [Obsolete("Use AngularSpeedClamp instead.")]
    public float U15
    {
        get => AngularSpeedClamp;
        set => AngularSpeedClamp = value;
    }

    [Obsolete("Use UseTMSimulation instead.")]
    public bool U16
    {
        get => UseTMSimulation;
        set => UseTMSimulation = value;
    }

    [Obsolete("Use SleepingMethod instead.")]
    public byte U17
    {
        get => (byte)SleepingMethod;
        set => SleepingMethod = (ESleepingMethod)value;
    }

    [Obsolete("Use EnableSubStepping instead.")]
    public int U18
    {
        get => EnableSubStepping ? 1 : 0;
        set => EnableSubStepping = value != 0;
    }

    public override void ReadWrite(GbxReaderWriter rw)
    {
        ReadWrite(rw, v: 0);
    }
}
