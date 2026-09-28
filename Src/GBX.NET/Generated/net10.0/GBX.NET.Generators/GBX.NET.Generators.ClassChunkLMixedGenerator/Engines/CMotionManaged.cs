namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x0804E000</remarks>
[Class(0x0804E000)]
public abstract partial class CMotionManaged : CMotion, IClass
{
    [Hexadecimal] public static new uint Id => 0x0804E000;




    /// <summary>
    /// Creates a new instance of <see cref="CMotionManaged"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CMotionManaged() { }




}
